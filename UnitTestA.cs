using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Exercise.Tests
{
    [TestCaseOrderer("XUnit.Project.Orderers.PriorityOrderer", "XUnit.Project")]
    public class UnitTestA
    {
        private IMovement prog;
        public UnitTestA()
        {
            prog = (IMovement)new ProgramA();
        }

        [Fact]
        public void Test0()
        {
            
            Assert.True(typeof(IMovement).IsInstanceOfType(prog), $"All tests will only work once the interface is correctly used. Currently you are not using the interface.");
        }


        [Theory]
        [InlineData("hello", "HELLO")]
        [InlineData("mellon pan", "MELLON PAN")]
        [InlineData("There was a long road, ahead of us.", "THERE WAS A LONG ROAD, AHEAD OF US.")]
        [InlineData("Christmas is upon us", "CHRISTMAS IS UPON US")]
        [InlineData("12345", "12345")]
        [InlineData("FINALLY WE ARE NEARLY DONE!", "FINALLY WE ARE NEARLY DONE!")]
        [InlineData("no rest for the ...", "NO REST FOR THE ...")]
        [InlineData("the end is nigh", "THE END IS NIGH")]
        public void Test1(string values, string result)
        {
            System.Random rand = new System.Random();
            string[] message = new string[rand.Next(1,2022)];
            int pos = rand.Next(0, message.Length - 1);
            message[pos] = values;
            var outcome = prog.CapitaliseText(message);
            Assert.True(outcome == result, $"You should have returned: <{result}> but did return <{outcome}>.");
        }

        [Theory]
        [InlineData('w', 0)]
        [InlineData('W', 0)]
        [InlineData('a', 3)]
        [InlineData('s', 1)]
        [InlineData('S', 1)]
        [InlineData('A', 3)]
        [InlineData('L', 2)]
        [InlineData('i', 0)]
        [InlineData('d', 2)]
        [InlineData('k', 1)]
        [InlineData('J', 3)]
        public void Test2(char values, int result)
        {
            var outcome = prog.Move(values);
            Assert.True(outcome == result, $"You should have returned: <{result}> but did return <{outcome}>.");
        }

        [Theory]
        [InlineData(0, IMovement.Compass.North)]
        [InlineData(3, IMovement.Compass.West)]
        [InlineData(1, IMovement.Compass.South)]
        [InlineData(2, IMovement.Compass.East)]
        public void Test3(int values, IMovement.Compass result)
        {
            var outcome = prog.Move(values);
            Assert.True(outcome == result, $"You should have returned: <{result}> but did return <{outcome}>.");
        }


        [Theory]
        [InlineData(0, IMovement.Compass.North)]
        [InlineData(3, IMovement.Compass.West)]
        [InlineData(1, IMovement.Compass.South)]
        [InlineData(2, IMovement.Compass.East)]
        public void Test4(int values, IMovement.Compass result)
        {
            var outcome = prog.Move(result);
            Assert.True(outcome == values, $"You should have returned: <{result}> but did return <{outcome}>.");
        }

    }
}
