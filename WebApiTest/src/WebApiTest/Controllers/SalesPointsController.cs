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
    public class SalesPointsController : Controller
    {
        private readonly ILogger<SalesPointsController> _logger;
        private readonly WebApiDbContext _dbContext;
        
        public SalesPointsController(ILogger<SalesPointsController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }
        
        [HttpGet]
        public IEnumerable<SalesPoint> GetSalesPoints()
        {
            _logger.Log(LogLevel.Information, "Getting all sales points");
            return _dbContext.SalesPoints.Include(u => u.ProvidedProducts).ToList();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesPoint>> GetSalesPoint(int id)
        {
            _logger.Log(LogLevel.Information, "Getting Sales Point with ID={Id}", id);
            var salesPoint = await _dbContext.SalesPoints.FindAsync(id);

            if (salesPoint != null) return salesPoint;

            _logger.Log(LogLevel.Error, "Sales Point with ID={Id} NOT FOUND", id);
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SalesPoint>> PostSalesPoint(SalesPoint salesPoint)
        {
            _logger.Log(LogLevel.Information, "Adding new Sales Point with ID={Id}", salesPoint.Id);
            _dbContext.SalesPoints.Add(salesPoint);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                _logger.Log(LogLevel.Information, "Fail to add new Sales Point with ID={Id}", salesPoint.Id);
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetSalesPoint), new { id = salesPoint.Id }, salesPoint);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesPoint(int id, SalesPoint newSalesPoint)
        {
            _logger.Log(LogLevel.Information, "Updating new Sales Point with ID={Id}", newSalesPoint.Id);
            
            if (id != newSalesPoint.Id)
            {
                _logger.Log(LogLevel.Error, "Attempt to update the Sales Point with new ID; {Id} != {NewSalesPointId}", id,
                    newSalesPoint.Id);
                return BadRequest();
            }
            
            var salesPoint = await _dbContext.SalesPoints.FindAsync(id);
            if (salesPoint == null)
            {
                _logger.Log(LogLevel.Error, "Sales Point with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            salesPoint.Name = newSalesPoint.Name;

            await _dbContext.SaveChangesAsync();
            _logger.Log(LogLevel.Information, "Sales Point with ID={Id} successfully updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesPoint(int id)
        {
            var salesPoint = await _dbContext.SalesPoints.FindAsync(id);
            if (salesPoint == null)
            {
                _logger.Log(LogLevel.Error, "Sales Point with ID={Id} NOT FOUND", id);
                return NotFound();
            }
            
            _dbContext.SalesPoints.Remove(salesPoint);
            await _dbContext.SaveChangesAsync();

            _logger.Log(LogLevel.Information, "Sales Point with ID={Id} successfully deleted", id);
            return NoContent();
        }
    }
}