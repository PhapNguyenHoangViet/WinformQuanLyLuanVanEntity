using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.DAO
{
    public class TheLoaiDAO
    {
        DBConnection conn = new DBConnection();
        public List<TheLoai> FindByKhoaId(string khoaId)
        {
            List<TheLoai> dsTL = new List<TheLoai>();
            string sqlStr = string.Format("select theLoaiId, tenTheLoai, khoaId from TheLoai where khoaId = '{0}'", khoaId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    dsTL.Add(new TheLoai(tb.Rows[i][0].ToString(), tb.Rows[i][1].ToString(), tb.Rows[i][2].ToString()));
                }
                return dsTL;
            }
            else
            {
                return null;
            }
        }

        public TheLoai FindByTenTheLoai(string tenTheLoai)
        {
            string sqlStr = string.Format("select theLoaiId, tenTheLoai, khoaId from TheLoai where tenTheLoai = '{0}'", tenTheLoai);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                TheLoai tl = new TheLoai(dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                return tl;
            }
            else
            {
                return null;
            }
        }
        public DataTable LoadListTheLoai()
        {
            DataTable dt = new DataTable();
            String sqlStr = string.Format("select * from TheLoai WHERE khoaId = '{0}'", Const.sinhVien.KhoaId);
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }
    }
}
