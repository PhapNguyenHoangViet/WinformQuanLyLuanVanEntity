using Group01_QuanLyLuanVan.DAO;
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
        DeTaiDAO detaiDAO = new DeTaiDAO();
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
                detaiDAO.UpdateScoreTopic(float.Parse(score), Const.deTaiId);

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
