namespace ProductAPI.Models
{
    public class Product
    {        
        public string? CategoryId { get; set; }
        public string? ProductId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }        
        public double Rating { get; set; }
        public double RateCount { get; set; }
    }
}
