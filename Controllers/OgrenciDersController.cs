using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OgrenciOtomasyonWeb.Context;
using OgrenciOtomasyonWeb.Models;
using Microsoft.EntityFrameworkCore;    

namespace OgrenciOtomasyonWeb.Controllers
{
    public class OgrenciDersController : Controller
    {
        private readonly OgrenciOtomasyonuWebDbContext _context;

        public OgrenciDersController(OgrenciOtomasyonuWebDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string ogrenciNo)
        {

            ViewBag.OgrenciNo = ogrenciNo;

            if (string.IsNullOrWhiteSpace(ogrenciNo))
            {
                return View(new List<OgrenciDers>());
            }

            if (!int.TryParse(ogrenciNo, out int ogrenciNoInt))
            {
                ViewBag.Hata = "Geçerli bir öğrenci numarası giriniz.";
                return View(new List<OgrenciDers>());
            }

            var ogrenci = _context.Ogrenci.FirstOrDefault(o => o.OgrenciNo == ogrenciNoInt);
            if (ogrenci == null)
            {
                ViewBag.Hata = "Bu öğrenci numarasına ait kayıt bulunamadı.";
                return View(new List<OgrenciDers>());
            }

            var dersler = _context.OgrenciDers
                .Include(x => x.Ogrenci)
                .Include(x => x.Dersler)
                .Include(x => x.OgretimUyesi)
                .Where(x => x.OgrenciId == ogrenci.OgrenciId)
                .ToList();

            return View(dersler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Ogrenci = new SelectList(_context.Ogrenci.ToList(), "OgrenciId", "Ad");
            ViewBag.Dersler = new SelectList(_context.Dersler.ToList(), "DersId", "DersAdi");
            ViewBag.OgretimUyesi = new SelectList(_context.OgretimUyesi.ToList(), "OgretimUyesiId", "Ad");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OgrenciDers ogrenciDers)
        {
            if (ogrenciDers.OgrenciId == 0 ||
                ogrenciDers.DersId == 0 ||
                ogrenciDers.OgretimUyesiId == 0 ||
                ogrenciDers.Yil == 0 ||
                ogrenciDers.Donem == 0 ||
                ogrenciDers.Vize2 < 0 ||
                ogrenciDers.Final < 0)
            {
                ViewBag.Ogrenci = new SelectList(_context.Ogrenci.ToList(), "OgrenciId", "Ad", ogrenciDers.OgrenciId);
                ViewBag.Dersler = new SelectList(_context.Dersler.ToList(), "DersId", "DersAdi", ogrenciDers.DersId);
                ViewBag.OgretimUyesi = new SelectList(_context.OgretimUyesi.ToList(), "OgretimUyesiId", "Ad", ogrenciDers.OgretimUyesiId);
                
                ModelState.AddModelError("", "Lütfen tüm zorunlu alanları doldurunuz.");
                return View(ogrenciDers);
            }
            
            _context.Add(ogrenciDers);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var ogrenciDers = _context.OgrenciDers
            .Include(od => od.Ogrenci)
            .Include(od => od.Dersler)
            .Include(od => od.OgretimUyesi)
            .FirstOrDefault(od => od.OgrenciDersId == id);

            if (ogrenciDers == null)
            {
                return NotFound();
            }

            ViewBag.Ogrenci = new SelectList(_context.Ogrenci.ToList(), "OgrenciId", "Ad", ogrenciDers.OgrenciId);
            ViewBag.Dersler = new SelectList(_context.Dersler.ToList(), "DersId", "DersAdi", ogrenciDers.DersId);
            ViewBag.OgretimUyesi = new SelectList(_context.OgretimUyesi.ToList(), "OgretimUyesiId", "Ad", ogrenciDers.OgretimUyesiId);

            return View(ogrenciDers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OgrenciDers ogrenciDers)
        {
            if (!ModelState.IsValid)
            {
                string sql = @"
            UPDATE OgrenciDers
            SET Vize1 = @p0,
                Vize2 = @p1,
                Final = @p2,
                Quiz = @p3,
                Yil = @p4,
                Donem = @p5
            WHERE OgrenciDersId = @p6";

                _context.Database.ExecuteSqlRaw(
                    sql,
                    ogrenciDers.Vize1,
                    ogrenciDers.Vize2,
                    ogrenciDers.Final,
                    ogrenciDers.Quiz,
                    ogrenciDers.Yil,
                    ogrenciDers.Donem,
                    ogrenciDers.OgrenciDersId
                );

                return RedirectToAction(nameof(Index));
            }
            return View(ogrenciDers);
        }
        public IActionResult Delete(int id)
        {
            var ogrenciDers = _context.OgrenciDers.Find(id);
            if (ogrenciDers == null)
            {
                return NotFound();
            }
            return View(ogrenciDers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ogrenciDers = _context.OgrenciDers.Find(id);
            if (ogrenciDers == null)
            {
                return NotFound();
            }
            _context.OgrenciDers.Remove(ogrenciDers);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private int HesaplaHarfNotu(decimal vize1, decimal vize2, decimal final, decimal quiz)
        {
            decimal ortalama = vize1 * 0.2m + vize2 * 0.2m + final * 0.4m + quiz * 0.2m;

            if (ortalama >= 90) return 1;  
            else if (ortalama >= 85) return 2; 
            else if (ortalama >= 80) return 3; 
            else if (ortalama >= 75) return 4;
            else if (ortalama >= 70) return 5; 
            else if (ortalama >= 65) return 6; 
            else if (ortalama >= 60) return 7; 
            else return 8; 
        }

        [HttpGet]
        public IActionResult Filtrele()
        {
            ViewBag.OgrenciNo = "";
            return View(new List<OgrenciDers>());
        }

        [HttpPost]
        public IActionResult Filtrele(string ogrenciNo)
        {
            if (!int.TryParse(ogrenciNo, out int ogrenciNoInt))
            {
                ViewBag.Hata = "Geçerli bir öğrenci numarası girin.";
                return View(new List<OgrenciDers>());
            }

            var ogrenciDersListesi = _context.OgrenciDers
                .Include(od => od.Ogrenci)
                .Include(od => od.Dersler)
                .Include(od => od.OgretimUyesi)
                .Where(od => od.Ogrenci.OgrenciNo == ogrenciNoInt)
                .ToList();

            ViewBag.OgrenciNo = ogrenciNo;
            return View(ogrenciDersListesi);
        }
    }
}
