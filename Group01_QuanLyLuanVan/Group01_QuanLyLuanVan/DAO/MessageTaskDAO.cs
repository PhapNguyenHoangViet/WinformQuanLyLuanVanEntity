using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.DAO
{
    public class MessageTaskDAO
    {
        DBConnection conn = new DBConnection();

        public DataTable LoadListMessageTask(int yeuCauId)
        {
            string sqlStr = string.Format("SELECT * From TinNhanYeuCau WHERE yeuCauId = '{0}'", yeuCauId);
            DataTable tb = conn.Sql_Select(sqlStr);
            return tb;
        }
        public List<MessageTask> ListMessageTask(int yeuCauId)
        {
            List<MessageTask> dsTn = new List<MessageTask>();
            string sqlStr = string.Format("SELECT * From TinNhanYeuCau WHERE yeuCauId = '{0}'", yeuCauId);
            DataTable tb = conn.Sql_Select(sqlStr);
            if (tb.Rows.Count > 0)
            {
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    dsTn.Add(new MessageTask(int.Parse(tb.Rows[i]["tinNhanId"].ToString()), tb.Rows[i]["tinNhan"].ToString(), DateTime.Parse(tb.Rows[i]["thoiGian"].ToString()), tb.Rows[i]["username"].ToString(), int.Parse(tb.Rows[i]["yeuCauId"].ToString())));
                }
                return dsTn;
            }
            else
            {
                return null;
            }
        }
        public void AddMessage(string tinNhan, DateTime thoiGian, string username, int yeuCauId)
        {
            string sqlStr = string.Format("Insert into TinNhanYeuCau(tinNhan, thoiGian, username,yeuCauId) values(N'{0}', '{1}', '{2}', '{3}')", tinNhan, thoiGian, username, yeuCauId);
            conn.Sql_Them_Xoa_Sua(sqlStr);
        }
        public DataTable ListYeuCauByDeTaiId(string deTaiId)
        {
            DataTable dt = new DataTable();
            string sqlStr = string.Format("SELECT yeuCauId, noiDung, trangThai, deTaiId FROM YeuCau where deTaiId = '{0}'", deTaiId);
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }
    }
}
