using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace NumberGenerator
{
    public class CallPrimeDecomposition
    {
        private string _primeDecompositionServiceUrl;

        private ConcurrentBag<CallStatistic> _callStatistics;

        public CallPrimeDecomposition(string primeDecompositionServiceUrl)
        {
            _primeDecompositionServiceUrl = primeDecompositionServiceUrl;
            _callStatistics = new ConcurrentBag<CallStatistic>();
        }

        public IReadOnlyCollection<CallStatistic> CallStatistics => _callStatistics;

        public async Task Call(long number)
        {
            var client = new HttpClient();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var responseMessage = await client.GetAsync($"{_primeDecompositionServiceUrl}?number={number}");
            stopwatch.Stop();
            var result = await responseMessage.Content.ReadFromJsonAsync<ResultDto>();
            _callStatistics.Add(new CallStatistic
            {
                DurationInMilliseconds = stopwatch.ElapsedMilliseconds,
                Number = number,
                Result = result.Result.ToArray()
            });
        }

        private class ResultDto
        {
            public List<long> Result { get; set; }
        }
    }
}