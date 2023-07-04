using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly WebApiDbContext _dbContext;

        public ProductsController(ILogger<ProductsController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            _logger.Log(LogLevel.Information, "Getting all products");
            return _dbContext.Products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.Log(LogLevel.Information, "Getting product with ID={Id}", id);
            var product = await _dbContext.Products.FindAsync(id);

            if (product != null) return product;

            _logger.Log(LogLevel.Error, "Product with ID={Id} NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _logger.Log(LogLevel.Information, "Adding new product with ID={Id}", product.Id);
            _dbContext.Products.Add(product);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.Log(LogLevel.Information, "Fail to add new product with ID={Id}", product.Id);
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product newProduct)
        {
            _logger.Log(LogLevel.Information, "Updating new product with ID={Id}", newProduct.Id);
            
            if (id != newProduct.Id)
            {
                _logger.Log(LogLevel.Error, "Attempt to update the product with new ID; {Id} != {NewProductId}", id,
                    newProduct.Id);
                return BadRequest();
            }
            
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                _logger.Log(LogLevel.Error, "Product with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;

            await _dbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, "Product with ID={Id} successfully updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                _logger.Log(LogLevel.Error, "Product with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, "Product with ID={Id} successfully deleted", id);
            return NoContent();
        }
    }
}