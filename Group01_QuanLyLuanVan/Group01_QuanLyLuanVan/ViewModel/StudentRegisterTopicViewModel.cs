using System;
using Group01_QuanLyLuanVan.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.DAO;
using System.Data;
using System.Windows.Controls;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Navigation;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentRegisterTopicViewModel : BaseViewModel
    {

        DeTaiDAO dtDAO = new DeTaiDAO();

        public ObservableCollection<DeTai> Topics { get; set; }
        public ICommand Register { get; set; }

        public ICommand back { get; set; }
        public StudentRegisterTopicViewModel()
        {
            back = new RelayCommand<StudentRegisterTopicView>((p) => true, p => _back(p));
            Register = new RelayCommand<StudentRegisterTopicView>((p) => true, (p) => _Register(p));
        }

        void _back(StudentRegisterTopicView paramater)
        {
            StudentListTopicView topicsView = new StudentListTopicView();
            StudentMainViewModel.MainFrame.Content = topicsView;
        }

        void _Register(StudentRegisterTopicView p)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("  Bạn muốn đăng ký đề tài ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                int nhomId = GetNextNhomId(); // Lấy giá trị nhomId tiếp theo


                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
                {
                    conn.Open();
                    string selectNhomIdQuery = String.Format("SELECT NhomId FROM SinhVien WHERE Username = '{0}'", Const.taiKhoan.Username);
                    SqlCommand selectNhomIdCommand = new SqlCommand(selectNhomIdQuery, conn);
                    object nhomIdResult = selectNhomIdCommand.ExecuteScalar();

                    string selectTrangThaiQuery = String.Format("SELECT  d.TrangThai FROM SinhVien s INNER JOIN detai d ON s.nhomId = d.nhomId WHERE Username = '{0}'", Const.taiKhoan.Username);
                    SqlCommand selectTrangThaiCommand = new SqlCommand(selectTrangThaiQuery, conn);
                    object result = selectTrangThaiCommand.ExecuteScalar();
                    int TrangThaiResult = result != null ? Convert.ToInt32(result) : 0;

                    if (nhomIdResult != DBNull.Value && TrangThaiResult != 2)
                    {
                        MessageBox.Show("Bạn đã đăng ký đề tài trước đó rồi!");
                    }
                    else if (p.TenTrangThai.Text == "Đã đăng ký")
                    {
                        MessageBox.Show("Đề tài đã có nhóm đăng ký, vui lòng đăng ký đề tài khác!");
                    }
                    else
                    {
                        SqlTransaction transaction = conn.BeginTransaction();

                        try
                        {
                            // Thực thi truy vấn SQL để chèn nhomId vào bảng Nhom
                            string insertNhomQuery = "INSERT INTO Nhom (nhomId) VALUES (@nhomId)";
                            SqlCommand insertNhomCommand = new SqlCommand(insertNhomQuery, conn, transaction);
                            insertNhomCommand.Parameters.AddWithValue("@nhomId", nhomId);
                            insertNhomCommand.ExecuteNonQuery();

                            // Thực thi truy vấn SQL để cập nhật trạng thái trong bảng DeTai thành 1
                            string updateDeTaiQuery = "UPDATE DeTai SET trangThai = 1, nhomId = @nhomId WHERE deTaiId = @deTaiId";
                            SqlCommand updateDeTaiCommand = new SqlCommand(updateDeTaiQuery, conn, transaction);
                            updateDeTaiCommand.Parameters.AddWithValue("@nhomId", nhomId);
                            updateDeTaiCommand.Parameters.AddWithValue("@deTaiId", p.deTaiId.Text); // Lấy giá trị từ TextBox deTaiId
                            updateDeTaiCommand.ExecuteNonQuery();


                            string updateSinhVienUserQuery = String.Format("UPDATE SinhVien SET nhomId = @nhomId WHERE Username = '{0}'", Const.taiKhoan.Username);
                            SqlCommand updateSinhVienUserCommand = new SqlCommand(updateSinhVienUserQuery, conn, transaction);
                            updateSinhVienUserCommand.Parameters.AddWithValue("@nhomId", nhomId);
                            Const.sinhVien.NhomId = nhomId;
                            updateSinhVienUserCommand.ExecuteNonQuery();

                            // Commit transaction
                            transaction.Commit();

                            //MessageBox.Show("Đã cập nhật trạng thái và thêm nhomId vào bảng DeTai và cơ sở dữ liệu.");
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction nếu có lỗi
                            transaction.Rollback();

                            MessageBox.Show("Lỗi khi thực hiện cập nhật trạng thái và thêm nhomId vào bảng DeTai và cơ sở dữ liệu: " + ex.Message);
                        }

                        string sinhVienId = "";
                        foreach (SinhVien sinhVien in p.multiSelectComboBox.ItemsSource)
                        {
                            if (sinhVien.IsSelected)
                            {
                                sinhVienId = sinhVien.SinhVienId;

                                if (string.IsNullOrEmpty(sinhVienId))
                                {
                                    MessageBox.Show("Vui lòng chọn một sinh viên.");
                                    return;
                                }

                                // Thực thi truy vấn SQL để cập nhật nhomId trong bảng SinhVien
                                string updateSinhVienQuery = "UPDATE SinhVien SET nhomId = @nhomId WHERE sinhVienId = @sinhVienId";
                                SqlCommand updateSinhVienCommand = new SqlCommand(updateSinhVienQuery, conn);
                                updateSinhVienCommand.Parameters.AddWithValue("@nhomId", nhomId);
                                updateSinhVienCommand.Parameters.AddWithValue("@sinhVienId", sinhVienId);


                                try
                                {
                                    // Thực thi truy vấn cập nhật nhomId trong bảng SinhVien
                                    updateSinhVienCommand.ExecuteNonQuery();

                                    //MessageBox.Show("Đã cập nhật nhomId cho sinh viên thành công.");


                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Lỗi khi cập nhật nhomId cho sinh viên: " + ex.Message);
                                }

                            }

                        }
                        MessageBox.Show("Đăng ký thành công.");

                    }
                }

                //DataProvider.Ins.DB.SaveChanges();
                //MessageBox.Show("Cập nhật thông tin thành công !", "THÔNG BÁO");
                Topics = new ObservableCollection<DeTai>();
                var topicsData = dtDAO.LoadListTopicCoGV();
                foreach (DataRow row in topicsData.Rows)
                {
                    string deTaiId = row["deTaiId"].ToString();
                    string tenDeTai = row["tenDeTai"].ToString();
                    string tenTheLoai = row["tenTheLoai"].ToString();
                    string hoTen = row["hoTen"].ToString();
                    string moTa = row["moTa"].ToString();
                    string yeuCauChung = row["yeuCauChung"].ToString();
                    DateTime ngayBatDau = Convert.ToDateTime(row["ngayBatDau"]);
                    DateTime ngayKetThuc;
                    try
                    {
                        ngayKetThuc = Convert.ToDateTime(row["ngayKetThuc"]);
                    }
                    catch
                    {
                        ngayKetThuc = Convert.ToDateTime(row["ngayBatDau"]);
                    }
                    int soLuong = Convert.ToInt32(row["soLuong"]);
                    int trangThai = Convert.ToInt32(row["trangThai"]);
                    int an = Convert.ToInt32(row["an"]);
                    string tenTrangThai = "";

                    if (trangThai == 1)
                    {
                        tenTrangThai = "Đã đăng ký";
                    }
                    else
                    {
                        tenTrangThai = "Chưa đăng ký";
                    }
                    if (an != 1)
                        Topics.Add(new DeTai(deTaiId, tenDeTai, tenTheLoai, hoTen, moTa, yeuCauChung, ngayBatDau, ngayKetThuc, soLuong, tenTrangThai));
                }
                StudentListTopicView studentListTopicView = new StudentListTopicView();
                studentListTopicView.ListTopicView.ItemsSource = Topics;
                StudentMainViewModel.MainFrame.Content = studentListTopicView;

            }
        }

        private int GetNextNhomId()
        {
            int nextNhomId = 0; // Giá trị mặc định nếu bảng Nhom chưa có dữ liệu


            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
            {
                conn.Open();

                // Truy vấn SQL để lấy giá trị nhomId lớn nhất trong bảng Nhom
                string selectQuery = "SELECT ISNULL(MAX(nhomId), 0) FROM Nhom";
                SqlCommand command = new SqlCommand(selectQuery, conn);

                // Thực thi truy vấn và lấy giá trị nhomId lớn nhất
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    nextNhomId = Convert.ToInt32(result) + 1;
                }
            }

            return nextNhomId;
        }

    }
}
