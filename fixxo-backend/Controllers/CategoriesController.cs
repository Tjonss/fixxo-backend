using fixxo_backend.Contexts;
using fixxo_backend.Models.Entities;
using fixxo_backend.Models.Requests;
using fixxo_backend.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace fixxo_backend.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryRequest req)
        {
            try
            {
                var category = new CategoryEntity
                { Name = req.Name };

                _context.Add(category);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new CategoryResponse
                {
                     Id = category.Id,
                     Name = category.Name,
                });
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return new OkObjectResult(await _context.Categories.ToListAsync());

            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                return new OkObjectResult(await _context.Categories.FirstOrDefaultAsync(x => x.Id == id));
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return NoContent();
        }
    }
}
