using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis;
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
        public async Task<ActionResult<Issue>> Post([FromBody] object value)
        {
            var issue = JsonSerializer.Deserialize<Issue>(value.ToString());
            if (!_context.Issues.Contains(issue))
            {
                _context.Issues.Add(issue);
                await _context.SaveChangesAsync();
                return issue;
            } else
            {
                return BadRequest();
            }
        }

        // PUT: api/component/test
        [HttpPut("{name}")]
        public async Task<ActionResult<Issue>> Put(string name, [FromBody] object value)
        {
            var oldIssue = new Issue();
            var newIssue = new Issue();

            oldIssue = await _context.Issues.FindAsync(name);

            if (_context.Issues.Contains(oldIssue))
            {
                newIssue = JsonSerializer.Deserialize<Issue>(value.ToString());
                _context.Issues.Remove(oldIssue);
                _context.Issues.Add(newIssue);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete()]
        public async Task<ActionResult<Issue>> Delete([FromBody] object value)
        {
            var issue = JsonSerializer.Deserialize<Issue>(value.ToString());
            if (_context.Issues.Contains(issue))
            {
                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();
                return Ok();
            }else
            {
                return BadRequest();
            }
        }
    }
}
