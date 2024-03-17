using S1ASPNETBase.Dto;

namespace S1ASPNETBase.Abstraction
{
    public interface IProductRepository
    {
        public int AddProduct(ProductDto productDto);
        public IEnumerable<ProductDto> GetProducts();
        public string GetProductsCsv();
        public bool DelProduct(string name);
        public bool UpdProduct(string name, UpdateProductsDto updateProductsDto);
    }
}
