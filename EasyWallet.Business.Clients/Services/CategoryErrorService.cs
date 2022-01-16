using EasyWallet.Business.Clients.Abstractions;
using EasyWallet.Business.Clients.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace EasyWallet.Business.Clients.Services
{
    public class CategoryErrorService : ICategoryErrorService
    {
        private readonly string ApiErrorsSectionName = "ApiErrors";

        private readonly List<Error> Errors;

        public Error DuplicatedCategoryNameError => Errors.FirstOrDefault(e => e.Name == "DuplicatedCategoryName");
        public Error DuplicatedKeywordNameError => Errors.FirstOrDefault(e => e.Name == "DuplicatedKeywordName");

        public CategoryErrorService(ILogger<CategoryErrorService> logger)
        {
            string location = Assembly.GetAssembly(typeof(CategoryErrorService)).Location;
            location = Path.GetDirectoryName(location);

            using (StreamReader s = new StreamReader(location + "/categoriesApiErrors.json"))
            {
                string errorsData = s.ReadToEnd();
                var errors = JsonConvert.DeserializeObject<Cats>(errorsData);
                Errors = errors.ApiErrors;
            }
        }
    }

    internal class Cats
    {
        public List<Error> ApiErrors { get; set; }
    }
}
