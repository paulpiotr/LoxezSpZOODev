using LoxezSpZOOContext.Data;
using LoxezSpZOOContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoxezSpZOO.ViewComponents
{
    public class KoszykElementDodajViewComponent : ViewComponent
    {
        private readonly LoxezSpZOODataContext _context;
        public KoszykElementDodajViewComponent(LoxezSpZOODataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string nazwa = null, decimal ilosc = 0, decimal cena = 0, int? twTowarTwId = 0)
        {
            try
            {
                HttpContext.Session.SetString("Id", HttpContext.Session.Id);
                KoszykNaglowek koszykNaglowek = _context.KoszykNaglowek.Where(w => w.Sesja == HttpContext.Session.Id).FirstOrDefault();
                if (null == koszykNaglowek)
                {
                    koszykNaglowek = new KoszykNaglowek() { Sesja = HttpContext.Session.Id };
                    _context.Add(koszykNaglowek);
                    await _context.SaveChangesAsync();
                }
                koszykNaglowek = _context.KoszykNaglowek.Where(w => w.Sesja == HttpContext.Session.Id).FirstOrDefault();
                KoszykElement koszykElement = _context.KoszykElement.Where(w => w.KoszykNaglowekId == koszykNaglowek.Id && w.TwTowarTwId == twTowarTwId).FirstOrDefault();
                //return Content(koszykNaglowek.Id.ToString());
                if (null == koszykElement)
                {
                    //return Content(nazwa + " " + ilosc.ToString() + " " + cena.ToString() + " " + twTowarTwId.ToString());
                    koszykElement = new KoszykElement() { KoszykNaglowekId = koszykNaglowek.Id, Nazwa = nazwa, Ilosc = ilosc, Cena = cena, TwTowarTwId = twTowarTwId };
                }
                return View(koszykElement);
            }
            catch (Exception)
            {
                return View(null);
            }
        }
    }
}
