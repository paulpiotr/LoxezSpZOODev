using LoxezSpZOOContext.Data;
using LoxezSpZOOContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LoxezSpZOO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DokumentApiController : ControllerBase
    {
        private readonly LoxezSpZOODataContext _context;

        public DokumentApiController(LoxezSpZOODataContext context)
        {
            _context = context;
        }

        // GET: api/DokumentApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dokument>>> GetDokument()
        {
            return await _context.Dokument.ToListAsync();
        }

        // GET: api/DokumentApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dokument>> GetDokument(int id)
        {
            Dokument dokument = await _context.Dokument.FindAsync(id);
            if (dokument == null)
            {
                return NotFound();
            }
            return dokument;
        }

        // GET: api/DokumentApi/?identyfikator=5
        [HttpGet("znajdz")]
        public ActionResult<Dokument> GetQuery(int identyfikator)
        {
            Dokument dokument = _context.Dokument.Where(w => w.Identyfikator == identyfikator).FirstOrDefault();
            if (dokument == null)
            {
                return NotFound();
            }
            return dokument;
        }

        // PUT: api/DokumentApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDokument(int id, Dokument dokument)
        {
            if (id != dokument.Id)
            {
                return BadRequest();
            }
            _context.Entry(dokument).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DokumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/DokumentApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Dokument>> PostDokument(Dokument dokument)
        {
            bool tryGetValue = HttpContext.Request.Headers.TryGetValue("X-Md5", out Microsoft.Extensions.Primitives.StringValues HeadersXMd5);
            byte[] getBytes = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(dokument));
            string xMd5 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.Text.Encoding.UTF8.GetString(MD5.Create().ComputeHash(getBytes))));
            if (xMd5 == HeadersXMd5)
            {
                _context.Dokument.Add(dokument);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDokument", new { id = dokument.Id }, dokument);
            }
            return NotFound();
        }

        // DELETE: api/DokumentApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dokument>> DeleteDokument(int id)
        {
            Dokument dokument = await _context.Dokument.FindAsync(id);
            if (dokument == null)
            {
                return NotFound();
            }
            _context.Dokument.Remove(dokument);
            await _context.SaveChangesAsync();
            return dokument;
        }

        private bool DokumentExists(int id)
        {
            return _context.Dokument.Any(e => e.Id == id);
        }
    }
}
