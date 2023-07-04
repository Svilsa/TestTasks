namespace WebApiTest.Models
{
    public class ProvidedProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int ProductQuantity { get; set; }

        public int SalesPointId { get; set; }
    }
}