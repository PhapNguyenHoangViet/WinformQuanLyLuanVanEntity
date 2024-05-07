using Group01_QuanLyLuanVan.Model;
using Group01_QuanLyLuanVan.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentScoreViewModel:BaseViewModel
    {
        public ICommand StudentScoreCM { get; set; }
        public StudentScoreViewModel()
        {

            StudentScoreCM = new RelayCommand<StudentScoreView>((p) => true, (p) => _Score(p));

        }
        void _Score(StudentScoreView studentScoreView)
        {
            StudentScoreView2 score = new StudentScoreView2();
            StudentMainViewModel.MainFrame.Content = score;
        }
    }
}
