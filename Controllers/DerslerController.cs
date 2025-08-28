using Microsoft.AspNetCore.Mvc;
using OgrenciOtomasyonWeb.Context;
using OgrenciOtomasyonWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace OgrenciOtomasyonWeb.Controllers
{
    public class DerslerController : Controller
    {
        private readonly OgrenciOtomasyonuWebDbContext _context;

        public DerslerController(OgrenciOtomasyonuWebDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string DersAdi)
        {
            ViewBag.DersAdi = DersAdi;

            if (string.IsNullOrWhiteSpace(DersAdi) || DersAdi.Length < 5)
            {
                var tumDersler = _context.Dersler
                    .Select(o => new Dersler
                    {
                        DersId = o.DersId,
                        DersAdi = o.DersAdi,
                        DersKodu = o.DersKodu,
                        Kredi = o.Kredi,
                        Durum = o.Durum
                    })
                    .ToList();

                return View(tumDersler);
            }

            var derslerListesi = _context.Dersler
                .Where(o => o.DersAdi.Contains(DersAdi))
                .Select(o => new Dersler
                {
                    DersId = o.DersId,
                    DersAdi = o.DersAdi,
                    DersKodu = o.DersKodu,
                    Kredi = o.Kredi,
                    Durum = o.Durum
                })
                .ToList();

            if (!derslerListesi.Any())
            {
                ViewBag.Hata = "Aradığınız ada sahip ders bulunamadı.";

                var tumDersler = _context.Dersler.ToList();
                return View(tumDersler);
            }

            return View(derslerListesi);
        }
        

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Dersler dersler)
        {
            if (string.IsNullOrWhiteSpace(dersler.DersAdi) ||
         string.IsNullOrWhiteSpace(dersler.DersKodu) ||
         dersler.Kredi == 0)
            {
                ModelState.AddModelError("", "Lütfen tüm zorunlu alanları doldurunuz.");
                return View(dersler);
            }
            _context.Add(dersler);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var ders = _context.Dersler.Find(id);
            if (ders == null)
            {
                return NotFound();
            }
            return View(ders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Dersler dersler)
        {
            if (!ModelState.IsValid)
            {
                _context.Dersler.Update(dersler);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(dersler);
        }
        public IActionResult Delete(int id)
        {
            var ders = _context.Dersler.Find(id);
            if (ders == null)
            {
                return NotFound();
            }
            return View(ders);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ders = _context.Dersler.Find(id);
            if (ders == null)
            {
                return NotFound();
            }
            _context.Dersler.Remove(ders);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Filtrele()
        {
            ViewBag.DersAdi = "";
            return View(new List<Dersler>());
        }

        [HttpPost]
        public IActionResult Filtrele(string DersAdi)
        {
            if (string.IsNullOrWhiteSpace(DersAdi))
            {
                ViewBag.Hata = "Lütfen geçerli bir ders adı giriniz.";
                return View(new List<Dersler>());
            }

            var derslerListesi = _context.Dersler
                .Where(od => od.DersAdi.Contains(DersAdi))
                .ToList();

            if (!derslerListesi.Any())
            {
                ViewBag.Hata = "Kayıtlı bir ders bulunamadı.";
            }

            ViewBag.DersAdi = DersAdi;
            return View(derslerListesi);
        }
    }
}
