using AutoMapper;
using System;

namespace EasyWallet.Business.Mapper
{
    internal static class BusinessMapper
    {
        public static IMapper Mapper => Lazy.Value;

        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                config.AddProfile<BusinessMapperProfile>();
            });

            return configuration.CreateMapper();
        });
    }
}
