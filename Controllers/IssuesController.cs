using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChecklistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public IssuesController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }
        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> Get()
        {
            return await _context.Issues.ToListAsync();
        }


        // GET: api/Issues/id/5
        [HttpGet("id/{key}")]
        public async Task<ActionResult<Issue[]>> Get(int key)
        {
            var issue = await _context.Issues.FindAsync(key);

            if (issue == null)
            {
                return NotFound();
            }

            return new Issue[] { issue };
        }


        // GET: api/Issues/description/5
        [HttpGet("description/{key}")]
        public async Task<ActionResult<Issue[]>> Get(string key)
        {
            var issue = await _context.Issues.Where<Issue>(issue => issue.Id.StartsWith(@$"{key}")).ToArrayAsync();
            

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // POST: api/Issues
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Issues/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
