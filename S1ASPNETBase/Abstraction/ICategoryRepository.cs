using S1ASPNETBase.Dto;

namespace S1ASPNETBase.Abstraction
{
    public interface ICategoryRepository
    {
        public int AddCategory(CategoryDto categoryDto);
        public IEnumerable<CategoryDto> GetCategories();
    }
}
