using EasyWallet.Business.Clients.Abstractions;
using EasyWallet.Business.Clients.Dtos;
using EasyWallet.Business.Clients.Exceptions;
using EasyWallet.Common;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWallet.Business.Clients
{
    public class CategoriesClient : ICategoriesClient
    {
        private readonly EnvironmentVariables _environmentVariables;
        private readonly ICategoryErrorService _categoryErrorService;
        private readonly ILogger<CategoriesClient> _logger;

        public CategoriesClient(
            EnvironmentVariables environmentVariables, 
            ICategoryErrorService categoryErrorService, 
            ILogger<CategoriesClient> logger)
        {
            _environmentVariables = environmentVariables;
            _categoryErrorService = categoryErrorService;
            _logger = logger;
        }

        public async Task<int> CreateCategory(int userId, string name, List<string> keywordsNames)
        {
            var request = new CreateCategoryRequest
            {
                UserId = userId,
                Name = name,
                KeywordsNames = keywordsNames
            };

            int categoryId = 0;

            try
            {
                var result = await _environmentVariables.CategoriesBaseUrl
                    .AppendPathSegment("categories")
                    .WithHeader("ApiKey", _environmentVariables.CategoriesApiKey)
                    .PostJsonAsync(request);

                var response = await result.GetJsonAsync<Response<CreateEntryResponse>>();
                categoryId = response.Data.EntryId;
            }
            catch (FlurlHttpException e)
            {
                if (e.StatusCode == 400)
                {
                    var response = await e.GetResponseJsonAsync<ErrorResponse>();

                    if (response.Error.Code == _categoryErrorService.DuplicatedCategoryNameError.Code)
                    {
                        throw new DuplicatedCategoryNameException();
                    }
                    else if (response.Error.Code == _categoryErrorService.DuplicatedKeywordNameError.Code)
                    {
                        var error = new DuplicatedKeywordNameException();
                        error.Data.Add("keywordName", response.Error.Data["KeywordName"]);
                        throw error;
                    }
                }
                else
                {
                    _logger.LogError(
                        $"Error at trying to create the category \"{request.Name}\"," +
                        $"for the userId: {request.UserId}. " +
                        $"Status code: {e.StatusCode}. " +
                        $"Message: {e.Message}");
                }
            }

            return categoryId;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesByUserId(int userId)
        {
            var categories = new List<CategoryDto>();

            try
            {
                var result = await _environmentVariables.CategoriesBaseUrl
                    .AppendPathSegment("categories")
                    .SetQueryParam("userId", userId)
                    .WithHeader("ApiKey", _environmentVariables.CategoriesApiKey)
                    .GetAsync();

                var response = await result.GetJsonAsync<Response<GetCategoriesResponse>>();
                categories = response.Data.Categories;
            }
            catch (FlurlHttpException e)
            {
                _logger.LogError(
                    $"No success at trying to get categories for the user id: {userId}. " +
                    $"Status code: {e.StatusCode}. " +
                    $"Message: {e.Message}");
            }

            return categories;
        }

        public async Task<CategoryDto> GetCategoryById(int id)
        {
            CategoryDto category = null;

            try
            {
                var result = await _environmentVariables.CategoriesBaseUrl
                    .AppendPathSegment($"categories/{id}")
                    .WithHeader("ApiKey", _environmentVariables.CategoriesApiKey)
                    .GetAsync();

                var response = await result.GetJsonAsync<Response<GetCategoryResponse>>();
                category = response.Data.Category;
            }
            catch (FlurlHttpException e)
            {
                _logger.LogError(
                    $"No success at trying to get the category for the category id: {id}. " +
                    $"Status code: {e.StatusCode}. " +
                    $"Message: {e.Message}");
            }

            return category;
        }
    }
}
