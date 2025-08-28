namespace OgrenciOtomasyonWeb.Models
{
    public class Dersler
    {
        public int DersId { get; set; }
        public string DersAdi { get; set; }
        public string DersKodu { get; set; }
        public int Kredi {  get; set; }
        public bool Durum { get; set; }

        public virtual ICollection<OgrenciDers> OgrenciDers { get; set; }
    }
}
