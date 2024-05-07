using Group01_QuanLyLuanVan.DAO;
using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Input;
using Group01_QuanLyLuanVan.View;
using Microsoft.Win32;
using System.Data.Common;
using System.ComponentModel;
using Group01_QuanLyLuanVan.Chat.Net;

namespace Group01_QuanLyLuanVan.ViewModel
{
    public class StudentUpdateTaskViewModel : BaseViewModel
    {
        TaiKhoanDAO tkDAO = new TaiKhoanDAO();

        YeuCauDAO ycDAO = new YeuCauDAO();
        MessageTaskDAO messageTaskDAO = new MessageTaskDAO();

        public ICommand ThemTask { get; set; }

        private ObservableCollection<YeuCau> _ListTask;
        public ObservableCollection<YeuCau> ListTask { get => _ListTask; set { _ListTask = value;/* OnPropertyChanged();*/ } }

        public ObservableCollection<YeuCau> Tasks { get; set; }
        public ICommand LoadTasksCommand { get; set; }
        public ICommand MessageTaskCommand { get; set; }
        private ObservableCollection<MessageTask> _ListMessage;
        public ObservableCollection<MessageTask> ListMessage
        {
            get { return _ListMessage ?? (_ListMessage = new ObservableCollection<MessageTask>()); }
            set { _ListMessage = value; }
        }

        public ObservableCollection<MessageTask> MessageTasks { get; set; }

        public StudentUpdateTaskViewModel()
        {
            Tasks = new ObservableCollection<YeuCau>();
            var tasksData = ycDAO.LoadListYeuCau();
            foreach (DataRow row in tasksData.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);


                Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            ListTask = Tasks;
            LoadTasksCommand = new RelayCommand<StudentUpdateTaskView>((p) => true, (p) => _LoadTasksCommand(p));
            ThemTask = new RelayCommand<StudentUpdateTaskView>((p) => true, (p) => _ThemTask(p));
            MessageTaskCommand = new RelayCommand<StudentUpdateTaskView>((p) => { return p.ListTaskView.SelectedItem == null ? false : true; }, (p) => _MessageTaskCommand(p));
        }

        void _MessageTaskCommand(StudentUpdateTaskView studentTaskDetailView)
        {


            StudentChatYeuCauView messageView = new StudentChatYeuCauView();
            YeuCau temp = (YeuCau)studentTaskDetailView.ListTaskView.SelectedItem;
            Const.yeuCauId = temp.YeuCauId;
            Const.YeuCau = temp;
            ////messageView.TenDeTai.Text = teacherTaskDetailView.TenDeTai.Text;
            messageView.TenTask.Text = temp.NoiDung;
            messageView.TienDo.Text = temp.TrangThai.ToString();
            MessageTasks = new ObservableCollection<MessageTask>();
            var messages = messageTaskDAO.LoadListMessageTask(temp.YeuCauId);
            foreach (DataRow row in messages.Rows)
            {
                int tinNhanId = int.Parse(row["tinNhanId"].ToString());
                string tinNhan = row["tinNhan"].ToString();
                DateTime thoiGian = DateTime.Parse(row["thoiGian"].ToString());
                string username = row["username"].ToString();
                int yeuCauId = Convert.ToInt32(row["yeuCauId"]);
                TaiKhoan tk = new TaiKhoan();
                tk = tkDAO.FindOneByUsername(username);
                string ava = Const._localLink + tk.Avatar;

                MessageTasks.Add(new MessageTask(tinNhanId, tinNhan, thoiGian, username, yeuCauId, ava));
            }

            messageView.ListMessageView.ItemsSource = MessageTasks;
            messageView.ListMessageView.SelectedItem = null;
            StudentMainViewModel.MainFrame.Content = messageView;
        }

        void _LoadTasksCommand(StudentUpdateTaskView tasksView)
        {
            tasksView.ListTaskView.ItemsSource = listTask();

        }
        ObservableCollection<YeuCau> listTask()
        {
            Tasks = new ObservableCollection<YeuCau>();
            var tasksData = ycDAO.LoadListYeuCau();
            foreach (DataRow row in tasksData.Rows)
            {
                int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                string noiDung = row["noiDung"].ToString();
                string deTaiId = row["deTaiId"].ToString();
                int trangThai = Convert.ToInt32(row["trangThai"]);


                Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
            }
            return Tasks;
        }

        void _ThemTask(StudentUpdateTaskView p)
        {
            if (string.IsNullOrWhiteSpace(p.ThemTask.Text))
            {
                MessageBox.Show("Vui lòng nhập nội dung cho task.");
                return;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.connStr))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction(); // Bắt đầu giao dịch

                    try
                    {
                        string sql = "SELECT deTaiId FROM SinhVien JOIN DeTai ON SinhVien.nhomId = DeTai.nhomId WHERE sinhVienId = @sinhVienId";

                        using (SqlCommand command = new SqlCommand(sql, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@sinhVienId", Const.sinhVien.SinhVienId);

                            object detaiId = command.ExecuteScalar();

                            if (detaiId != null)
                            {
                                string insertYeuCauQuery = "INSERT INTO YeuCau (noiDung, trangThai, deTaiId) VALUES (@noiDung, 0, @deTaiId)";
                                using (SqlCommand insertYeuCauCommand = new SqlCommand(insertYeuCauQuery, conn, transaction))
                                {
                                    insertYeuCauCommand.Parameters.AddWithValue("@noiDung", p.ThemTask.Text);
                                    insertYeuCauCommand.Parameters.AddWithValue("@deTaiId", detaiId);
                                    insertYeuCauCommand.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                MessageBox.Show("Thêm thành công");
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy dữ liệu.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    Tasks = new ObservableCollection<YeuCau>();
                    var tasksData = ycDAO.LoadListYeuCau();
                    foreach (DataRow row in tasksData.Rows)
                    {
                        int yeuCauId = int.Parse(row["yeuCauId"].ToString());
                        string noiDung = row["noiDung"].ToString();
                        string deTaiId = row["deTaiId"].ToString();
                        int trangThai = Convert.ToInt32(row["trangThai"]);


                        Tasks.Add(new YeuCau(yeuCauId, noiDung, trangThai, deTaiId));
                    }

                    p.ThemTask.Text = "";

                    StudentUpdateTaskView tasksView = new StudentUpdateTaskView();
                    tasksView.ListTaskView.ItemsSource = Tasks;
                    tasksView.ListTaskView.Items.Refresh();
                    StudentMainViewModel.MainFrame.Content = tasksView;
                }
            }
        }
    }
}
