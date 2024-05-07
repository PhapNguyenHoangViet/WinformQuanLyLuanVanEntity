using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{

    public class StudentAddTopicsViewModel : BaseViewModel, INotifyPropertyChanged
    {



        public ICommand back { get; set; }
        public ICommand DeXuatCommand { get; set; }
        public StudentAddTopicsViewModel()
        {

            back = new RelayCommand<StudentAddTopicsView>((p) => true, p => _back(p));
            DeXuatCommand = new RelayCommand<StudentAddTopicsView>((p) => true, p => _DeXuatCommand(p));

        }

        void _back(StudentAddTopicsView paramater)
        {
            StudentListTopicView topicsView = new StudentListTopicView();
            StudentMainViewModel.MainFrame.Content = topicsView;
        }

        void _DeXuatCommand(StudentAddTopicsView p)
        {
            int nhomId = GetNextNhomId();
            if (string.IsNullOrWhiteSpace(p.TenDeTai.Text) ||
            p.GiangVien.SelectedItem == null ||
            p.TheLoai.SelectedItem == null ||
            string.IsNullOrWhiteSpace(p.MoTa.Text) ||
            string.IsNullOrWhiteSpace(p.YeuCau.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin vào các trường");
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string selectTheLoaiIdQuery = String.Format("SELECT theLoaiId FROM TheLoai WHERE TenTheLoai = N'{0}'", p.TheLoai.Text);
                        SqlCommand selectTheLoaiIdCommand = new SqlCommand(selectTheLoaiIdQuery, conn, transaction);
                        string theLoaiId = (string)selectTheLoaiIdCommand.ExecuteScalar();


                        string selectGiangVienIdQuery = String.Format("SELECT GiangVienId FROM GiangVien WHERE hoten = N'{0}'", p.GiangVien.Text);
                        SqlCommand selectGiangVienIdCommand = new SqlCommand(selectGiangVienIdQuery, conn, transaction);
                        string giangVienId = (string)selectGiangVienIdCommand.ExecuteScalar();


                        string insertNhomQuery = "INSERT INTO Nhom (nhomId) VALUES (@nhomId)";
                        SqlCommand insertNhomCommand = new SqlCommand(insertNhomQuery, conn, transaction);
                        insertNhomCommand.Parameters.AddWithValue("@nhomId", nhomId);
                        insertNhomCommand.ExecuteNonQuery();

                        string insertDeTaiQuery = "INSERT INTO DeTai (tenDeTai, moTa, yeuCauChung,trangThai, ngayBatDau,theLoaiId,nhomId, giangVienId, an, diem) VALUES (@tenDeTai, @moTa, @yeuCauChung, @trangThai, @ngayBatDau,@theLoaiId, @nhomId, @giangVienId, @an, @diem)";
                        SqlCommand insertDeTaiCommand = new SqlCommand(insertDeTaiQuery, conn, transaction);
                        insertDeTaiCommand.Parameters.AddWithValue("@tenDeTai", p.TenDeTai.Text);
                        insertDeTaiCommand.Parameters.AddWithValue("@moTa", p.MoTa.Text);
                        insertDeTaiCommand.Parameters.AddWithValue("@yeuCauChung", p.YeuCau.Text);
                        insertDeTaiCommand.Parameters.AddWithValue("@trangThai", 2);
                        insertDeTaiCommand.Parameters.AddWithValue("@ngayBatDau", DateTime.Today);
                        insertDeTaiCommand.Parameters.AddWithValue("@an", 0);
                        insertDeTaiCommand.Parameters.AddWithValue("@nhomId", nhomId);
                        insertDeTaiCommand.Parameters.AddWithValue("@theLoaiId", theLoaiId);
                        insertDeTaiCommand.Parameters.AddWithValue("@giangVienId", giangVienId);
                        insertDeTaiCommand.Parameters.AddWithValue("@diem", 0);

                        //insertDeTaiCommand.Parameters.AddWithValue("@soLuong", _selectedCount);
                        insertDeTaiCommand.ExecuteNonQuery();


                        string updateSinhVienUserQuery = String.Format("UPDATE SinhVien SET nhomId = @nhomId WHERE Username = '{0}'", Const.taiKhoan.Username);
                        SqlCommand updateSinhVienUserCommand = new SqlCommand(updateSinhVienUserQuery, conn, transaction);
                        updateSinhVienUserCommand.Parameters.AddWithValue("@nhomId", nhomId);
                        Const.sinhVien.NhomId = nhomId;
                        updateSinhVienUserCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        MessageBox.Show("Lỗi khi thực hiện đề xuất đề tài!" + ex.Message);
                    }
                    int selectedCount = 1;
                    string sinhVienId = "";
                    foreach (SinhVien sinhVien in p.multiSelectComboBox.ItemsSource)
                    {
                        if (sinhVien.IsSelected)
                        {
                            selectedCount++;

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
                    string updateDeTaiQuery = "UPDATE DeTai SET soluong = @SoLuong WHERE NhomId = @nhomId";
                    SqlCommand updateSoLuongCommand = new SqlCommand(updateDeTaiQuery, conn);
                    updateSoLuongCommand.Parameters.AddWithValue("@nhomId", nhomId);
                    updateSoLuongCommand.Parameters.AddWithValue("@SoLuong", selectedCount);
                    updateSoLuongCommand.ExecuteNonQuery();
                    MessageBox.Show("Đề xuất thành công.");
                }



            }
            StudentListTopicView studentListTopicView = new StudentListTopicView();
            StudentMainViewModel.MainFrame.Content = studentListTopicView;

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
