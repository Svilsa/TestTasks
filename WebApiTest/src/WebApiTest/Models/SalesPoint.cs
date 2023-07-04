using System.Collections.Generic;

namespace WebApiTest.Models
{
    public class SalesPoint
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ProvidedProduct>? ProvidedProducts { get; set; }
    }
}