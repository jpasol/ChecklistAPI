using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;

namespace ChecklistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public ComponentsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/Components
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Component>>> GetComponents()
        {
            return await _context.Components.ToListAsync();
        }

        // GET: api/Components/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Component>> GetComponents(int id)
        {
            var components = await _context.Components.FindAsync(id);

            if (components == null)
            {
                return NotFound();
            }

            return components;
        }

        // PUT: api/Components/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComponents(string id, Component components)
        {
            if (id != components.Id)
            {
                return BadRequest();
            }

            _context.Entry(components).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentsExists(id))
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

        // POST: api/Components
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Component>> PostComponents(Component components)
        {
            _context.Components.Add(components);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComponents", new { id = components.Id }, components);
        }

        // DELETE: api/Components/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Component>> DeleteComponents(int id)
        {
            var components = await _context.Components.FindAsync(id);
            if (components == null)
            {
                return NotFound();
            }

            _context.Components.Remove(components);
            await _context.SaveChangesAsync();

            return components;
        }

        private bool ComponentsExists(string id)
        {
            return _context.Components.Any(e => e.Id == id);
        }
    }
}
