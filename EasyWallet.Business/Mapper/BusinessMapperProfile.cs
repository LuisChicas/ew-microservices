using AutoMapper;
using EasyWallet.Business.Models;
using EasyWallet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWallet.Business.Mapper
{
    class BusinessMapperProfile : Profile
    {
        public BusinessMapperProfile()
        {
            CreateMap<UserData, User>().ReverseMap();
        }
    }
}
