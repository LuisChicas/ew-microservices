using AutoMapper;
using EasyWallet.Business.Clients.Dtos;
using EasyWallet.Business.Models;
using EasyWallet.Data.Entities;

namespace EasyWallet.Business.Mapper
{
    internal class BusinessMapperProfile : Profile
    {
        public BusinessMapperProfile()
        {
            CreateMap<UserData, User>().ReverseMap();
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Keywords))
                .ReverseMap();
            CreateMap<KeywordDto, Tag>().ReverseMap();
            //CreateMap<EntryData, Entry>().ReverseMap();
        }
    }
}
