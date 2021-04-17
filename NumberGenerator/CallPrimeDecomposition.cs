using System.Net.Http;
using System.Threading.Tasks;

namespace NumberGenerator
{
    public class CallPrimeDecomposition
    {
        private readonly string _primeDecompositionServiceUrl;
        private readonly HttpClient _httpClient;

        public CallPrimeDecomposition(string primeDecompositionServiceUrl)
        {
            _httpClient = new HttpClient();
            _primeDecompositionServiceUrl = primeDecompositionServiceUrl;
        }

        public Task Call(long number)
        {
            return _httpClient.GetAsync($"{_primeDecompositionServiceUrl}?number={number}");
        }
    }
}