using EasyWallet.Business.Clients.Dtos;

namespace EasyWallet.Business.Clients.Abstractions
{
    public interface ICategoryErrorService
    {
        Error DuplicatedCategoryNameError { get; }
        Error DuplicatedKeywordNameError { get; }
    }
}
