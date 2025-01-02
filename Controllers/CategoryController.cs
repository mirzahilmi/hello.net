using Asp.Versioning;
using Hello.NET.Domain.DTOs;
using Hello.NET.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hello.NET.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/categories")]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        private readonly ICategoryService _service = service;

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<CategoryResourceResponse>>
        > GetCategories([FromQuery] PagingDto paging)
        {
            return await _service.RetrieveCategoriesAsync(paging);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResourceResponse>> GetCategory(
            [FromRoute] long id
        )
        {
            var category = await _service.RetrieveCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CategoryResourceResponse>> PostCategory(
            [FromBody] CategoryCreateRequest category
        )
        {
            var created = await _service.CreateCategoryAsync(category);
            return CreatedAtAction(
                "GetCategory",
                new { id = created.ID },
                created
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutCategory(
            [FromRoute] long id,
            [FromBody] CategoryUpdateRequest category
        )
        {
            await _service.UpdateCategoryAsync(id, category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id)
        {
            await _service.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
