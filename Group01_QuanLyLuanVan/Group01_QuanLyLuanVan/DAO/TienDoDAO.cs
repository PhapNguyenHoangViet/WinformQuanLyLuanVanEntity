using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.DAO
{
    public class TienDoDAO
    {
        DBConnection conn = new DBConnection();
        public TienDo FindOneByDeTaiId(string deTaiId)
        {
            string sqlStr = string.Format("(select tienDoId, noiDung, phanTram, ngay, deTaiId from TienDo where deTaiId='{0}') ", deTaiId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                DataRow dr = tb.Rows[0];
                TienDo tienDo = new TienDo(dr[0].ToString(), dr[1].ToString(), int.Parse(dr[2].ToString()), DateTime.Parse(dr[3].ToString()), dr[4].ToString());
                return tienDo;
            }
            else
            {
                return null;
            }
        }
    }
}
