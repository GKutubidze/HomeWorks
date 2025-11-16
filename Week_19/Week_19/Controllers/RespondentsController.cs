using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week_19.Data;
using Week_19.Domain;

namespace Week_19.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespondentsController : ControllerBase
    {
        private readonly PersonContext _context;

        // ✅ კონტექსტის ინექცია
        public RespondentsController(PersonContext context)
        {
            _context = context;
        }

        // --- POST: api/respondents ---
        [HttpPost]
        public async Task<IActionResult> CreateRespondent([FromBody] Person person)
        {
            // საჭიროების მიხედვით დაამატე FluentValidation თუ გინდა
            person.CreateDate = DateTime.UtcNow;

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Respondent created successfully.", data = person });
        }

        // --- GET: api/respondents ---
        [HttpGet]
        public async Task<IActionResult> GetAllRespondents()
        {
            var respondents = await _context.Persons
                                    .Include(p => p.Adress) // მიბმული მისამართი
                                    .ToListAsync();
            return Ok(new { success = true, data = respondents });
        }

        // --- GET: api/respondents/{id} ---
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _context.Persons
                                .Include(p => p.Adress)
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound(new { success = false, message = $"Respondent with ID {id} not found." });

            return Ok(new { success = true, data = person });
        }

        // --- GET: api/respondents/filter?minSalary=2000&city=Tbilisi ---
        [HttpGet("filter")]
        public async Task<IActionResult> FilterRespondents([FromQuery] double? minSalary, [FromQuery] double? maxSalary, [FromQuery] string? city)
        {
            var query = _context.Persons.Include(p => p.Adress).AsQueryable();

            if (minSalary.HasValue)
                query = query.Where(p => p.Salary >= minSalary.Value);

            if (maxSalary.HasValue)
                query = query.Where(p => p.Salary <= maxSalary.Value);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(p => p.Adress != null && 
                                         p.Adress.City.Equals(city, StringComparison.OrdinalIgnoreCase));

            var respondents = await query.ToListAsync();

            return Ok(new { success = true, data = respondents });
        }

        // --- DELETE: api/respondents/{id} ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRespondent(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
                return NotFound(new { success = false, message = $"Respondent with ID {id} not found." });

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Respondent deleted successfully." });
        }

        // --- PUT: api/respondents/{id} ---
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRespondent(int id, [FromBody] Person updatedPerson)
        {
            var existingPerson = await _context.Persons
                                        .Include(p => p.Adress)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (existingPerson == null)
                return NotFound(new { success = false, message = $"Respondent with ID {id} not found." });

            // Update fields
            existingPerson.Firstname = updatedPerson.Firstname;
            existingPerson.Lastname = updatedPerson.Lastname;
            existingPerson.JobPosition = updatedPerson.JobPosition;
            existingPerson.Salary = updatedPerson.Salary;
            existingPerson.WorkExperience = updatedPerson.WorkExperience;

            if (updatedPerson.Adress != null)
            {
                if (existingPerson.Adress == null)
                    existingPerson.Adress = new Adress();

                existingPerson.Adress.City = updatedPerson.Adress.City;
                existingPerson.Adress.Country = updatedPerson.Adress.Country;
                existingPerson.Adress.HomeNumber = updatedPerson.Adress.HomeNumber;
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Respondent updated successfully.", data = existingPerson });
        }
    }
}
