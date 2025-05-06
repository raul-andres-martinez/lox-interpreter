namespace Lox.Models
{
    public static class ErrorReporter
    {
        public static bool HadError { get; private set; } = false;

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        public static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            HadError = true;
        }

        public static void Reset()
        {
            HadError = false;
        }
    }
}