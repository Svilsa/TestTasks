using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuyersController : Controller
    {
        private readonly ILogger<BuyersController> _logger;
        private readonly WebApiDbContext _dbContext;
        
        public BuyersController(ILogger<BuyersController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }
        
        [HttpGet]
        public IEnumerable<Buyer> GetBuyers()
        {
            _logger.Log(LogLevel.Information, "Getting all Buyers");
            return _dbContext.Buyers.Include(b => b.Sales).ThenInclude(s => s.SalesData);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Buyer>> GetBuyer(int id)
        {
            _logger.Log(LogLevel.Information, "Getting Buyer with ID={Id}", id);
            var buyer = await _dbContext.Buyers.FindAsync(id);

            if (buyer != null) return buyer;

            _logger.Log(LogLevel.Error, "Buyer with ID={Id} NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Buyer>> PostBuyer(Buyer buyer)
        {
            _logger.Log(LogLevel.Information, "Adding new Buyer with ID={Id}", buyer.Id);
            _dbContext.Buyers.Add(buyer);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.Log(LogLevel.Information, "Fail to add new Buyer with ID={Id}", buyer.Id);
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetBuyer), new { id = buyer.Id }, buyer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuyer(int id, Buyer newBuyer)
        {
            _logger.Log(LogLevel.Information, "Updating new Buyer with ID={Id}", newBuyer.Id);
            
            if (id != newBuyer.Id)
            {
                _logger.Log(LogLevel.Error, "Attempt to update the Buyer with new ID; {Id} != {NewProductId}", id,
                    newBuyer.Id);
                return BadRequest();
            }
            
            var buyer = await _dbContext.Buyers.FindAsync(id);
            if (buyer == null)
            {
                _logger.Log(LogLevel.Error, "Buyer with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            buyer.Name = newBuyer.Name;
            
            await _dbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, "Buyer with ID={Id} successfully updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuyer(int id)
        {
            var buyer = await _dbContext.Buyers.FindAsync(id);
            if (buyer == null)
            {
                _logger.Log(LogLevel.Error, "Buyer with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            _dbContext.Buyers.Remove(buyer);
            await _dbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, "Buyer with ID={Id} successfully deleted", id);
            return NoContent();
        }
    }
}