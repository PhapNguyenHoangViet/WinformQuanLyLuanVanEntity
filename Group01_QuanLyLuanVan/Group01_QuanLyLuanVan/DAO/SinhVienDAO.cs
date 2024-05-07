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
    public class SinhVienDAO
    {
        DBConnection conn = new DBConnection();
        public void UpdateSinhVien(SinhVien sv)
        {
            string sqlStr = string.Format("update SinhVien set khoaId='{0}', hoTen=N'{1}', gioiTinh=N'{2}',ngaySinh='{3}', sdt='{4}', email='{5}', diaChi= N'{6}' where SinhVienId='{7}'", sv.KhoaId, sv.HoTen, sv.GioiTinh, sv.NgaySinh, sv.SDT, sv.Email, sv.DiaChi, sv.SinhVienId);
            conn.Sql_Them_Xoa_Sua(sqlStr);
        }
        public DataTable LoadListSinhVienDangKyDeTai()
        {
            DataTable dt = new DataTable();
            String sqlStr = string.Format("SELECT * FROM SinhVien WHERE khoaId = '{0}' AND nhomId IS NULL AND username != '{1}'", Const.sinhVien.KhoaId, Const.taiKhoan.Username);
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }
        public SinhVien FindOneByUsername(string username)
        {
            string sqlStr = string.Format("select * from SinhVien where username = '{0}'", username);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                int nhomId;
                if (dr["nhomId"].ToString() == "")
                    nhomId = -1;
                else
                    nhomId = int.Parse(dr["nhomId"].ToString());

                SinhVien sinhVien = new SinhVien(dr["sinhVienId"].ToString(), dr["hoTen"].ToString(), DateTime.Parse(dr["ngaySinh"].ToString()), dr["gioiTinh"].ToString(),
                dr["diaChi"].ToString(), dr["email"].ToString(), dr["sdt"].ToString(), dr["khoaId"].ToString(), dr["username"].ToString(), nhomId);
                return sinhVien;
            }
            else
            {
                return null;
            }
        }

        public void Register(SinhVien model)
        {
            string sqlStr = string.Format("Insert into SinhVien(id, hoTen, ngaySinh, gioiTinh, email, SDT, khoaId, username) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", model.Id, model.HoTen, model.NgaySinh, model.GioiTinh, model.Email, model.SDT, model.KhoaId, model.Username);
            conn.Sql_Them_Xoa_Sua(sqlStr);
        }

        public int FindNhomIDByDeTaiId(string deTaiId)
        {
            string sqlStr = string.Format("select nhomId from DeTai where deTaiId = '{0}'", deTaiId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                int nhomId;
                if (dr["nhomId"].ToString() == "")
                    nhomId = -1;
                else
                    nhomId = int.Parse(dr["nhomId"].ToString());

                return nhomId;
            }
            else
            {
                return -1;
            }
        }

        public string FindDeTaiIdByNhomID(int nhomId)
        {
            string sqlStr = string.Format("select deTaiId from DeTai where nhomId = '{0}'", nhomId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                string deTaiId = "";
                DataRow dr = tb.Rows[0];
                if (dr["deTaiId"].ToString() == "")
                    deTaiId = "";
                else
                    deTaiId = dr["deTaiId"].ToString();

                return deTaiId;
            }
            else
            {
                return "";
            }
        }
        public List<SinhVien> FindByDeTaiId(int nhom)
        {
            List<SinhVien> dsTK = new List<SinhVien>();
            string sqlStr = string.Format("(select * from SinhVien where nhomId = '{0}'", nhom);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    DataRow dr = tb.Rows[i];
                    int nhomId;
                    if (dr["nhomId"].ToString() == "")
                        nhomId = -1;
                    else
                        nhomId = int.Parse(dr["nhomId"].ToString());
                    SinhVien sinhVien = new SinhVien(dr["sinhVienId"].ToString(), dr["hoTen"].ToString(), DateTime.Parse(dr["ngaySinh"].ToString()), dr["gioiTinh"].ToString(),
                    dr["diaChi"].ToString(), dr["email"].ToString(), dr["sdt"].ToString(), dr["khoaId"].ToString(), dr["username"].ToString(), nhomId);
                    dsTK.Add(sinhVien);
                }
                return dsTK;
            }
            else
            {
                return null;
            }
        }



    }
}
