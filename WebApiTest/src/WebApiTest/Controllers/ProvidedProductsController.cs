using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProvidedProductController : Controller
    {
        private readonly ILogger<ProvidedProductController> _logger;
        private readonly WebApiDbContext _dbContext;
        
        public ProvidedProductController(ILogger<ProvidedProductController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }
        
        [HttpGet]
        public IEnumerable<ProvidedProduct> GetProvidedProduct()
        {
            _logger.Log(LogLevel.Information, "Getting all Provided Products");
            return _dbContext.ProvidedProducts;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProvidedProduct>> GetProvidedProduct(int id)
        {
            _logger.Log(LogLevel.Information, "Getting Provided Product with ID={Id}", id);
            var providedProduct = await _dbContext.ProvidedProducts.FindAsync(id);

            if (providedProduct != null) return providedProduct;

            _logger.Log(LogLevel.Error, "Provided Product with ID={Id} NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ProvidedProduct>> PostProvidedProduct(ProvidedProduct providedProduct)
        {
            _logger.Log(LogLevel.Information, "Adding new Provided Product with ID={Id}", providedProduct.Id);
            _dbContext.ProvidedProducts.Add(providedProduct);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.Log(LogLevel.Information, "Fail to add new Provided Product with ID={Id}", providedProduct.Id);
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetProvidedProduct), new { id = providedProduct.Id }, providedProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvidedProduct(int id, ProvidedProduct newProvidedProduct)
        {
            _logger.Log(LogLevel.Information, "Updating new Provided Product with ID={Id}", newProvidedProduct.Id);
            
            if (id != newProvidedProduct.Id)
            {
                _logger.Log(LogLevel.Error, "Attempt to update the Provided Product with new ID; {Id} != {NewProvidedProductId}", id,
                    newProvidedProduct.Id);
                return BadRequest();
            }
            
            var providedProduct = await _dbContext.ProvidedProducts.FindAsync(id);
            if (providedProduct == null)
            {
                _logger.Log(LogLevel.Error, "Provided Product with ID={Id} NOT FOUND", id);
                return NotFound();
            }

            providedProduct.ProductId = newProvidedProduct.ProductId;
            providedProduct.SalesPointId = newProvidedProduct.SalesPointId;
            providedProduct.ProductQuantity = newProvidedProduct.ProductQuantity;

            await _dbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, "Provided Product with ID={Id} successfully updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvidedProduct(int id)
        {
            var providedProduct = await _dbContext.ProvidedProducts.FindAsync(id);
            if (providedProduct == null)
            {
                _logger.Log(LogLevel.Error, "Provided Product with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            _dbContext.ProvidedProducts.Remove(providedProduct);
            await _dbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, "Provided Product with ID={Id} successfully deleted", id);
            return NoContent();
        }
    }
}