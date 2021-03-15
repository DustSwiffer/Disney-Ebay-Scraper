using System;
namespace DisneyBroker.Utilities
{
    public static class ConsoleBannerWriter
    {
        public static void Write()
        {
            ConsoleColor foregroundConsoleColorAtStart = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@" ____________________________________________________________________________________________________________");
            Console.WriteLine(@"/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"                         ____  _                          ____             __            ");
            Console.WriteLine(@"                        / __ \(_)________  ___  __  __   / __ )_________  / /_____  _____");
            Console.WriteLine(@"                       / / / / / ___/ __ \/ _ \/ / / /  / __  / ___/ __ \/ //_/ _ \/ ___/");
            Console.WriteLine(@"                      / /_/ / (__  ) / / /  __/ /_/ /  / /_/ / /  / /_/ / ,< /  __/ /    ");
            Console.WriteLine(@"                     /_____/_/____/_/ /_/\___/\__, /  /_____/_/   \____/_/|_|\___/_/     ");
            Console.WriteLine(@"                                             /____/                                   ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@" ____________________________________________________________________________________________________________");
            Console.WriteLine(@"/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Webscraping eBay to get the estimate value of the Disney product");

            Console.ForegroundColor = foregroundConsoleColorAtStart;
        }
    }
}
