using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        private readonly WebApiDbContext _dbContext;
        
        public SalesController(ILogger<SalesController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }
        
        [HttpGet]
        public IEnumerable<Sale> GetSales()
        {
            _logger.Log(LogLevel.Information, "Getting all sales");
            return _dbContext.Sales.Include(x => x.SalesData);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            _logger.Log(LogLevel.Information, "Getting sale with ID={Id}", id);
            var sale = await _dbContext.Sales.FindAsync(id);

            if (sale != null) return sale;

            _logger.Log(LogLevel.Error, "Sale with ID={Id} NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(Sale sale)
        {
            _logger.Log(LogLevel.Information, "Adding new sale with ID={Id}", sale.Id);
            _dbContext.Sales.Add(sale);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.Log(LogLevel.Information, "Fail to add new sale with ID={Id}", sale.Id);
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale newSale)
        {
            _logger.Log(LogLevel.Information, "Updating new sale with ID={Id}", newSale.Id);
            
            if (id != newSale.Id)
            {
                _logger.Log(LogLevel.Error, "Attempt to update the sale with new ID; {Id} != {NewSaleId}", id,
                    newSale.Id);
                return BadRequest();
            }
            
            var sale = await _dbContext.Sales.FindAsync(id);
            if (sale == null)
            {
                _logger.Log(LogLevel.Error, "Sale with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            sale.SalesPointId = newSale.SalesPointId;
            sale.BuyerId = newSale.BuyerId;
            sale.SaleDateTime = newSale.SaleDateTime;

            await _dbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, "Sale with ID={Id} successfully updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _dbContext.Sales.FindAsync(id);
            if (sale == null)
            {
                _logger.Log(LogLevel.Error, "Sale with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            _dbContext.Sales.Remove(sale);
            await _dbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, "Sale with ID={Id} successfully deleted", id);
            return NoContent();
        }
    }
}