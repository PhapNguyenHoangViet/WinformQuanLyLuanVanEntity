//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Group01_QuanLyLuanVan.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeTai
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeTai()
        {
            this.DanhGias = new HashSet<DanhGia>();
            this.ThongBaos = new HashSet<ThongBao>();
            this.TienDoes = new HashSet<TienDo>();
            this.YeuCaus = new HashSet<YeuCau>();
        }
    
        public int id { get; set; }
        public string deTaiId { get; set; }
        public string tenDeTai { get; set; }
        public string moTa { get; set; }
        public string yeuCauChung { get; set; }
        public Nullable<int> soLuong { get; set; }
        public Nullable<int> trangThai { get; set; }
        public Nullable<System.DateTime> ngayBatDau { get; set; }
        public Nullable<System.DateTime> ngayKetThuc { get; set; }
        public string theLoaiId { get; set; }
        public Nullable<int> nhomId { get; set; }
        public Nullable<int> an { get; set; }
        public Nullable<double> diem { get; set; }
        public string giangVienId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        public virtual GiangVien GiangVien { get; set; }
        public virtual Nhom Nhom { get; set; }
        public virtual TheLoai TheLoai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongBao> ThongBaos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TienDo> TienDoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YeuCau> YeuCaus { get; set; }
    }
}