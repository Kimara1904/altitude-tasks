namespace Task5Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Main_ValidInput_ShouldPrintSortedAndDeduplicatedList()
        {
            // Arrange
            var input = "--input=[3, 1, 4, 1, 5, 9, 2, 6, 5]";
            var expectedOutput = "1, 2, 3, 4, 5, 6, 9";

            // Capture console output
            _ = Console.Out;
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            Task5.Program.Main(new string[] { input });

            // Assert
            StringAssert.Contains(consoleOutput.ToString().Trim(), expectedOutput);
        }

        [TestMethod]
        public void Test_Main_MissingInput_ShouldPrintErrorMessage()
        {
            // Arrange
            var input = "";

            // Capture console output
            _ = Console.Out;
            using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            Task5.Program.Main(new string[] { input });

            // Assert
            StringAssert.Contains(consoleOutput.ToString().Trim(), "Missing input parameter.");
        }
    }
}