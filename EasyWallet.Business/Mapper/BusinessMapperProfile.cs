using AutoMapper;
using EasyWallet.Business.Models;
using EasyWallet.Data.Entities;

namespace EasyWallet.Business.Mapper
{
    internal class BusinessMapperProfile : Profile
    {
        public BusinessMapperProfile()
        {
            CreateMap<UserData, User>().ReverseMap();
            CreateMap<CategoryData, Category>().ReverseMap();
            CreateMap<TagData, Tag>().ReverseMap();
        }
    }
}
