using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;

namespace ChecklistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreakdownsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public BreakdownsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/Breakdowns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Breakdown>>> GetBreakdowns()
        {
            return await _context.Breakdowns.ToListAsync();
        }

        // GET: api/Breakdowns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Breakdown>> GetBreakdown(int id)
        {
            var breakdown = await _context.Breakdowns.FindAsync(id);

            if (breakdown == null)
            {
                return NotFound();
            }

            return breakdown;
        }

        // PUT: api/Breakdowns/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBreakdown(int id, Breakdown breakdown)
        {
            if (id != breakdown.Id)
            {
                return BadRequest();
            }

            _context.Entry(breakdown).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BreakdownExists(id))
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

        // POST: api/Breakdowns
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Breakdown>> PostBreakdown(Breakdown breakdown)
        {
            _context.Breakdowns.Add(breakdown);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBreakdown", new { id = breakdown.Id }, breakdown);
        }

        // DELETE: api/Breakdowns/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Breakdown>> DeleteBreakdown(int id)
        {
            var breakdown = await _context.Breakdowns.FindAsync(id);
            if (breakdown == null)
            {
                return NotFound();
            }

            _context.Breakdowns.Remove(breakdown);
            await _context.SaveChangesAsync();

            return breakdown;
        }

        private bool BreakdownExists(int id)
        {
            return _context.Breakdowns.Any(e => e.Id == id);
        }
    }
}
