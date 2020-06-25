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
    public class ChecklistsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public ChecklistsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/Checklists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetChecklists()
        {
            return await _context.Checklists.ToListAsync();
        }

        // GET: api/Checklists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Checklist>> GetChecklist(int id)
        {
            var checklist = await _context.Checklists.FindAsync(id);

            if (checklist == null)
            {
                return NotFound();
            }

            return checklist;
        }

        // PUT: api/Checklists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChecklist(int id, Checklist checklist)
        {
            //if (id != checklist.ID)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(checklist).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ChecklistExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();

            throw new NotImplementedException();
        }

        // POST: api/Checklists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<object>> PostChecklist(Checklist checklist)
        {
            try
            {
                _context.Checklists.Add(checklist);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetChecklist", new { id = checklist.ID }, checklist);
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }

        // DELETE: api/Checklists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Checklist>> DeleteChecklist(int id)
        {
            //var checklist = await _context.Checklists.FindAsync(id);
            //if (checklist == null)
            //{
            //    return NotFound();
            //}

            //_context.Checklists.Remove(checklist);
            //await _context.SaveChangesAsync();

            //return checklist;

            throw new NotImplementedException();
        }

        private bool ChecklistExists(int id)
        {
            return _context.Checklists.Any(e => e.ID == id);
        }
    }
}
