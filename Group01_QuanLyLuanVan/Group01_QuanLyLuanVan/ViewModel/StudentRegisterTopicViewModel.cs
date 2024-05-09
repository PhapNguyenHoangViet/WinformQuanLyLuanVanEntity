using System;
using Group01_QuanLyLuanVan.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Group01_QuanLyLuanVan.Model;
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

        public int nhomId { get; set; }

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
                int nhomId = GetNextNhomId();
                var nhomIdResult = DataProvider.Ins.DB.SinhViens
                    .Where(sv => sv.username == Const.taiKhoan.username)
                    .Select(sv => sv.nhomId)
                    .FirstOrDefault();

                var result = DataProvider.Ins.DB.SinhViens
                    .Where(sv => sv.username == Const.taiKhoan.username)
                    .Join(DataProvider.Ins.DB.DeTais,
                          sv => sv.nhomId,
                          d => d.nhomId,
                          (sv, d) => d.trangThai)
                    .FirstOrDefault();
                int TrangThaiResult = result != null ? Convert.ToInt32(result) : 0;

                if (nhomIdResult != null && TrangThaiResult != 2)
                {
                    MessageBox.Show("Bạn đã đăng ký đề tài trước đó rồi!");
                }
                else if (p.TenTrangThai.Text == "Đã đăng ký")
                {
                    MessageBox.Show("Đề tài đã có nhóm đăng ký, vui lòng đăng ký đề tài khác!");
                }
                else
                {
                    try
                    {
                        Nhom newNhom = new Nhom
                        {
                            nhomId = nhomId
                        };
                        DataProvider.Ins.DB.Nhoms.Add(newNhom);
                        DataProvider.Ins.DB.SaveChanges();

                        var deTai = DataProvider.Ins.DB.DeTais.FirstOrDefault(dt => dt.deTaiId == p.deTaiId.Text);
                        if (deTai != null)
                        {
                            deTai.trangThai = 1;
                            deTai.nhomId = nhomId;
                            DataProvider.Ins.DB.SaveChanges();
                        }

                        var sinhVien = DataProvider.Ins.DB.SinhViens.FirstOrDefault(sv => sv.username == Const.taiKhoan.username);
                        if (sinhVien != null)
                        {
                            sinhVien.nhomId = nhomId;
                            DataProvider.Ins.DB.SaveChanges();
                        }

                    }
                    catch
                    {
                        MessageBox.Show("Lỗi khi thực hiện cập nhật trạng thái và thêm nhomId vào bảng DeTai và cơ sở dữ liệu");

                    }
                    string sinhVienId = "";
                    foreach (SinhVien sinhVien in p.multiSelectComboBox.ItemsSource)
                    {
                        if (sinhVien.IsSelected)
                        {
                            sinhVienId = sinhVien.sinhVienId;

                            if (string.IsNullOrEmpty(sinhVienId))
                            {
                                MessageBox.Show("Vui lòng chọn một sinh viên.");
                                return;
                            }

                            var svien = DataProvider.Ins.DB.SinhViens.FirstOrDefault(sv => sv.sinhVienId == sinhVienId);
                            if (svien != null)
                            {
                                svien.nhomId = nhomId;
                                DataProvider.Ins.DB.SaveChanges();
                            }
                        }

                    }
                    MessageBox.Show("Đăng ký thành công.");
                }


                Topics = new ObservableCollection<DeTai>();

                var topicsData = DataProvider.Ins.DB.DeTais
                .Where(dt => dt.GiangVien.khoaId == Const.sinhVien.khoaId && dt.trangThai != 2 && dt.an != 1)
                .ToList();

                foreach (DeTai dt in topicsData)
                {
                    string deTaiId = dt.deTaiId;
                    string tenDeTai = dt.tenDeTai;
                    string tenTheLoai = dt.TheLoai.tenTheLoai;
                    string hoTen = dt.GiangVien.hoTen;
                    string moTa = dt.moTa;
                    string yeuCauChung = dt.yeuCauChung;
                    DateTime ngayBatDau = Convert.ToDateTime(dt.ngayBatDau);
                    DateTime ngayKetThuc;
                    try
                    {
                        ngayKetThuc = Convert.ToDateTime(dt.ngayKetThuc);
                    }
                    catch
                    {
                        ngayKetThuc = Convert.ToDateTime(dt.ngayBatDau);
                    }
                    int soLuong = Convert.ToInt32(dt.soLuong);
                    int trangThai = Convert.ToInt32(dt.trangThai);
                    int an = Convert.ToInt32(dt.an);
                    string tenTrangThai = "";

                    if (trangThai == 1)
                    {
                        tenTrangThai = "Đã đăng ký";
                    }
                    else if (trangThai == 0)
                    {
                        tenTrangThai = "Chưa đăng ký";
                    }
                    else
                    {
                        tenTrangThai = "Đề xuất";
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
            int nextNhomId = 0;
            int maxNhomId = DataProvider.Ins.DB.Nhoms.Select(n => n.nhomId).DefaultIfEmpty(0).Max();
            nextNhomId = maxNhomId + 1;
            return nextNhomId;
        }

    }
}
