using Lox.Models;
using System.Text;

namespace Lox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: clox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                Repl();
            }
        }

        private static void RunFile(string path)
        {
            string source = File.ReadAllText(path, Encoding.UTF8);
            Run(source);

            if (ErrorReporter.HadError)
            {
                Environment.Exit(65);
            }
        }

        private static void Repl()
        {
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }
                Run(line);
                ErrorReporter.Reset();
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}