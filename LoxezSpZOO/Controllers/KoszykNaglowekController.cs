using LoxezSpZOOContext.Data;
using LoxezSpZOOContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LoxezSpZOO.Controllers
{
    public class KoszykNaglowekController : Controller
    {
        private readonly LoxezSpZOODataContext _context;

        public KoszykNaglowekController(LoxezSpZOODataContext context)
        {
            _context = context;
        }

        // GET: KoszykNaglowek/DodajDoKoszyka
        public async Task<IActionResult> DodajDoKoszyka(int? idProduktu, double? iloscProduktowDoDodania)
        {
            return View(await _context.KoszykNaglowek.ToListAsync());
        }

        // GET: KoszykNaglowek
        public async Task<IActionResult> Index()
        {
            return View(await _context.KoszykNaglowek.ToListAsync());
        }

        // GET: KoszykNaglowek/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            KoszykNaglowek koszykNaglowek = await _context.KoszykNaglowek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (koszykNaglowek == null)
            {
                return NotFound();
            }

            return View(koszykNaglowek);
        }

        // GET: KoszykNaglowek/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KoszykNaglowek/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sesja,DataUtworzenia")] KoszykNaglowek koszykNaglowek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(koszykNaglowek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(koszykNaglowek);
        }

        // GET: KoszykNaglowek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            KoszykNaglowek koszykNaglowek = await _context.KoszykNaglowek.FindAsync(id);
            if (koszykNaglowek == null)
            {
                return NotFound();
            }
            return View(koszykNaglowek);
        }

        // POST: KoszykNaglowek/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sesja,DataUtworzenia")] KoszykNaglowek koszykNaglowek)
        {
            if (id != koszykNaglowek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(koszykNaglowek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KoszykNaglowekExists(koszykNaglowek.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(koszykNaglowek);
        }

        // GET: KoszykNaglowek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            KoszykNaglowek koszykNaglowek = await _context.KoszykNaglowek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (koszykNaglowek == null)
            {
                return NotFound();
            }

            return View(koszykNaglowek);
        }

        // POST: KoszykNaglowek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            KoszykNaglowek koszykNaglowek = await _context.KoszykNaglowek.FindAsync(id);
            _context.KoszykNaglowek.Remove(koszykNaglowek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KoszykNaglowekExists(int id)
        {
            return _context.KoszykNaglowek.Any(e => e.Id == id);
        }
    }
}
