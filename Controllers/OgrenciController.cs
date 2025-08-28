using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OgrenciOtomasyonWeb.Context;
using OgrenciOtomasyonWeb.Models;
using Rotativa.AspNetCore;
using System.ComponentModel;

namespace OgrenciOtomasyonWeb.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly OgrenciOtomasyonuWebDbContext _context;

        public OgrenciController(OgrenciOtomasyonuWebDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string ogrenciNo)
        {
            ViewBag.OgrenciNo = ogrenciNo;

            var tumOgrenciler = _context.Ogrenci
                .Select(o => new Ogrenci
                {
                    OgrenciId = o.OgrenciId,
                    OgrenciNo = o.OgrenciNo,
                    Ad = o.Ad,
                    Soyad = o.Soyad,
                    TcNo = o.TcNo,
                    Email = o.Email,
                    TelefonNo = o.TelefonNo,
                    Bolumu = o.Bolumu,
                    Sinif = o.Sinif,
                    DogumTarihi = o.DogumTarihi,
                    KayitTarihi = o.KayitTarihi,
                    Durum = o.Durum
                })
                .ToList();

            if (string.IsNullOrEmpty(ogrenciNo))
            {
                return View(tumOgrenciler);
            }

            if (!int.TryParse(ogrenciNo, out int ogrenciNoInt))
            {
                ViewBag.Hata = "Geçerli bir öğrenci numarası giriniz.";
                return View(tumOgrenciler); 
            }

            var ogrenciListesi = _context.Ogrenci
                .Where(o => o.OgrenciNo == ogrenciNoInt)
                .Select(o => new Ogrenci
                {
                    OgrenciId = o.OgrenciId,
                    OgrenciNo = o.OgrenciNo,
                    Ad = o.Ad,
                    Soyad = o.Soyad,
                    TcNo = o.TcNo,
                    Email = o.Email,
                    TelefonNo = o.TelefonNo,
                    Bolumu = o.Bolumu,
                    Sinif = o.Sinif,
                    DogumTarihi = o.DogumTarihi,
                    KayitTarihi = o.KayitTarihi,
                    Durum = o.Durum
                })
                .ToList();

            if (!ogrenciListesi.Any())
            {
                ViewBag.Hata = "Bu öğrenci numarasına ait kayıt bulunamadı.";
                return View(tumOgrenciler);
            }

            return View(ogrenciListesi);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ogrenci ogrenci)
        {
            if (string.IsNullOrWhiteSpace(ogrenci.Ad) ||
                string.IsNullOrWhiteSpace(ogrenci.Soyad) ||
                string.IsNullOrWhiteSpace(ogrenci.TcNo) || ogrenci.TcNo.Length != 11 ||
                string.IsNullOrWhiteSpace(ogrenci.Email) ||
                ogrenci.OgrenciNo == 0 ||
                ogrenci.Sinif == 0 ||
                ogrenci.DogumTarihi == default ||
                ogrenci.KayitTarihi == default)
            {
                ModelState.AddModelError("", "Lütfen tüm zorunlu alanları doğru ve eksiksiz doldurunuz.");
                return View(ogrenci);

            }
            _context.Add(ogrenci);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var ogrenci = _context.Ogrenci.Find(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            return View(ogrenci);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ogrenci ogrenci)
        {
            if (string.IsNullOrWhiteSpace(ogrenci.TcNo) || ogrenci.TcNo.Length != 11)
            {
                ModelState.AddModelError("TcNo", "TC Kimlik numarası 11 haneli olmak zorundadır.");
            }

            if (ModelState.IsValid)
            {
                _context.Ogrenci.Update(ogrenci);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(ogrenci);
        }

        public IActionResult Delete(int id)
        {
            var ogrenci = _context.Ogrenci.Find(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            return View(ogrenci);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ogrenci = _context.Ogrenci.Find(id);
            if (ogrenci == null)
            {
                return NotFound();
            }

            var ogrenciDersleri = _context.OgrenciDers
                .Where(od => od.OgrenciId == id)
                .ToList();

            _context.OgrenciDers.RemoveRange(ogrenciDersleri);
            _context.Ogrenci.Remove(ogrenci);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Transkript(int id)
        {
            var ogrenci = _context.Ogrenci.FirstOrDefault(o => o.OgrenciId == id);

            if (ogrenci == null)
                return NotFound();

            var dersNotlari = _context.OgrenciDers
                .Include(od => od.Dersler)
                .Where(od => od.OgrenciId == id)
                .ToList();

            double toplamNot = 0;
            int toplamKredi = 0;

            foreach (var item in dersNotlari)
            {
                if (item.Dersler != null)
                {
                    double katsayi = HarfNotuToKatsayi((HarfNotu)item.HarfNotu);
                    toplamNot += katsayi * item.Dersler.Kredi;
                    toplamKredi += item.Dersler.Kredi;
                }
            }

            double gano = toplamKredi > 0 ? toplamNot / toplamKredi : 0;

            var model = new OgrenciTranskriptViewModel
            {
                Ogrenci = ogrenci,
                DersNotlari = dersNotlari,
                Gano = Math.Round(gano, 2)
            };

            return View(model);
        }

        private double HarfNotuToKatsayi(HarfNotu harf)
        {
            return harf switch
            {
                HarfNotu.AA => 4.0,
                HarfNotu.BA => 3.5,
                HarfNotu.BB => 3.0,
                HarfNotu.CB => 2.5,
                HarfNotu.CC => 2.0,
                HarfNotu.DC => 1.5,
                HarfNotu.DD => 1.0,
                HarfNotu.FF => 0.0,
                _ => 0.0
            };
        }

        public IActionResult TranskriptPdf(int id)
        {
            var ogrenci = _context.Ogrenci.FirstOrDefault(o => o.OgrenciId == id);

            if (ogrenci == null)
                return NotFound();

            var dersNotlari = _context.OgrenciDers
                .Include(od => od.Dersler)
                .Where(od => od.OgrenciId == id)
                .ToList();

            double toplamNot = 0;
            int toplamKredi = 0;

            foreach (var item in dersNotlari)
            {
                if (item.Dersler != null)
                {
                    double katsayi = HarfNotuToKatsayi((HarfNotu)item.HarfNotu);
                    toplamNot += katsayi * item.Dersler.Kredi;
                    toplamKredi += item.Dersler.Kredi;
                }
            }

            double gano = toplamKredi > 0 ? toplamNot / toplamKredi : 0;

            var model = new OgrenciTranskriptViewModel
            {
                Ogrenci = ogrenci,
                DersNotlari = dersNotlari,
                Gano = Math.Round(gano, 2)
            };

            return new ViewAsPdf("Transkript", model)
            {
                FileName = $"Transkript_{ogrenci.OgrenciNo}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };
        }
        [HttpGet]
        public IActionResult Filtrele()
        {
            ViewBag.OgrenciNo = "";
            return View(new List<Ogrenci>());
        }

        [HttpPost]
        public IActionResult Filtrele(string ogrenciNo)
        {
            if (!int.TryParse(ogrenciNo, out int ogrenciNoInt))
            {
                ViewBag.Hata = "Geçerli bir öğrenci numarası girin.";
                return View(new List<Ogrenci>()); 
            }

            var ogrenciListesi = _context.Ogrenci
                .Where(o => o.OgrenciNo == ogrenciNoInt)
                .ToList();

            ViewBag.OgrenciNo = ogrenciNo;
            return View(ogrenciListesi);
        }
    }
}
