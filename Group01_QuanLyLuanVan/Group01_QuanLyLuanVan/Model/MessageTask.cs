using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Group01_QuanLyLuanVan.Model
{
    public class MessageTask
    {
        private int tinNhanId;
        private string tinNhan;
        private DateTime thoiGian;
        private string username;
        private int yeuCauId;
        private string uid;
        private string avatar;


        public MessageTask(int tinNhanId, string tinNhan, DateTime thoiGian, string username, int yeuCauId, string avatar)
        {
            this.tinNhanId = tinNhanId;
            this.tinNhan = tinNhan;
            this.thoiGian = thoiGian;
            this.username = username;
            this.yeuCauId = yeuCauId;
            this.avatar = avatar;

        }

        public MessageTask(int tinNhanId, string tinNhan, DateTime thoiGian, string username, int yeuCauId)
        {
            this.tinNhanId = tinNhanId;
            this.tinNhan = tinNhan;
            this.thoiGian = thoiGian;
            this.username = username;
            this.yeuCauId = yeuCauId;

        }

        public MessageTask(string username, string uid)
        {
            this.username = username;
            this.uid = uid;
        }

        public int TinNhanId { get => tinNhanId; set => tinNhanId = value; }
        public string TinNhan { get => tinNhan; set => tinNhan = value; }
        public int YeuCauId { get => yeuCauId; set => yeuCauId = value; }
        public string Username { get => username; set => username = value; }
        public DateTime ThoiGian { get => thoiGian; set => thoiGian = value; }
        public string UID { get => uid; set => uid = value; }
        public string Avatar { get => avatar; set => avatar = value; }

    }
}