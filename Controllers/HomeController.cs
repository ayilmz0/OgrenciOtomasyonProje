using Microsoft.AspNetCore.Mvc;
using OgrenciOtomasyonWeb.Context;
using OgrenciOtomasyonWeb.Models;
using System.Diagnostics;

namespace OgrenciOtomasyonWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly OgrenciOtomasyonuWebDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(OgrenciOtomasyonuWebDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            ViewBag.OgrenciSayisi = _context.Ogrenci.Count();
            ViewBag.DersSayisi = _context.Dersler.Count();
            ViewBag.OgretimUyesiSayisi = _context.OgretimUyesi.Count();
            ViewBag.AktifOgrenciSayisi = _context.Ogrenci.Count(o => o.Durum == Durum.Aktif);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
