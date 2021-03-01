using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers()
        {
            return await _context.Vouchers.ToListAsync();
        }

        // GET api/<VouchersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVoucher(string id)
        {
            return await getVouchersById(id);
        }

        // POST api/<VouchersController>
        [HttpPost]
        public async Task<ActionResult<object>> PostVoucher([FromBody] Voucher voucher)
        {
            var _userId = voucher.UserID;
            var _vouchersById = getVouchersById(_userId).Result.Value;
            if (_vouchersById.Any()) _context.RemoveRange(_vouchersById);
            try
            {
                var _voucher = voucher;
                _voucher.Validity = DateTime.Now.AddHours(4);
                _context.Vouchers.Add(_voucher);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetVoucher", new { id = _userId}, voucher);
            }
            catch (Exception e)
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
        public async Task<ActionResult<object>> Delete(string id)
        {
            var _vouchers = getVouchersById(id).Result.Value;
            try
            {
                _context.Vouchers.RemoveRange(_vouchers.ToArray());
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Vouchers have been deleted",
                    vouchers = _vouchers
                });
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private async Task<ActionResult<IEnumerable<Voucher>>> getVouchersById(string id)
        {
            return await _context.Vouchers.Where(x => x.UserID == id).ToListAsync();
        }

    }
}
