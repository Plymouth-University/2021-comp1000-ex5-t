using System;
using System.Collections.Generic;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Exercise.Tests
{
    public class UnitTestB
    {
        private Exercise.ProgramB prog;
        public UnitTestB()
        {
            prog = new ProgramB();
        }

        [Theory]
        [InlineData("files/file1.txt", 0)]
        [InlineData("file/file1.txt", -2)]
        [InlineData("files/files1.txt", -1)]
        public void Test1(string values, int result)
        {
            var outcome = prog.CheckFileAvailability(values);
            
            Assert.True(outcome == result, $"For CheckFileAvailability: When trying to open a file, you should have returned state: {result} but did return {outcome}.");
        }

        [Theory]
        [InlineData("files/file1.txt", 0, 0, 0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lobortis, ligula at hendrerit facilisis, eros risus dapibus dui, et volutpat dui nibh vel nisi. Morbi gravida sapien ac odio tincidunt tristique. Praesent tristique libero tristique tincidunt varius. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. ")]
        [InlineData("files/file2.map", 0, 3, 2, "Duis vel sagittis elit. Pellentesque et viverra nibh. Proin sed lectus justo. Aliquam volutpat laoreet nisi a placerat. Sed nulla erat, volutpat in dictum ac, pulvinar non enim. Mauris finibus lacus fermentum facilisis bibendum. Mauris dui tortor, vehicula eu libero condimentum, sodales volutpat sem. Nulla facilisi.")]
        [InlineData("files/file2.map", 0, 3, 1, "")]
        [InlineData("files/file2.map", 3, 6, 1, "Sed tincidunt, velit quis venenatis rutrum, nibh orci consequat dui, ac tristique erat dolor a dolor. Mauris gravida justo ligula, vel rutrum velit bibendum et. Quisque sit amet felis elit. Nam pellentesque venenatis nisi. Donec ultricies elit vitae tortor semper pharetra. Morbi lobortis, nulla vel dignissim rhoncus, magna risus semper orci, quis pellentesque odio felis sit amet libero. Suspendisse vel felis at orci volutpat maximus et et leo. Sed tempus accumsan purus, ac elementum metus. Ut finibus fermentum justo nec efficitur. Fusce malesuada, velit at tristique aliquet, lacus mauris congue est, vitae accumsan enim ex quis tellus.")]
        [InlineData("files/file3.txt", 1, 7, 5, "Phasellus ultricies mi sed lacus egestas pretium ut eu purus. Donec vel nulla vel mi pharetra efficitur. Praesent felis mi, efficitur at ullamcorper nec, pharetra ut nulla. Vestibulum sagittis gravida eros sit amet mattis. Praesent condimentum mattis dapibus. Donec iaculis mauris fermentum velit dignissim pulvinar. Aliquam ornare vulputate quam et finibus. Nulla justo mi, euismod vitae tristique eu, ullamcorper ut erat. Aenean et diam ut dolor fermentum euismod vitae a massa. Nulla rutrum dolor nec eleifend faucibus. Phasellus facilisis dui vel molestie lobortis.")]
        public void Test2(string values, int start, int stop, int result, string line)
        {
            var outcome = prog.NonPersistentSpecificLineReading(values, start, stop);

            Assert.True(outcome[result].Equals(line), $"For NonPersistentSpecificLineReading: When trying to open and read a file: From the file, you should have returned {result + 1} lines where line nr.{result} is:\n \"{line}\" \nbut did return: \n\"{outcome[result]}\".");
        }

        [Theory]
        [InlineData("files/file1.txt", 0, 1)]
        [InlineData("file/file1.txt", -2, 0)]
        [InlineData("files/file01.txt", -1, 0)]
        [InlineData("files/files1.map", -1, 1)]
        [InlineData("files/file2.map", 0, 2)]
        public void Test3(string values, int result, int retries)
        {
            prog = new ProgramB();

            var outcome = prog.PersistentFileOpen(values);
            Assert.True(outcome == result, $"For PersistentFileOpen: When trying to open a file  in persistent mode which requires an object variable: You should have returned state: {result} but did return {outcome}.");

            if (retries > 0)
            {
                if (result == 0)
                {
                    Assert.True(prog.GetOpenedFile() != null, "For PersistentFileOpen: When trying to open a file  in persistent mode: You should use the object variable openedFile to store the stream.");
                    Assert.True(prog.openedFileName == values, $"For PersistentFileOpen: When trying to open a file  in persistent mode: You should use the object variable openedFileName to store which file you opened. \n Right now you stored this name: {prog.openedFileName}");

                    outcome = prog.PersistentFileOpen(values);
                    Assert.True(outcome == 1, "For PersistentFileOpen: When trying to open a file  in persistent mode: Opening a file the 2nd time but did not get correct response!");

                }
                else
                {
                    outcome = prog.PersistentFileOpen(values);
                    Assert.True(outcome == result, $"For PersistentFileOpen: When trying to open a file  in persistent mode: You should have returned file state: {result} but did return {outcome}.");
                }
            }
            else
            {
                prog = new ProgramB();
                outcome = prog.PersistentFileOpen(values);
                Assert.True(outcome == result, $"For PersistentFileOpen: When trying to open a file  in persistent mode: Opening the file a 2nd time after closing the object once. You should have returned file state:{result} but did return {outcome}.");

            }
            try
            {
                prog.PersistentFileClose(values);
            }
            catch (Exception) //this needed to be added to stop some files from remaining open after not disposing the stream correctly 
            { }

        }

        [Theory]
        [InlineData("files/file1.txt", 0)]
        [InlineData("file/file1.txt", -2)]
        [InlineData("files/file01.txt", -1)]
        [InlineData("files/files1.map", -1)]
        [InlineData("files/file3.txt", 0)]
        public void Test4(string values, int result)
        {
            prog = new ProgramB();

            var outcome = prog.PersistentFileClose(values);
            Assert.True(outcome == -1, $"For PersistentFileClose: When trying to close a file  in persistent mode: Trying to close a file which was not opened before should return -1 instead you returned: {outcome}");


            Assert.True(prog.GetOpenedFile() == null, $"For PersistentFileClose: The initial value for openedFile should be null and only set when a file is actually opened.");
            Assert.True(prog.openedFileName == string.Empty, $"For PersistentFileClose: When trying to close a file  in persistent mode: You should use the object variable openedFileName to store which file you opened. \n Right now you stored this name: {prog.openedFileName} but it should be empty.");

            outcome = prog.PersistentFileOpen(values);
            if (outcome == result)//file could be opened or not
            {
                if (result == 0)// file should now be open
                {

                    Assert.True(prog.GetOpenedFile() != null, $"For PersistentFileClose: The initial value for openedFile should not be null after opening, but set when a file is actually opened.");
                    Assert.True(prog.openedFileName == values, $"For PersistentFileClose: When opening a file in persistent mode: You should use the object variable openedFileName to store which file you opened. \n Right now you stored this name: {prog.openedFileName} but it should be empty.");

                    outcome = prog.PersistentFileClose("something weird");
                    Assert.True(outcome == -1, $"We have an open file but tried closing some other random file which did not work, state: {outcome}");

                    Assert.True(prog.GetOpenedFile() != null, $"For PersistentFileClose: We have tried closing a random file which was not the opened one. The file which was opened should still be open but was closed.");
                    Assert.True(prog.openedFileName == values, $"For PersistentFileClose: We have tried closing an random file but the currently open one should be unchanged and not reset. \n Right now you stored this name: {prog.openedFileName} but it should be {values}.");


                    outcome = prog.PersistentFileClose(values);
                    Assert.True(outcome == 0, $"We have an open file but closing it did not work, state: {outcome}");

                    Assert.True(prog.GetOpenedFile() == null, $"For PersistentFileClose: We have closed a file which should have been open so openedFile should now be null but is not cleared.");
                    Assert.True(prog.openedFileName == string.Empty, $"For PersistentFileClose: We have closed an opened File but the stored name is not reset. \n Right now you stored this name: {prog.openedFileName} but it should be empty.");


                    outcome = prog.PersistentFileClose(values);
                    Assert.True(outcome == -1, "We tried closing an open file twice but the system does not return the correct state: -1");
                }
            }
            else //file open did not work correctly
            {
                Assert.True(false, "File Opening is not working correctly! To close a persistent file it needs to be open first.");
            }
        }

        [Theory]
        [InlineData("files/file1.txt", 0, 2, "")]
        [InlineData("file/file1.txt", -2, 0, "")]
        [InlineData("files/file1.txt", 0, 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In lobortis, ligula at hendrerit facilisis, eros risus dapibus dui, et volutpat dui nibh vel nisi. Morbi gravida sapien ac odio tincidunt tristique. Praesent tristique libero tristique tincidunt varius. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. ")]
        [InlineData("files/file2.map", 0, 3, "Duis vel sagittis elit. Pellentesque et viverra nibh. Proin sed lectus justo. Aliquam volutpat laoreet nisi a placerat. Sed nulla erat, volutpat in dictum ac, pulvinar non enim. Mauris finibus lacus fermentum facilisis bibendum. Mauris dui tortor, vehicula eu libero condimentum, sodales volutpat sem. Nulla facilisi.")]
        [InlineData("files/file2.map", 0, 6, "")]
        [InlineData("files/file2.map", 0, 9, "Sed at maximus ipsum, sed faucibus risus. Aliquam ligula dui, semper in rhoncus vel, ornare a libero. Pellentesque sit amet felis ut libero aliquet tempor. Praesent lacinia metus in luctus vehicula. Donec massa quam, mattis vitae urna vel, blandit rhoncus nulla. Nunc volutpat libero sit amet risus consequat, eget fermentum justo bibendum. Donec at dignissim mauris, nec suscipit leo. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Curabitur et interdum est, porttitor tincidunt turpis. Nullam vulputate mauris in fringilla volutpat. Cras dapibus molestie libero. In eget bibendum augue, rhoncus pharetra nulla. Maecenas lorem dolor, aliquam eget suscipit ac, dignissim at lectus. ")]

        public void Test5(string values, int result, int lines, string line)
        {
            prog = new ProgramB();

            var outcome = prog.PersistentFileRead(lines);
            Assert.True(outcome.Length == 0, $"For PersistentFileRead: When trying to read a file  in persistent mode: Trying to read from a file which is not open did return {outcome.Length} lines.");

            var openCheck = prog.PersistentFileOpen(values);
            Assert.True(openCheck == result, $"For PersistentFileRead: When trying to read a file  in persistent mode: Trying to open a file for persistent access did not work.");

            if (result != 0)
                return;
            Assert.True(prog.GetOpenedFile() != null, $"For PersistentFileRead: The initial value for openedFile should not be null after opening, but set when a file is actually opened.");
            Assert.True(prog.openedFileName == values, $"For PersistentFileRead: When opening a file in persistent mode: You should use the object variable openedFileName to store which file you opened. \n Right now you stored this name: {prog.openedFileName} but it should be empty.");


            openCheck = prog.PersistentFileClose(values);
            Assert.True(openCheck == result, $"For PersistentFileRead: Trying to close a file for persistent access did not work.");
            if (result != 0)
                return;


            outcome = prog.PersistentFileRead(lines);
            Assert.True(outcome.Length == 0, $"For PersistentFileRead: Trying to read from a file which is not open did return {outcome.Length} lines.");

            prog.PersistentFileOpen(values);
            outcome = prog.PersistentFileRead(lines);
            Assert.True(outcome[lines - 1].Trim().Equals(line.Trim()), $"For PersistentFileRead: From the file, you should have returned {lines} lines where line nr.{lines - 1} is:\n {line} \nbut did return:\n {outcome[lines - 1]}.");

            Assert.True(outcome[lines - 1].Trim().Equals(line.Trim()), $"For PersistentFileRead: Reading a second time should return the same result.\nFrom the file, you should have returned {lines} lines where line nr.{lines - 1} is:\n {line} \nbut did return:\n {outcome[lines - 1]}.");

            openCheck = prog.PersistentFileClose(values.Trim());
            Assert.True(openCheck == result, $"For PersistentFileRead: Trying to close a file for persistent access did not work.");
        }
    }
}
