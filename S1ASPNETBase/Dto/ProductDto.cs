using S1ASPNETBase.Models;

namespace S1ASPNETBase.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Cost { get; set; }
        public int CategoryId { get; set; }
    }
}
