using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChecklistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        // GET: api/<VouchersController>
        private readonly EquipmentChecklistDBContext _context;

        public VouchersController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<string> GetVoucher()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<VouchersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(string userId)
        {
            //return await _context.Vouchers.FindAsync(userId);
            return NotFound();
        }

        // POST api/<VouchersController>
        [HttpPost]
        public async Task<ActionResult<object>> PostVoucher([FromBody] Voucher voucher)
        {
            try
            {
                var _voucher = voucher;
                _voucher.Validity = DateTime.Now.AddDays(1);
                _context.Vouchers.Add(_voucher);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVoucher", new { }, voucher);
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        // PUT api/<VouchersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VouchersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
