using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quiz_backend.Models;

namespace quiz_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase {
        private readonly QuizContext context;
        public QuestionController(QuizContext quizContext) {
            context = quizContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Question question) {

            var quiz = context.Quiz.SingleOrDefault(q => q.Id == question.QuizId);

            if (quiz == null) {
                return NotFound();
            }
            context.Questions.Add(question);
            await context.SaveChangesAsync();
            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Question question) {
            
            // Not needed because of using Entry
            // var question = await context.Questions.SingleOrDefaultAsync(q => q.Id == id);
            if (id != question.Id) {
                return BadRequest();
            }

                context.Entry(question).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(question);
        }

        [HttpGet]
        public IEnumerable<Question> Get() {
            return context.Questions;
        }


        [HttpGet("{quizId}")]
        public IEnumerable<Question> Get([FromRoute] int quizId) {
            return context.Questions.Where(q => q.QuizId == quizId);
        }

    }
}