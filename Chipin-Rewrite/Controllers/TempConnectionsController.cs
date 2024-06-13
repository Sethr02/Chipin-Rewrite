using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Models.woocommerce;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempConnectionsController : ControllerBase
    {
        private readonly ChipinDbContext _context;

        public TempConnectionsController(ChipinDbContext context)
        {
            _context = context;
        }

        // GET: api/TempConnections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TempConnection>>> GetTempConnection()
        {
            if (_context.TempConnection == null)
            {
                return NotFound();
            }
            return await _context.TempConnection.ToListAsync();
        }

        // GET: api/TempConnections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TempConnection>> GetTempConnection(int? id)
        {
            if (_context.TempConnection == null)
            {
                return NotFound();
            }
            var tempConnection = await _context.TempConnection.FindAsync(id);

            if (tempConnection == null)
            {
                return NotFound();
            }

            return tempConnection;
        }

        // PUT: api/TempConnections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTempConnection(int? id, TempConnection tempConnection)
        {
            if (id != tempConnection.TempConnectionId)
            {
                return BadRequest();
            }

            _context.Entry(tempConnection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TempConnectionExists(id))
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

        // POST: api/TempConnections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TempConnection>> PostTempConnection(TempConnection tempConnection)
        {
            if (_context.TempConnection == null)
            {
                return Problem("Entity set 'ChipinDbContext.TempConnection'  is null.");
            }
            if (HttpContext.Request.Cookies.ContainsKey("temp_session_id"))
            {
                string sessionId = HttpContext.Request.Cookies["temp_session_id"];

                Console.WriteLine(sessionId);
            }
            tempConnection.CreatedAt = DateTime.Now;
            _context.TempConnection.Add(tempConnection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTempConnection", new { id = tempConnection.TempConnectionId }, tempConnection);
        }

        // DELETE: api/TempConnections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTempConnection(int? id)
        {
            if (_context.TempConnection == null)
            {
                return NotFound();
            }
            var tempConnection = await _context.TempConnection.FindAsync(id);
            if (tempConnection == null)
            {
                return NotFound();
            }

            _context.TempConnection.Remove(tempConnection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TempConnectionExists(int? id)
        {
            return (_context.TempConnection?.Any(e => e.TempConnectionId == id)).GetValueOrDefault();
        }
    }
}
