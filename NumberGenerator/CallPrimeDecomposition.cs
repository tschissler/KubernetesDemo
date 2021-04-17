using System;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace NumberGenerator
{
    public class CallPrimeDecomposition
    {
        private readonly string _primeDecompositionServiceUrl;
        private readonly HttpClient _httpClient;

        public CallPrimeDecomposition(string primeDecompositionServiceUrl)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
            _primeDecompositionServiceUrl = primeDecompositionServiceUrl;
        }

        public async Task<bool> Call(long number)
        {
            try
            {
                await _httpClient.GetAsync($"{_primeDecompositionServiceUrl}?number={number}");
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Request to Prime Decomposition failed");
                return false;
            }
        }
    }
}