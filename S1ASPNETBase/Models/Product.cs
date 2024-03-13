namespace S1ASPNETBase.Models
{
    public class Product : BaseModel
    {
        public string? Description { get; set; }
        public int Cost { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual List<Storage>? Storages { get; set; } = new List<Storage>();

    }
}
