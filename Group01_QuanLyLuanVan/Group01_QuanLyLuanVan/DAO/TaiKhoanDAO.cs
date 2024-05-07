using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Group01_QuanLyLuanVan.DAO
{
    public class TaiKhoanDAO
    {
        DBConnection dbcon = new DBConnection();
        public DataTable KiemTraThongTinTaiKhoan(string username, string passowrd)
        {
            string sqlStr = string.Format("(select * from TaiKhoan where username='{0}' and password='{1}' ) ", username, passowrd);
            return dbcon.Sql_Select(sqlStr);
        }

        public TaiKhoan FindOneByUsername(string username)
        {
            string sqlStr = string.Format("(select * from TaiKhoan where username='{0}') ", username);
            DataTable tb = dbcon.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                TaiKhoan taiKhoan = new TaiKhoan(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), int.Parse(dr[3].ToString()), int.Parse(dr[4].ToString()), dr[5].ToString(), dr[6].ToString());
                return taiKhoan;
            }
            else
            {
                return null;
            }
        }

        public TaiKhoan FindOneByMail(string mail)
        {
            string sqlStr = string.Format("(select * from TaiKhoan where mail='{0}') ", mail);
            DataTable tb = dbcon.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                TaiKhoan taiKhoan = new TaiKhoan(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), int.Parse(dr[3].ToString()), int.Parse(dr[4].ToString()), dr[5].ToString(), dr[6].ToString());
                return taiKhoan;
            }
            else
            {
                return null;
            }
        }

        public TaiKhoan FindOne(string username, string passowrd)
        {
            string sqlStr = string.Format("(select * from TaiKhoan where username='{0}' and password='{1}' ) ", username, passowrd);
            DataTable tb = dbcon.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                TaiKhoan taiKhoan = new TaiKhoan(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), int.Parse(dr[3].ToString()), int.Parse(dr[4].ToString()), dr[5].ToString(), dr[6].ToString());
                return taiKhoan;
            }
            else
            {
                return null;
            }
        }

        public void Register(TaiKhoan model)
        {
            string sqlStr = string.Format("Insert into TaiKhoan values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", model.Username, model.Password, model.Mail, model.Quyen, model.TrangThai, model.Code, model.Avatar);
            dbcon.Sql_Them_Xoa_Sua(sqlStr);
        }



        public List<TaiKhoan> FindAll()
        {
            List<TaiKhoan> dsTK = new List<TaiKhoan>();
            string sqlStr = string.Format("(select * from TaiKhoan) ");
            DataTable tb = dbcon.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    dsTK.Add(new TaiKhoan(tb.Rows[i][0].ToString(), tb.Rows[i][1].ToString(), tb.Rows[i][2].ToString(), int.Parse(tb.Rows[i][3].ToString()), int.Parse(tb.Rows[i][4].ToString()), tb.Rows[i][5].ToString(), tb.Rows[i][6].ToString()));
                }
                return dsTK;
            }
            else
            {
                return null;
            }
        }

        public void UpdateTaiKhoan(string mail, string avatar, string username)
        {
            string sqlStr = string.Format("update TaiKhoan set mail='{0}', avatar='{1}' where username='{2}'", mail, avatar, username);
            dbcon.Sql_Them_Xoa_Sua(sqlStr);
        }

        public void ChangePassword(string username, string matKhauMoi)
        {
            string sqlStr = string.Format("update TaiKhoan set password='{0}' where username='{1}'", matKhauMoi, username);
            dbcon.Sql_Them_Xoa_Sua(sqlStr);
        }

        public void UpdateStatus(string username)
        {
            string sqlStr = string.Format("update TaiKhoan set trangThai='{0}' where username='{1}'", 1, username);
            dbcon.Sql_Them_Xoa_Sua(sqlStr);
        }

        public void Delete(string username)
        {
            string sqlStr = string.Format("select * from TaiKhoan where username = '{0}'", username);
            DataTable tb = dbcon.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                sqlStr = string.Format("Delete from TaiKhoan where username = '{0}'", username);
                dbcon.Sql_Them_Xoa_Sua(sqlStr);
            }
            else
            {
                MessageBox.Show("Tài khoản không tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
