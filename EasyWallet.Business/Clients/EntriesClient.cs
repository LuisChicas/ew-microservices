using EasyWallet.Business.Abstractions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using EasyWallet.Business.Dtos;
using EasyWallet.Common;
using EasyWallet.Business.Dtos.Reports;
using Microsoft.Extensions.Logging;
using System;

namespace EasyWallet.Business.Clients
{
    public class EntriesClient : IEntriesClient
    {
        private readonly EnvironmentVariables _environmentVariables;
        private readonly ILogger<EntriesClient> _logger;

        public EntriesClient(EnvironmentVariables environmentVariables, ILogger<EntriesClient> logger)
        {
            _environmentVariables = environmentVariables;
            _logger = logger;
        }

        public async Task<int> CreateEntry(CreateEntryRequest request)
        {
            int entryId = 0;

            try
            {
                var result = await _environmentVariables.EntriesBaseUrl
                    .AppendPathSegment("entries/create")
                    .WithHeader("ApiKey", _environmentVariables.EntriesApiKey)
                    .PostJsonAsync(request);

                if (result.StatusCode == 200)
                {
                    var response = await result.GetJsonAsync<Response<CreateEntryResponse>>();
                    entryId = response.Data.EntryId;
                }
                else
                {
                    _logger.LogError(
                        $"No success at trying to create an entry for the keyword ID: {request.KeywordId}. " +
                        $"Status code: {result.StatusCode}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Error at trying to create an entry for the keyword ID: {request.KeywordId}. " +
                    $"Message: {e.Message}");
            }

            return entryId;
        }

        public async Task<bool> DeleteEntry(int entryId)
        {
            try
            {
                var result = await _environmentVariables.EntriesBaseUrl
                    .AppendPathSegment($"entries/delete/{entryId}")
                    .WithHeader("ApiKey", _environmentVariables.EntriesApiKey)
                    .DeleteAsync();

                if (result.StatusCode != 200)
                {
                    _logger.LogError(
                        $"No success at trying to delete an entry with the ID: {entryId}. " +
                        $"Status code: {result.StatusCode}");
                }

                return result.StatusCode == 200;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Error at trying to delete an entry with the ID: {entryId}. " +
                    $"Message: {e.Message}");

                return false;
            }
        }

        public async Task<HistoryReport> GetHistoryReport(int userId)
        {
            HistoryReport report = null;

            try
            {
                var result = await _environmentVariables.EntriesBaseUrl
                    .AppendPathSegment($"reports/history/{userId}")
                    .WithHeader("ApiKey", _environmentVariables.EntriesApiKey)
                    .GetAsync();

                if (result.StatusCode == 200)
                {
                    var response = await result.GetJsonAsync<Response<HistoryReport>>();
                    report = response.Data;
                }
                else
                {
                    _logger.LogError($"No success at retrieving the history report. Status code: {result.StatusCode}");
                }
            }
            catch(Exception e)
            {
                _logger.LogError($"Error at retrieving the history report. Message: {e.Message}");
            }

            return report;
        }

        public async Task<MonthlyReport> GetMonthlyReport(int userId)
        {
            MonthlyReport report = null;

            try
            {
                var result = await _environmentVariables.EntriesBaseUrl
                    .AppendPathSegment($"reports/monthly/{userId}")
                    .WithHeader("ApiKey", _environmentVariables.EntriesApiKey)
                    .GetAsync();

                if (result.StatusCode == 200)
                {
                    var response = await result.GetJsonAsync<Response<MonthlyReport>>();
                    report = response.Data;
                }
                else
                {
                    _logger.LogError($"No success at retrieving the monthly report. Status code: {result.StatusCode}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error at retrieving the monthly report. Message: {e.Message}");
            }

            return report;
        }

        public async Task<BalanceReport> GetBalanceReport(int userId, int incomeCategoryId)
        {
            BalanceReport report = null;

            try
            {
                var result = await _environmentVariables.EntriesBaseUrl
                    .AppendPathSegment($"reports/balance/{userId}/{incomeCategoryId}")
                    .WithHeader("ApiKey", _environmentVariables.EntriesApiKey)
                    .GetAsync();

                if (result.StatusCode == 200)
                {
                    var response = await result.GetJsonAsync<Response<BalanceReport>>();
                    report = response.Data;
                }
                else
                {
                    _logger.LogError($"No success at retrieving the balance report. Status code: {result.StatusCode}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error at retrieving the balance report. Message: {e.Message}");
            }

            return report;
        }
    }
}
