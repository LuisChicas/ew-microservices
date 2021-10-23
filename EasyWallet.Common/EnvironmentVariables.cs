using Microsoft.Extensions.Configuration;

namespace EasyWallet.Common
{
    public class EnvironmentVariables
    {
        public string EntriesBaseUrl => _configuration["ENTRIES_BASE_URL"];
        public string EntriesApiKey => _configuration["ENTRIES_API_KEY"];

        private readonly IConfiguration _configuration;

        public EnvironmentVariables(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
