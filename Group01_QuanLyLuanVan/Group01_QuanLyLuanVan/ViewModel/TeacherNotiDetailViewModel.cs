﻿using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherNotiDetailViewModel:BaseViewModel
    {
        public ICommand back { get; set; }

        private ObservableCollection<ThongBao> _ListThongBao;
        public ObservableCollection<ThongBao> ListThongBao { get => _ListThongBao; set { _ListThongBao = value; OnPropertyChanged(); } }
        public ICommand DetailThongBaoCommand { get; set; }
        public ObservableCollection<ThongBao> thongBaos { get; set; }
        public ICommand LoadListNotiCommand { get; set; }

        private string _selectedThongBaoNoiDung;
        public string SelectedThongBaoNoiDung
        {
            get { return _selectedThongBaoNoiDung; }
            set
            {
                _selectedThongBaoNoiDung = value;
                OnPropertyChanged(nameof(SelectedThongBaoNoiDung));
            }
        }
        public ICommand AddNoti { get; set; }


        public TeacherNotiDetailViewModel()
        {
            back = new RelayCommand<TeacherNotiDetailView>((p) => true, p => _back(p));
            LoadListNotiCommand = new RelayCommand<TeacherNotiDetailView>((p) => true, (p) => _LoadListNotiCommand(p));
            DetailThongBaoCommand = new RelayCommand<TeacherNotiDetailView>((p) => { return p.ListThongBaoView.SelectedItem == null ? false : true; }, (p) => _DetailThongBaoCommand(p));
            AddNoti = new RelayCommand<TeacherNotiDetailView>((p) => true, (p) => _AddNoti(p));
        }
        void _AddNoti(TeacherNotiDetailView teacherNotiDetailView)
        {
            TeacherNotiAddView teacherNotiAddView = new TeacherNotiAddView();
            TeacherMainViewModel.MainFrame.Content = teacherNotiAddView;
        }

        void _DetailThongBaoCommand(TeacherNotiDetailView tbView)
        {
            if (tbView != null && tbView.ListThongBaoView.SelectedItem != null)
            {
                var selectedThongBao = (ThongBao)tbView.ListThongBaoView.SelectedItem;
                SelectedThongBaoNoiDung = selectedThongBao.noiDung;
            }
        }

        void _back(TeacherNotiDetailView paramater)
        {
            TeacherNotiView taskView = new TeacherNotiView();
            TeacherMainViewModel.MainFrame.Content = taskView;
        }

        void _LoadListNotiCommand(TeacherNotiDetailView topicsView)
        {
            topicsView.ListThongBaoView.ItemsSource = listNoti();
            topicsView.ListThongBaoView.Items.Refresh();

            var dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == Const.deTaiId);
            if (dt != null)
            {
                topicsView.TenDeTai.Text = dt.tenDeTai;
            }
        }

        ObservableCollection<ThongBao> listNoti()
        {
            thongBaos = new ObservableCollection<ThongBao>();

            var thongBaosData = DataProvider.Ins.DB.ThongBaos
                        .Where(tb => tb.deTaiId == Const.deTaiId)
                        .ToList();
            foreach (ThongBao tb in thongBaosData)
            {
                int thongBaoId = Convert.ToInt32(tb.thongBaoId);
                string tieuDe = (tb.tieude).ToString();
                string noiDung = (tb.noiDung).ToString();
                string deTaiId = (tb.deTaiId).ToString();
                DateTime ngay = Convert.ToDateTime(tb.ngay);

                thongBaos.Add(new ThongBao(thongBaoId, tieuDe, noiDung, deTaiId, ngay));
            }
            return thongBaos;
        }
    }
}
