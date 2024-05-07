using Group01_QuanLyLuanVan.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.DAO
{
    public class ThongBaoDAO
    {
        DBConnection conn = new DBConnection();
        public DataTable LoadListThongBao()
        {
            DataTable dt = new DataTable();
            string sqlStr = string.Format("SELECT thongBaoId, tieude, noiDung, ThongBao.deTaiId, ngay ,ThongBao.trangthai FROM ThongBao INNER JOIN DeTai ON ThongBao.deTaiId = DeTai.deTaiId INNER JOIN SinhVien ON DeTai.nhomId = SinhVien.nhomId  WHERE sinhVienId = '{0}'", Const.sinhVien.SinhVienId);
            //string sqlStr = string.Format("SELECT thongBaoId, tieude, noiDung, ThongBao.deTaiId, ngay ,ThongBao.trangthai FROM ThongBao INNER JOIN DeTai ON ThongBao.deTaiId = DeTai.deTaiId INNER JOIN SinhVien ON DeTai.nhomId = SinhVien.nhomId  WHERE sinhVienId = 'SV21110589'");

            dt = conn.Sql_Select(sqlStr);
            return dt;
        }
        public DataTable LoadListThongBao(string deTaiId)
        {
            DataTable dt = new DataTable();
            string sqlStr = string.Format("SELECT thongBaoId, tieude, noiDung, ThongBao.deTaiId, ngay  FROM ThongBao INNER JOIN DeTai ON ThongBao.deTaiId = DeTai.deTaiId WHERE DeTai.deTaiId = '{0}'", deTaiId);
            dt = conn.Sql_Select(sqlStr);
            return dt;
        }

        public void AddThongBao(ThongBao tb)
        {
            string sqlStr = string.Format("Insert into ThongBao(tieude, noidung, deTaiId, ngay) values(N'{0}', N'{1}', '{2}', '{3}')", tb.TieuDe, tb.NoiDung, tb.DeTaiId, tb.Ngay);
            conn.Sql_Them_Xoa_Sua(sqlStr);
        }


    }
}
