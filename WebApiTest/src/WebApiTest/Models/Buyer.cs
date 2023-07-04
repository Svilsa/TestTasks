using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebApiTest.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [NotMapped] public IEnumerable<int>? SalesIds => Sales?.Select(s => s.Id);
        public ICollection<Sale>? Sales { get; set; } 
    }
}