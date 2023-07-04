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
    public class SalesDataController : Controller
    {
        private readonly ILogger<SalesDataController> _logger;
        private readonly WebApiDbContext _dbContext;
        
        public SalesDataController(ILogger<SalesDataController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }
        
        [HttpGet]
        public IEnumerable<SaleData> GetSalesData()
        {
            _logger.Log(LogLevel.Information, "Getting all sales data");
            return _dbContext.SalesData;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleData>> GetSaleData(int id)
        {
            _logger.Log(LogLevel.Information, "Getting Sale Data with ID={Id}", id);
            var saleData = await _dbContext.SalesData.FindAsync(id);

            if (saleData != null) return saleData;

            _logger.Log(LogLevel.Error, "Sale Data with ID={Id} NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SaleData>> PostSaleData(SaleData saleData)
        {
            _logger.Log(LogLevel.Information, "Adding new Sale Data with ID={Id}", saleData.Id);
            _dbContext.SalesData.Add(saleData);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.Log(LogLevel.Information, "Fail to add new Sale Data with ID={Id}", saleData.Id);
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetSaleData), new { id = saleData.Id }, saleData);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleData(int id, SaleData newSaleData)
        {
            _logger.Log(LogLevel.Information, "Updating new Sale Data with ID={Id}", newSaleData.Id);
            
            if (id != newSaleData.Id)
            {
                _logger.Log(LogLevel.Error, "Attempt to update the Sale Data with new ID; {Id} != {NewSaleDataId}", id,
                    newSaleData.Id);
                return BadRequest();
            }
            
            var saleData = await _dbContext.SalesData.FindAsync(id);
            if (saleData == null)
            {
                _logger.Log(LogLevel.Error, "Sale Data with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            saleData.SaleId = newSaleData.SaleId;
            saleData.ProductId = newSaleData.ProductId;
            saleData.ProductQuantity = newSaleData.ProductQuantity;
            saleData.ProductIdAmount = newSaleData.ProductIdAmount;

            await _dbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, "Sale Data with ID={Id} successfully updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleData(int id)
        {
            var saleData = await _dbContext.SalesData.FindAsync(id);
            if (saleData == null)
            {
                _logger.Log(LogLevel.Error, "Sale Data with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            _dbContext.SalesData.Remove(saleData);
            await _dbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, "Sale Data with ID={Id} successfully deleted", id);
            return NoContent();
        }
    }
}