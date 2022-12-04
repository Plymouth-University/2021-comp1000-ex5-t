using Xunit;


namespace Exercise.Tests
{
    [TestCaseOrderer("XUnit.Project.Orderers.PriorityOrderer", "XUnit.Project")]
    public class UnitTestB
    {
        private Exercise.ProgramB prog;
        public UnitTestB()
        {
            prog = new ProgramB();
        }
        [Theory]
        [InlineData("Hello my friend.", ' ', "Friend my hello.")]
        [InlineData("HelloYmyYfriend.", 'Y', "FriendYmyYhello.")]
        [InlineData("Hello myYfriend.", 'Y', "FriendYhello my.")]
        [InlineData(" Let'sxgo xtoxthexpark.", 'x', "Parkxthextoxgo x let's.")]
        [InlineData(" Let'sXgoxXtoXtheXpark.", 'X', "ParkXtheXtoXgoxX let's.")]
        [InlineData("One ale,please.", ',', "Please,one ale.")]
        public void Test0(string a, char c, string b)
        {
            var value = prog.ReorderSentence(a, c);
            Assert.True(value == b, $"You returned {value} but should have returned {b}");
        }

        [Theory]
        [InlineData(new int[] { 1}, 1, 10)]
        [InlineData(new int[] { 1, 2 }, 1, 2)]
        [InlineData(new int[] { 1, 2, 3 }, 6, 4)]
        [InlineData(new int[] {  }, 0, 1000)]
        [InlineData(new int[] { 1, 9, 4, 0, -2, -9, 9 }, -6, 8)]
        [InlineData(new int[] { -6, 1, -7, 9, -10, -3, -21, -2, 7, 2, -3, -10, 2, 8, 3, -10, 8, -7, -9, 5 }, -43, 45)]
        [InlineData(new int[] { -86, -28, -32, 15, -41, -63, -63, -64, -9, -78, 60, 2, 18, -38, 27, -79, 27, 95, -68, 70, 29, -18, 27, -90, 94, -99, 20, -70, -61, -61, -31, -54, -25, -64, -93, -91, -64, 55, -21, 95 }, -1211, 67)]
        [InlineData(new int[] { -1466, 1118, -7022, 2926, 2389, -5760, -1442, 730, 8414, 4140, -6643, -9534, -8859, -239, -157, 9323, 9085, 6565, -5254, -7670 }, -54046, 590)]
        public void Test1(int[] values, int result, int border)
        {
            var outcome = prog.AddValuesBelowBoundary(values, border);
            Assert.True(outcome == result, $"You should have returned {result} but did return {outcome}.");
        }

        [Theory]
        [InlineData("What shall we do?", ' ', "shall", 1, 2)]
        [InlineData("Hello my friend, how are you?", ' ', "friend, how", 2,4)]
        [InlineData("Helloxmyxfriend,xhowxarexyou?", 'x', "friend,xhow", 2, 4)]
        [InlineData("Hello friend, how are you?", ' ', "friend, how", 1, 3)]
        [InlineData("Du hast so viele Träume, doch du denkst, es wär zu spät. Und du glaubst, du bist der Einzige, dem es so geht. Du bist nicht allein, du bist nicht allein", 'ä', "ume, doch du denkst, es w", 1,2)]
        [InlineData("We danced and sang, and the music played in a de boomtown.", 'n', "g, and the music played in a de boomtow", 3,6)]
        public void Test2(string a, char split, string result, int s, int e)
        {
            var value = prog.FindSpecificSubText(a, split, s, e);
            Assert.True(value == result, $"You returned <{value}> but should have returned <{result}>");
        }

    }
}
