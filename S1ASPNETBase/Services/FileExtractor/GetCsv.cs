using S1ASPNETBase.Dto;
using System.Text;

namespace S1ASPNETBase.Services.FileExtractor
{
    public class GetCsv
    {
        public static string GetProducts(IEnumerable<ProductDto> types)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in types)
            {
                sb.Append(t.Id + ";" + t.Name + ";" + t.Cost + ";" + t.Description + ";" + t.CategoryId + "\n");
            }
            return sb.ToString();
        }
    }
}
