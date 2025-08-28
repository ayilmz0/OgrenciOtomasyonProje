using Microsoft.AspNetCore.Mvc;
using OgrenciOtomasyonWeb.Context;
using OgrenciOtomasyonWeb.Models;

namespace OgrenciOtomasyonWeb.Controllers
{
    public class OgretimUyesiController : Controller
    {
        private readonly OgrenciOtomasyonuWebDbContext _context;

        public OgretimUyesiController(OgrenciOtomasyonuWebDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Ad)
        {
            ViewBag.Ad = Ad;

            if (string.IsNullOrWhiteSpace(Ad) || Ad.Length < 3)
            {
                var tumOgretimUyesi = _context.OgretimUyesi.ToList();
                return View(tumOgretimUyesi);
            }
            var filtreliOgretimUyesi = _context.OgretimUyesi
                .Where(o => o.Ad.Contains(Ad))
                .ToList();

            if (!filtreliOgretimUyesi.Any())
            {
                ViewBag.Hata = "Aradığınız ada sahip öğretim üyesi bulunamadı.";
                filtreliOgretimUyesi = _context.OgretimUyesi.ToList();
            }
             return View(filtreliOgretimUyesi);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OgretimUyesi ogretimUyesi)
        {
            if (string.IsNullOrWhiteSpace(ogretimUyesi.Ad) ||
        string.IsNullOrWhiteSpace(ogretimUyesi.Soyad) ||
        string.IsNullOrWhiteSpace(ogretimUyesi.Email) ||
        string.IsNullOrWhiteSpace(ogretimUyesi.Unvan))
            {
                ModelState.AddModelError("", "Lütfen tüm zorunlu alanları doldurunuz.");
                return View(ogretimUyesi);
            }
            _context.Add(ogretimUyesi);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var ogretimUyesi = _context.OgretimUyesi.Find(id);
            if (ogretimUyesi == null)
            {
                return NotFound();
            }
            return View(ogretimUyesi);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OgretimUyesi ogretimUyesi)
        {
            if (!ModelState.IsValid)
            {
                _context.OgretimUyesi.Update(ogretimUyesi);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(ogretimUyesi);
        }
        public IActionResult Delete(int id)
        {
            var ogretimUyesi = _context.OgretimUyesi.Find(id);
            if (ogretimUyesi == null)
            {
                return NotFound();
            }
            return View(ogretimUyesi);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ogretimUyesi = _context.OgretimUyesi.Find(id);
            if (ogretimUyesi == null)
            {
                return NotFound();
            }
            _context.OgretimUyesi.Remove(ogretimUyesi);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Filtrele()
        {
            ViewBag.Ad = "";
            return View(new List<OgretimUyesi>());
        }

        [HttpPost]
        public IActionResult Filtrele(string Ad)
        {
            if (string.IsNullOrWhiteSpace(Ad))
            {
                ViewBag.Hata = "Lütfen geçerli bir öğretim üyesi adı giriniz.";
                return View(_context.OgretimUyesi.ToList());
            }

            var filtreliOgretimUyesi = _context.OgretimUyesi
                .Where(o => o.Ad.Contains(Ad))
                .ToList();

            if (!filtreliOgretimUyesi.Any())
            {
                ViewBag.Hata = "Aradığınız ada sahip öğretim üyesi bulunamadı.";
                filtreliOgretimUyesi = _context.OgretimUyesi.ToList();
            }

            ViewBag.Ad = Ad;
            return View(filtreliOgretimUyesi);
        }
    }
}
