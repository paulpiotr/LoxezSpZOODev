using LoxezSpZOOContext.Data;
using LoxezSpZOOContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoxezSpZOO.Controllers
{
    public class KoszykElementController : Controller
    {
        private readonly LoxezSpZOODataContext _context;

        public KoszykElementController(LoxezSpZOODataContext context)
        {
            _context = context;
        }

        // GET: KoszykElement
        public IActionResult Index()
        {
            return RedirectToAction("Index", "VwTowar");
        }

        // GET: KoszykElement/PobierzKoszyk
        public IActionResult PobierzKoszyk()
        {
            HttpContext.Session.SetString("Id", HttpContext.Session.Id);
            KoszykNaglowek koszykNaglowek = _context.KoszykNaglowek.Where(w => w.Sesja == HttpContext.Session.Id).FirstOrDefault();
            if (null != koszykNaglowek)
            {
                List<KoszykElement> koszykElement = _context.KoszykElement.Where(w => w.KoszykNaglowekId == koszykNaglowek.Id).ToList();
                if (null != koszykElement && koszykElement.Count > 0)
                {
                    return View(koszykElement);
                }
            }
            return RedirectToAction("Index", "VwTowar");
        }


        // GET: KoszykElement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            KoszykElement koszykElement = await _context.KoszykElement.FirstOrDefaultAsync(m => m.Id == id);
            if (koszykElement == null)
            {
                return NotFound();
            }
            return View(koszykElement);
        }

        // GET: KoszykElement/PrzeliczKoszykWartosc
        public async Task<IActionResult> PrzeliczKoszykWartosc()
        {
            KoszykNaglowek koszykNaglowek = _context.KoszykNaglowek.Where(w => w.Sesja == HttpContext.Session.Id).FirstOrDefault();
            if (null != koszykNaglowek)
            {
                List<KoszykElement> koszykElementList = _context.KoszykElement.Where(w => w.KoszykNaglowekId == koszykNaglowek.Id).ToList();
                if (null != koszykElementList && koszykElementList.Count > 0)
                {
                    foreach (KoszykElement koszykElement in koszykElementList)
                    {
                        if (koszykElement.Ilosc > 10)
                        {
                            koszykElement.Cena = 10;
                        }
                        else
                        {
                            koszykElement.Cena = 5;
                        }
                        _context.Update(koszykElement);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("PobierzKoszyk", "KoszykElement");
        }

        // GET: KoszykElement/DodajDoKoszyka
        public IActionResult DodajDoKoszyka()
        {
            return View();
        }

        // POST: KoszykElement/DodajDoKoszyka
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajDoKoszyka([Bind("KoszykNaglowekId,Nazwa,Ilosc,Cena,TwTowarTwId")] KoszykElement koszykElement)
        {
            koszykElement.Ilosc = Convert.ToDecimal(string.Format("{0:F2}", koszykElement.Ilosc));
            if (koszykElement.Ilosc <= 0)
            {
                koszykElement.Ilosc = 1;
            }
            if (ModelState.IsValid)
            {
                //if (koszykElement.Ilosc > 10)
                //{
                //    koszykElement.Cena = 10;
                //}
                //else
                //{
                //    koszykElement.Cena = 5;
                //}
                KoszykElement koszykElementZnajdz = _context.KoszykElement.Where(w => w.KoszykNaglowekId == koszykElement.KoszykNaglowekId && w.TwTowarTwId == koszykElement.TwTowarTwId).FirstOrDefault();
                if (null != koszykElementZnajdz)
                {
                    koszykElementZnajdz.Ilosc = koszykElement.Ilosc <= 0 ? 1 : koszykElement.Ilosc;
                    _context.Update(koszykElementZnajdz);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    koszykElement.Ilosc = koszykElement.Ilosc <= 0 ? 1 : koszykElement.Ilosc;
                    _context.Add(koszykElement);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(PobierzKoszyk));
            }
            return View(koszykElement);
        }

        // GET: KoszykElement/Delete/5
        public async Task<IActionResult> UsunZKoszyka(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            KoszykElement koszykElement = await _context.KoszykElement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (koszykElement == null)
            {
                return NotFound();
            }

            return View(koszykElement);
        }

        // POST: KoszykElement/Delete/5
        [HttpPost, ActionName("UsunZKoszyka")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            KoszykElement koszykElement = await _context.KoszykElement.FindAsync(id);
            _context.KoszykElement.Remove(koszykElement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PobierzKoszyk));
        }

        private bool KoszykElementExists(int id)
        {
            return _context.KoszykElement.Any(e => e.Id == id);
        }
    }
}
