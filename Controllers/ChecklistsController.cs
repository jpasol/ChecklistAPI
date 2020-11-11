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
using ChecklistAPI.Helpers;

namespace ChecklistAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        //to be removed at production . for now no GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetChecklists()
        {
            return await _context.Checklists
                .Include(x => x.Checklist_Items)
                .ToListAsync();
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

            var checklistItems = _context.Checklist_Items.Where(x => x.ChecklistID == id).ToHashSet();
            checklist.Checklist_Items = checklistItems;

            return checklist;
        }

        // GET: api/Checklists/issues
        [HttpGet("issues")]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetChecklistIssues()
        {
            var checklist = await _context.Checklists
                .Include(x => x.Checklist_Items)
                .Where(x => (x.Checklist_Items.Where(x => x.ConditionID != "OK").Count()) > 0)
                .ToListAsync();

            if (checklist == null)
            {
                return NotFound();
            }

            return checklist;
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<Checklist_Item>>> GetChecklistItems()
        {
            return await _context.Checklist_Items
                .Include(x => x.Checklist)
                .ThenInclude( y => y.User)
                .Include(x => x.Component)
                .ToListAsync();
        }

        // GET: api/Checklists/issues
        [HttpGet("items/issues")]
        public async Task<ActionResult<IEnumerable<Checklist_Item>>> GetChecklistItemsIssues()
        {
            var checklist = await _context.Checklist_Items
                .Include(x => x.Checklist)
                .ThenInclude(y => y.User)
                .Include(x => x.Component)
                .Where(x => x.ConditionID != "OK")
                .ToListAsync();

            if (checklist == null)
            {
                return NotFound();
            }

            return checklist;
        }

        // PUT: api/Checklists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutChecklist(int id, Checklist checklist)
        //{
        //    //if (id != checklist.ID)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    //_context.Entry(checklist).State = EntityState.Modified;

        //    //try
        //    //{
        //    //    await _context.SaveChangesAsync();
        //    //}
        //    //catch (DbUpdateConcurrencyException)
        //    //{
        //    //    if (!ChecklistExists(id))
        //    //    {
        //    //        return NotFound();
        //    //    }
        //    //    else
        //    //    {
        //    //        throw;
        //    //    }
        //    //}

        //    //return NoContent();

        //    throw new NotImplementedException();
        //}

        // POST: api/Checklists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<object>> PostChecklist(Checklist checklist)
        {
            try
            {
                EnsureHasChecklistItems(checklist);
                await EnsureSameEquipment(_context, checklist);
                _context.Checklists.Add(checklist);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetChecklist", new { id = checklist.ID }, checklist);
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        private bool EnsureHasChecklistItems(Checklist checklist)
        {
            var hasChecklistItems = checklist.Checklist_Items.Count();
            if (hasChecklistItems == 0) throw new Exception("Checklist has no items");
            return true;
        }

        private async Task<ActionResult<object>> EnsureSameEquipment(EquipmentChecklistDBContext context, Checklist checklist)
        {
            var items = checklist.Checklist_Items.ToArray();
            var equipment = await context.Equipments.FindAsync(checklist.EquipmentID);

            if (checklist.EquipmentID != equipment.ID) throw new Exception("Checklist's EquipmentID is invalid");

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Equipment_TypeID != equipment.Equipment_TypeID) throw new Exception("Some Checklist Item's Equipment Type ID is different from Checklist's Equipment Type ID");
            }

            return true;
        }

    }
}
