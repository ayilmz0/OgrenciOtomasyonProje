using System.Security.Policy;

namespace OgrenciOtomasyonWeb.Models
{
    public class OgretimUyesi
    {
        public int OgretimUyesiId { get; set; }
        public string Ad {  get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Unvan {  get; set; }
        public bool Durum { get; set; }

        public virtual ICollection<OgrenciDers> OgrenciDers { get; set; }
    }
}
