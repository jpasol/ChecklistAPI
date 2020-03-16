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
        private readonly AppDBContext _context;

        public IssuesController(AppDBContext context)
        {
            _context = context;
        }
        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issues>>> Get()
        {
            return await _context.Issues.ToListAsync();
        }


        // GET: api/Issues/id/5
        [HttpGet("id/{key}")]
        public async Task<ActionResult<Issues[]>> Get(int key)
        {
            var issue = await _context.Issues.FindAsync(key);

            if (issue == null)
            {
                return NotFound();
            }

            return new Issues[] { issue };
        }


        // GET: api/Issues/description/5
        [HttpGet("description/{key}")]
        public async Task<ActionResult<Issues[]>> Get(string key)
        {
            var issue = await _context.Issues.Where<Issues>(issue => issue.Description.StartsWith(@$"{key}")).ToArrayAsync();
            

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
