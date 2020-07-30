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
using System.Security.Cryptography.X509Certificates;

namespace ChecklistAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly EquipmentChecklistDBContext _context;

        public QuestionsController(EquipmentChecklistDBContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            return await _context.Questions
                .Include(x => x.Component)
                .ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question[]>> GetQuestionsbyEquipmentType(string id)
        {
            var question = await _context.Questions.Where(x => x.Equipment_TypeID == id).ToArrayAsync();

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(string id, Question question)
        {
            if (id != question.Equipment_TypeID)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw e;
                }
            }

            return NoContent();
        }

        // POST: api/Questions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            _context.Questions.Add(question);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (QuestionExists(question.Equipment_TypeID))
                {
                    return Conflict();
                }
                else
                {
                    throw e;
                }
            }

            return CreatedAtAction("GetQuestion", new { id = question.Equipment_TypeID }, question);
        }

        //// DELETE: api/Questions/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Question>> DeleteQuestion(string id)
        //{
        //    var question = await _context.Questions.FindAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Questions.Remove(question);
        //    await _context.SaveChangesAsync();

        //    return question;
        //}

        private bool QuestionExists(string id)
        {
            return _context.Questions.Any(e => e.Equipment_TypeID == id);
        }
    }
}
