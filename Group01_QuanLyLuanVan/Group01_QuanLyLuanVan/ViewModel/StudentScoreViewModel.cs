using Group01_QuanLyLuanVan.View;
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
