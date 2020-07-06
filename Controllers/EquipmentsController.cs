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
    public class EquipmentsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public EquipmentsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/Equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipments()
        {
            return await _context.Equipments
                .Include(x => x.Equipment_Type).ThenInclude(x => x.Questions)
                .ToListAsync();
        }

        // GET: api/Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(string id)
        {
            var equipment = await _context.Equipments.FindAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(string id, Equipment equipment)
        {
            if (id != equipment.ID)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EquipmentExists(id))
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

        // POST: api/Equipments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        //{
        //    _context.Equipments.Add(equipment);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (await EquipmentExists(equipment.ID))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetEquipment", new { id = equipment.ID }, equipment);
        //}

        [HttpPost("Auth")]
        public async Task<ActionResult<Equipment>> LoginEquipment(Equipment equipment)
        {
            if (await EquipmentExists(equipment.ID))
            {
                return await _context.Equipments
                    .Include(x => x.Equipment_Type)
                    .SingleOrDefaultAsync( x => x.ID == equipment.ID);
            }
            return BadRequest(new { message = "Invalid Equipment ID"});
        }

        //// DELETE: api/Equipments/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Equipment>> DeleteEquipment(string id)
        //{
        //    var equipment = await _context.Equipments.FindAsync(id);
        //    if (equipment == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Equipments.Remove(equipment);
        //    await _context.SaveChangesAsync();

        //    return equipment;
        //}

        private async Task<bool> EquipmentExists(string id)
        {
            return await _context.Equipments.AnyAsync(e => e.ID == id);
        }
    }
}
