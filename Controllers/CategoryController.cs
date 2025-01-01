using Hello.NET.Infrastructure.SQL.Database;
using Hello.NET.Infrastructure.SQL.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hello.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ApplicationDbContext context)
        : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<CategoryEntity>>
        > GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryEntity>> GetCategory(long id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);

            if (categoryEntity == null)
            {
                return NotFound();
            }

            return categoryEntity;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(
            long id,
            CategoryEntity categoryEntity
        )
        {
            if (id != categoryEntity.ID)
            {
                return BadRequest();
            }

            _context.Entry(categoryEntity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryEntity>> PostCategory(
            CategoryEntity categoryEntity
        )
        {
            _context.Categories.Add(categoryEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetCategoryEntity",
                new { id = categoryEntity.ID },
                categoryEntity
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);
            if (categoryEntity == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
