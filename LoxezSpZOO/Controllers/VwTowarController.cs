using GtContext.Data;
using GtContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LoxezSpZOO.Controllers
{
    public class VwTowarController : Controller
    {
        private readonly GtDataContext _context;

        public VwTowarController(GtDataContext context)
        {
            _context = context;
        }

        // GET: VwTowar
        public async Task<IActionResult> Index()
        {
            return View(await _context.VwTowar.ToListAsync());
        }

        // GET: VwTowar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VwTowar vwTowar = await _context.VwTowar.FirstOrDefaultAsync(m => m.TwId == id);
            if (vwTowar == null)
            {
                return NotFound();
            }
            return View(vwTowar);
        }

        private bool VwTowarExists(int id)
        {
            return _context.VwTowar.Any(e => e.TwId == id);
        }
    }
}
