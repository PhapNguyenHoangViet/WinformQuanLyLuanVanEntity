using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.DAO
{
    public class KhoaDAO
    {
        DBConnection conn = new DBConnection();
        public Khoa FindByKhoaId(string khoaId)
        {
            string sqlStr = string.Format("select khoaId, tenKhoa from Khoa where khoaId = '{0}'", khoaId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                Khoa tl = new Khoa(dr[0].ToString(), dr[1].ToString());
                return tl;
            }
            else
            {
                return null;
            }
        }

        public List<Khoa> FindAll()
        {
            List<Khoa> dsKhoa = new List<Khoa>();
            string sqlStr = string.Format("SELECT * from Khoa");
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    dsKhoa.Add(new Khoa(tb.Rows[i]["khoaId"].ToString(), tb.Rows[i]["tenKhoa"].ToString()));
                }
                return dsKhoa;
            }
            else
            {
                return null;
            }
        }
    }
}
