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
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequest req)
        {
            try 
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == req.CategoryId);
                if(category != null)
                {
                    _context.Add(new ProductEntity { 
                        Name = req.Name,
                        Description = req.Description,
                        Price = req.Price,
                        ImageURL = req.ImageURL,
                        CategoryId = req.CategoryId
                    });
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new ProductResponse
                    {
                        Name = req.Name,
                        Description = req.Description,
                        Price = req.Price,
                        ImageURL = req.ImageURL,
                        CategoryId = req.CategoryId,
                        
                    });  
                }               
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = new List<ProductResponse>();
                foreach(var product in await _context.Products.Include(x => x.Category).ToListAsync())
                    products.Add(new ProductResponse 
                    { 
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        ImageURL = product.ImageURL,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name,
                        Id = product.Id
                        
                        
                    });
                await _context.SaveChangesAsync();
                return new OkObjectResult(products);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            try
            {
                var product = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
                if(product != null)
                {
                    return new OkObjectResult(new ProductResponse
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        ImageURL = product.ImageURL,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name
                    });
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if(product == null)
                {
                    return NotFound();
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return NoContent();
        }
    }
}
