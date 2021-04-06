using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeDecomposition;
using FluentAssertions;

namespace PrimeDecompositionTests
{
    [TestClass]
    public class PrimeDecompositionCalcTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        public void CheckIfNumberIs2OrBigger(int input)
        {
            FluentActions.Invoking(() => Calculator.PrimeDecompositionCalc(input).ToList()).Should().ThrowExactly<ArgumentException>();
        }

        [TestMethod]
        public void CheckIfNumber2Returns2()
        {
            Calculator.PrimeDecompositionCalc(2).Should().Contain(2).And.HaveCount(1);
        }

        [TestMethod]
        [DataRow(2, new long[] { 2 })]
        [DataRow(4, new long[] { 2, 2 })]
        [DataRow(5, new long[] { 5 })]
        [DataRow(6, new long[] { 2, 3 })]
        [DataRow(9, new long[] { 3, 3 })]
        [DataRow(16, new long[] { 2, 2, 2, 2 })]
        [DataRow(450, new long[] { 2, 3, 3, 5, 5 })]
        [DataRow(938475008, new long[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 1231, 1489 })]
        [DataRow(9384750088, new long[] { 2, 2, 2, 7, 7, 29, 181, 4561 })]
        //[DataRow(5748953654873, new long[] {11, 13, 40202473111 })]
        public void CheckIfNumber4Returns2And2(long input, long[] factors)
        {
            Calculator.PrimeDecompositionCalc(input).Should().BeEquivalentTo(factors);
        }
    }
}
