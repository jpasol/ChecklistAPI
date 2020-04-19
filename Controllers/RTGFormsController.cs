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
    public class RTGFormsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public RTGFormsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/RTGForms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RTGForm>>> GetRTGForms()
        {
            return await _context.RTGForms.ToListAsync();
        }

        // GET: api/RTGForms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RTGForm>> GetRTGForm(int id)
        {
            var rTGForm = await _context.RTGForms.FindAsync(id);

            if (rTGForm == null)
            {
                return NotFound();
            }

            return rTGForm;
        }

        // PUT: api/RTGForms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRTGForm(int id, RTGForm rTGForm)
        {
            if (id != rTGForm.Id)
            {
                return BadRequest();
            }

            _context.Entry(rTGForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RTGFormExists(id))
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

        // POST: api/RTGForms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RTGForm>> PostRTGForm(RTGForm rTGForm)
        {
            _context.RTGForms.Add(rTGForm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRTGForm", new { id = rTGForm.Id }, rTGForm);
        }

        // DELETE: api/RTGForms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RTGForm>> DeleteRTGForm(int id)
        {
            var rTGForm = await _context.RTGForms.FindAsync(id);
            if (rTGForm == null)
            {
                return NotFound();
            }

            _context.RTGForms.Remove(rTGForm);
            await _context.SaveChangesAsync();

            return rTGForm;
        }

        private bool RTGFormExists(int id)
        {
            return _context.RTGForms.Any(e => e.Id == id);
        }
    }
}
