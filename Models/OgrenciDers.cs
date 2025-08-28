using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciOtomasyonWeb.Models
{
    public class OgrenciDers
    {
        public int OgrenciDersId { get; set; }

        public int OgrenciId { get; set; }
        public Ogrenci Ogrenci { get; set; }

        public int DersId { get; set; }
        [ForeignKey("DersId")]
        public Dersler Dersler { get; set; }

        public int OgretimUyesiId { get; set; }
        public OgretimUyesi OgretimUyesi { get; set; }

        public int Yil {  get; set; }
        public int Donem { get; set; }
        public decimal Vize1 { get; set; }
        public decimal Vize2 { get; set; }
        public decimal Final { get; set; }
        public decimal Quiz{ get; set; }
        public int HarfNotu { get; set; }
    }
}
