using AutoMapper;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;

namespace S1ASPNETBase.Repo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination)
                .ReverseMap();
            CreateMap<Category, CategoryDto>(MemberList.Destination)
                .ReverseMap();
            CreateMap<Storage, StorageDto>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
