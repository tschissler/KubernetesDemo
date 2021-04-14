using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dtos;

namespace NumberGenerator
{
    public class CallPrimeDecomposition
    {
        private readonly string _primeDecompositionServiceUrl;

        private readonly ConcurrentBag<CallStatisticDto> _callStatistics;

        public CallPrimeDecomposition(string primeDecompositionServiceUrl)
        {
            _primeDecompositionServiceUrl = primeDecompositionServiceUrl;
            _callStatistics = new ConcurrentBag<CallStatisticDto>();
        }

        public IReadOnlyCollection<CallStatisticDto> CallStatistics => _callStatistics;

        public async Task Call(long number)
        {
            var client = new HttpClient();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var responseMessage = await client.GetAsync($"{_primeDecompositionServiceUrl}?number={number}");
            stopwatch.Stop();
            var result = await responseMessage.Content.ReadFromJsonAsync<ResultDto>();
            if (result != null)
            {
                _callStatistics.Add(new CallStatisticDto(stopwatch.ElapsedMilliseconds, number, result.Result.ToArray()));
            }
        }

        public void Reset()
        {
            _callStatistics.Clear();
        }
    }
}