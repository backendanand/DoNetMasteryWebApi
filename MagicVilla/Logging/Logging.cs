namespace MagicVilla.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            switch (type)
            {
                case "error":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error : " + message);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "info":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Info : " + message); 
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "warning":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Warn : " + message);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
