using System.ComponentModel;

namespace OgrenciOtomasyonWeb.Models
{
        public enum Durum
        {
            [Description("Aktif")]
            Aktif = 1,
            [Description("Mezun")]
            Mezun = 2,
        }
    }
