namespace S1ASPNETBase.Models
{
    public class Category : BaseModel
    {
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}
