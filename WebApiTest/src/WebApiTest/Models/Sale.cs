using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiTest.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int SalesPointId { get; set; }
        public int? BuyerId { get; set; }
        
        public DateTime SaleDateTime { get; set; }
        public DateOnly Date => DateOnly.FromDateTime(SaleDateTime);
        public TimeOnly Time => TimeOnly.FromDateTime(SaleDateTime);
        
        public ICollection<SaleData>? SalesData { get; set; }
        public double TotalAmount
        {
            get
            {
                if (SalesData != null) return SalesData.Sum(s => s.ProductIdAmount);
                return 0;
            }
        }
    }
}