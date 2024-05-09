using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class TeacherScoreDetailViewModel : BaseViewModel
    {
        public ICommand back { get; set; }
        public ICommand ScoreDetailCM { get; set; }
        
        void _back(TeacherScoreDetailView p)
        {
            TeacherScoreView taskView = new TeacherScoreView();
            TeacherMainViewModel.MainFrame.Content = taskView;
        }
        void _ScoreDetailCM(TeacherScoreDetailView p)
        {
            string score = p.score.Text;
            if (score == "")
            {
                MessageBox.Show("Vui lòng nhập điểm!");
                return;
            }
            else
            {
                var dt = DataProvider.Ins.DB.DeTais.FirstOrDefault(x => x.deTaiId == Const.deTaiId);
                if (dt != null)
                {
                    dt.diem = float.Parse(score);
                    DataProvider.Ins.DB.SaveChanges();
                }
            }
                

            TeacherScoreView taskView = new TeacherScoreView();
            TeacherMainViewModel.MainFrame.Content = taskView;
        }
        public TeacherScoreDetailViewModel()
        {
            back = new RelayCommand<TeacherScoreDetailView>((p) => true, p => _back(p));

            ScoreDetailCM = new RelayCommand<TeacherScoreDetailView>((p) => true, p => _ScoreDetailCM(p));

        }
    }
}
