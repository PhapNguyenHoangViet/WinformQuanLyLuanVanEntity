using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
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
        public int nhomId { get; set; }
        public string theLoaiId { get; set; }
        public string giangVienId { get; set; }

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
                try
                {
                    var theLoai = DataProvider.Ins.DB.TheLoais.FirstOrDefault(tl => tl.tenTheLoai == p.TheLoai.Text);
                    if (theLoai != null)
                    {
                        string theLoaiId = theLoai.theLoaiId;
                    }


                    var giangVien = DataProvider.Ins.DB.GiangViens.FirstOrDefault(gv => gv.hoTen == p.GiangVien.Text);
                    if (giangVien != null)
                    {
                        string giangVienId = giangVien.giangVienId;
                    }


                    Nhom nhom = new Nhom
                    {
                        nhomId = nhomId
                    };

                    DataProvider.Ins.DB.Nhoms.Add(nhom);
                    DataProvider.Ins.DB.SaveChanges();

                    DeTai newDeTai = new DeTai
                    {
                        tenDeTai = p.TenDeTai.Text,
                        moTa = p.MoTa.Text,
                        yeuCauChung = p.YeuCau.Text,
                        trangThai = 2,
                        ngayBatDau = DateTime.Today,
                        an = 0,
                        nhomId = nhomId,
                        theLoaiId = theLoaiId,
                        giangVienId = giangVienId,
                        diem = 0
                    };

                    DataProvider.Ins.DB.DeTais.Add(newDeTai);
                    DataProvider.Ins.DB.SaveChanges();



                    var sinhVienToUpdate = DataProvider.Ins.DB.SinhViens.FirstOrDefault(sv => sv.username == Const.taiKhoan.username);

                    if (sinhVienToUpdate != null)
                    {
                        sinhVienToUpdate.nhomId = nhomId;

                        DataProvider.Ins.DB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thực hiện đề xuất đề tài!" + ex.Message);
                }
                int selectedCount = 1;
                string sinhVienId = "";
                foreach (SinhVien sinhVien in p.multiSelectComboBox.ItemsSource)
                {
                    if (sinhVien.IsSelected)
                    {
                        selectedCount++;
                        sinhVienId = sinhVien.sinhVienId;

                        if (string.IsNullOrEmpty(sinhVienId))
                        {
                            MessageBox.Show("Vui lòng chọn một sinh viên.");
                            return;
                        }

                        var sinhVienToUpdate = DataProvider.Ins.DB.SinhViens.FirstOrDefault(sv => sv.sinhVienId == sinhVienId);

                        if (sinhVienToUpdate != null)
                        {
                            sinhVienToUpdate.nhomId = nhomId;
                            DataProvider.Ins.DB.SaveChanges();
                        }

                    }

                    var deTais = DataProvider.Ins.DB.DeTais.Where(dt => dt.nhomId == nhomId);
                    foreach (var deTai in deTais)
                    {
                        deTai.soLuong = selectedCount;
                    }
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Đề xuất thành công.");
                }

            }
            StudentListTopicView studentListTopicView = new StudentListTopicView();
            StudentMainViewModel.MainFrame.Content = studentListTopicView;

        }

        private int GetNextNhomId()
        {
            int nextNhomId = DataProvider.Ins.DB.Nhoms.Select(n => n.nhomId).DefaultIfEmpty(0).Max() + 1;
            return nextNhomId;
        }
    }
}
