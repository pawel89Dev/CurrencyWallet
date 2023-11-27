using CurrencyWallet.Core.Abstractions;
using CurrencyWallet.Core.Exceptions;
using CurrencyWallet.DTO.Models;
using System.Net.Http.Json;

namespace CurrencyWallet.Core.Component
{
    internal class NBPRatesProvider : ICurrencyRatesProvider
    {
        private const string NbpRatesAddres = "http://api.nbp.pl/api/exchangerates/tables/B";
        private readonly IHttpClientFactory _httpClientFactory;
        public NBPRatesProvider(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory;
        }
        public async Task<NBPResponse> GetRates()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<NBPResponse>>(NbpRatesAddres);
                return response.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new NBPProviderException(ex);
            }
        }
    }
}
