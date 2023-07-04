namespace WebApiTest.Models
{
    public class SaleData
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductIdAmount { set; get; }
        
        public int SaleId { get; set; }
    }
}