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
                        CategoryId = req.CategoryId
                    });
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(new ProductResponse
                    {
                        Name = req.Name,
                        Description = req.Description,
                        Price = req.Price,
                        CategoryId = req.CategoryId
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
                foreach(var product in await _context.Products.ToListAsync())
                    products.Add(new ProductResponse 
                    { 
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId
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
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if(product != null)
                {
                    return new OkObjectResult(new ProductResponse
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId
                    });
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return new BadRequestResult();
        }
    }
}
