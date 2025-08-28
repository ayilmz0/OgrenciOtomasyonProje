namespace OgrenciOtomasyonWeb.Models

{
    public class Ogrenci
    {
        public int OgrenciId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TcNo { get; set; }
        public string TcNoSansur
        {
            get
            {
                if (string.IsNullOrEmpty(TcNo) || TcNo.Length != 11)
                    return "***********";

                return TcNo.Substring(0,3) + "*****" + TcNo.Substring(8);  
            }
        }
        public string Email { get; set; }
        public string TelefonNo { get; set; }
        public int OgrenciNo { get; set; }
        public string Bolumu { get; set; }
        public int Sinif { get; set; }
        public DateTime DogumTarihi  { get; set; }
        public DateTime KayitTarihi  { get; set; }
        public Durum Durum {  get; set; }

        public virtual ICollection<OgrenciDers> OgrenciDers { get; set; }
    }
}
