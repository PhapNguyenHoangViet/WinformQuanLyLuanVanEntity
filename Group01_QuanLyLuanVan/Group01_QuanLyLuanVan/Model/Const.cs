using Group01_QuanLyLuanVan.Chat.Net;
using Group01_QuanLyLuanVan.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01_QuanLyLuanVan.Model
{
    public class Const : BaseViewModel
    {
        public static SinhVien sinhVien { get; set; }
        public static GiangVien giangVien { get; set; }
        public static TaiKhoan taiKhoan { get; set; }
        public static DeTai DeTai { get; set; }
        public static YeuCau YeuCau { get; set; }

        public static Server _server { get; set; }

        public static string deTaiId { get; set; }
        public static int yeuCauId { get; set; }

        public static string _localLink = System.Reflection.Assembly.GetExecutingAssembly().Location.Remove(System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf(@"bin\Debug"));
    }
}
