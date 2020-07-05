using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
using Microsoft.AspNetCore.Authorization;

namespace ChecklistAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public ConditionsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/Conditions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Condition>>> GetConditions()
        {
            return await _context.Conditions.ToListAsync();
        }

        // GET: api/Conditions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Condition>> GetCondition(string id)
        {
            var condition = await _context.Conditions.FindAsync(id);

            if (condition == null)
            {
                return NotFound();
            }

            return condition;
        }

        // PUT: api/Conditions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCondition(string id, Condition condition)
        {
            if (id != condition.ID)
            {
                return BadRequest();
            }

            _context.Entry(condition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConditionExists(id))
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

        // POST: api/Conditions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPost]
        //    public async Task<ActionResult<Condition>> PostCondition(Condition condition)
        //    {
        //        _context.Conditions.Add(condition);
        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException)
        //        {
        //            if (ConditionExists(condition.ID))
        //            {
        //                return Conflict();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return CreatedAtAction("GetCondition", new { id = condition.ID }, condition);
        //    }

        //    // DELETE: api/Conditions/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult<Condition>> DeleteCondition(string id)
        //    {
        //        var condition = await _context.Conditions.FindAsync(id);
        //        if (condition == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Conditions.Remove(condition);
        //        await _context.SaveChangesAsync();

        //        return condition;
        //    }

        private bool ConditionExists(string id)
        {
            return _context.Conditions.Any(e => e.ID == id);
        }
    }
}
