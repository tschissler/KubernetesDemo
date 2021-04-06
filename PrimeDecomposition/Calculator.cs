using System;
using System.Collections.Generic;

namespace PrimeDecomposition
{
    public class Calculator
    {
        public static IEnumerable<long> PrimeDecompositionCalc(long number)
        {
            if (number < 2)
            {
                throw new ArgumentException("number must be >1", nameof(number));
            }

            long divider = 2;
            while (divider <= number)
            {
                if (number % divider == 0)
                {
                    number = number / divider;
                    yield return divider;
                    continue;
                }

                divider++;
            }
        }
    }
}
