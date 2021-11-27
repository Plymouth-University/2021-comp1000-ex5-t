using System;
using System.Collections.Generic;
using Xunit;


namespace Exercise.Tests
{
    
    public class UnitTestA
    {
        private Exercise.ProgramA prog;
        public UnitTestA()
        {
            prog = new ProgramA();
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 6)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11 }, 3991680)]
        [InlineData(new int[] { -1, -2, -3 }, -6)]
        [InlineData(new int[] { -1, -2, -3, 6, 14, 3, 3, 1, 1  }, -4536)]
        public void Test1(int[] values, int result)
        {
            var outcome = prog.RecursiveMultiplicationFromFirst(values, 0);
            Assert.True(outcome == result, $"For RecursiveMultiplicationFromFirst: You should have returned: {result} but did return {outcome}.");
        }


        [Theory]
        [InlineData(new int[] { 1, 2 }, 3, -1)]
        [InlineData(new int[] { 1 }, 1, -1)]
        [InlineData(new int[] { 1, -2 }, -1, -1)]
        [InlineData(new int[] {1, 2, 3, 4 }, 6, 2)]
        [InlineData(new int[] { 1, -1, 2, 3, 67, 1000, 15, -12, -1000, 2 }, 1075, 7)]
        [InlineData(new int[] { 1, -1, 2, 3, 67, 1000, 15, -12, -1000, 2 }, 0, 1)]
        public void Test2(int [] values, int result, int end)
        {
            var outcome = prog.RecursiveAdditionFromLast(values,end == -1? values.Length-1 : end);
            Assert.True(outcome == result, $"For RecursiveAdditionFromLast: You should have returned: {result} but did return {outcome}.");
        }



        [Theory]
        [InlineData(new double[] { 1.0f, 2.0f, 3.0f }, 6.0f)]
        [InlineData(new double[] { 1.0f, -2.0f, 3.0f, 2.00f }, 4.0f)]
        [InlineData(new double[] { -1.0001f, 2.0f, 3.0f, 2.0f }, 6.0f)]
        [InlineData(new double[] { -1.103f, 2.2f, 3.1f, 10.0f, 15.0f, 23.1f, 22,12f }, 86.30f)]
        public void Test3(double[] values, float result)
        {
            var test = new List<double>(values); 
            var outcome = prog.RecursiveSum(test);

            Assert.True(outcome.Equals(Math.Round(result,2)), $"For RecursiveSum: You should have returned: {result} but did return {outcome}.");
        }

    }
}
