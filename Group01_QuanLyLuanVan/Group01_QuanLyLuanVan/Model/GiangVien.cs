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
    
    public partial class GiangVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiangVien()
        {
            this.DeTais = new HashSet<DeTai>();
        }
    
        public int id { get; set; }
        public string giangVienId { get; set; }
        public string hoTen { get; set; }
        public Nullable<System.DateTime> ngaySinh { get; set; }
        public string gioiTinh { get; set; }
        public string diaChi { get; set; }
        public string email { get; set; }
        public string SDT { get; set; }
        public string khoaId { get; set; }
        public string username { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeTai> DeTais { get; set; }
        public virtual Khoa Khoa { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
