using LoxezSpZOOContext.Data;
using LoxezSpZOOContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LoxezSpZOO.Controllers
{
    public class DokumentController : Controller
    {
        private readonly LoxezSpZOODataContext _context;

        public DokumentController(LoxezSpZOODataContext context)
        {
            _context = context;
        }

        // GET: Dokument
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dokument.ToListAsync());
        }

        // GET: Dokument/PdfAsync/5
        [Route("Dokument/PdfAsync/{id}")]
        [HttpGet("PdfAsync")]
        public async Task<FileResult> PdfAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Dokument dokument = await _context.Dokument.FirstOrDefaultAsync(m => m.Id == id);
            if (dokument == null)
            {
                return null;
            }
            return File(dokument.Tresc, "application/pdf", dokument.NazwaPliku + ".pdf");
        }

        // GET: Dokument/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dokument dokument = await _context.Dokument
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dokument == null)
            {
                return NotFound();
            }

            return View(dokument);
        }

        // POST: Dokument/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Dokument dokument = await _context.Dokument.FindAsync(id);
            _context.Dokument.Remove(dokument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DokumentExists(int id)
        {
            return _context.Dokument.Any(e => e.Id == id);
        }
    }
}
