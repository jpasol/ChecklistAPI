using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChecklistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private string[] Issues = new string[]
        {
            "Incomplete",
            "Missing",
            "Broken",
            "Deformed",
            "Busted",
            "UnderPressure",
            "Malfunctioning",
            "Loosen",
            "Leaking"
        };

        // GET: api/Issues
        [HttpGet]
        public IEnumerable<Issues> Get()
        {
            return Enumerable.Range(1, Issues.Length).Select(index => new Issues
            {
                Key = index,
                Description = Issues[index - 1]
            })
                .ToArray();
        }
                

        // GET: api/Issues/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
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
