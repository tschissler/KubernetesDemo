using System.ComponentModel.DataAnnotations;

namespace PrimeDecompositionUi.Data
{
    public class NumberGeneratorParameter
    {
        [Required]
        [Range(2, long.MaxValue, ErrorMessage = "Number should be between 2 and 9223372036854775807")]
        public long MaxNumber { get; set; } = 5000;

        [Required]
        [Range(100, 10000, ErrorMessage = "Interval should be between 100 and 10000 ms")]
        public long IntervalInMillis { get; set; } = 1000;
    }
}