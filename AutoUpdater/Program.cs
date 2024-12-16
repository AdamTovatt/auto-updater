namespace AutoUpdater
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Dictionary<string, string> arguments = RuntimeArgumentsReader.CreateDictionary(args);

            if (!arguments.ContainsKey("script-path"))
                Console.WriteLine("Missing --script-path run time argument");
            if (!arguments.ContainsKey("logging"))
                Console.WriteLine("Missing --logging run time argument");

            bool logging;
            if (!bool.TryParse(arguments["logging"], out logging))
                Console.WriteLine("Invalid --logging run time argument, should be \"true\" or \"false\". Logging will be enabled by default unless changed.");

            ConsoleLogger logger = new ConsoleLogger(logging);
            Script script = Script.CreateFromFile(arguments["script-path"]);

            while (true)
            {
                try
                {
                    await script.RunAsync(commandLogger: logger);

                    logger.Log("Completed running script.");
                }
                catch (Exception exception)
                {
                    logger.Log($"Exception at top level: {exception}");
                }

                await Task.Delay(60 * 1000); // one minute
            }
        }
    }
}
