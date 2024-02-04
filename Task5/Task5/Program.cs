namespace Task5
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var input = args.FirstOrDefault(a => a.StartsWith("--input="))?.Split('=')[1];

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Missing input parameter.");
                return 1;
            }

            try
            {
                var result = ProcessInput(input);
                Console.WriteLine(string.Join(", ", result));
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        static List<int> ProcessInput(string input)
        {
            var inputList = input
                .Trim('[', ']')
                .Split(",")
                .Select(s => int.Parse(s.Trim()))
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            return inputList;
        }
    }
}
