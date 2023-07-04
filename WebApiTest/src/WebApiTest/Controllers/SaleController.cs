using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class SaleController : Controller
    {
        private readonly ILogger<SaleController> _logger;
        private readonly WebApiDbContext _dbContext;

        public SaleController(ILogger<SaleController> logger, WebApiDbContext webApiDbContext)
        {
            _logger = logger;
            _dbContext = webApiDbContext;
        }

        public record Order(int ProductId, int ProductQuantity);

        [HttpPost]
        public async Task<ActionResult<Sale>> Sale(int buyerId, [Required] int salesPointId, params Order[] orders)
        {
            var salesPoint = await _dbContext.SalesPoints
                .Include(sp => sp.ProvidedProducts)
                .ThenInclude(pp => pp.Product)
                .FirstOrDefaultAsync(sp => sp.Id == salesPointId);

            if (salesPoint == null)
            {
                _logger.Log(LogLevel.Error, "The Sales Point with ID={SalesPointId} NOT FOUND", salesPointId);
                return NotFound();
            }

            if (salesPoint.ProvidedProducts == null)
            {
                _logger.Log(LogLevel.Error, "No Provided Products in Sales Point with ID={SalesPointId}", salesPointId);
                return BadRequest();
            }

            var providedProductsDict = salesPoint.ProvidedProducts.ToDictionary(pp => pp.ProductId);

            //Validation
            foreach (var order in orders)
            {
                if (!providedProductsDict.ContainsKey(order.ProductId))
                {
                    _logger.Log(LogLevel.Error, "There is no product with ID={ProductId} in Sales Point={SalesPointId}",
                        order.ProductId, salesPointId);
                    return BadRequest();
                }

                if (providedProductsDict[order.ProductId].ProductQuantity < order.ProductQuantity)
                {
                    _logger.Log(LogLevel.Error,
                        "Insufficient number of product with ID={ProductId} in Sales Point={SalesPointId};" +
                        " ordered for {OrderProductQuantity} but Sales Point has only {SalesPointProductQuantity}",
                        order.ProductId, salesPointId, order.ProductQuantity,
                        providedProductsDict[order.ProductId].ProductQuantity);
                    return BadRequest();
                }
            }


            foreach (var order in orders)
                providedProductsDict[order.ProductId].ProductQuantity -= order.ProductQuantity;

            var sale = new Sale
            {
                BuyerId = buyerId,
                SaleDateTime = DateTime.UtcNow,
                SalesPointId = salesPointId,
                SalesData = orders.Select(ord => new SaleData
                {
                    ProductId = ord.ProductId,
                    ProductQuantity = ord.ProductQuantity,
                    ProductIdAmount = providedProductsDict[ord.ProductId].Product.Price * ord.ProductQuantity
                }).ToList()
            };
            
            _dbContext.Sales.Add(sale);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetSale", "Sales", new { id = sale.Id }, sale);
        }
    }
}