using System;
using System.Threading.Tasks;

namespace OrderImportQuiz
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Invalid command line arguments.");
                Console.WriteLine("Available parameters: full, import, clean, check");
            }
            else
            {
                var manager = new Manager(args);
                await ParseArgument(args[0]);

                async Task ParseArgument(string parameterName)
                {
                    switch (parameterName)
                    {
                        case "full":
                        {
                            await ParseArgument("clean");
                            await ParseArgument("import");
                            await ParseArgument("check");
                            break;
                        }
                        case "import":
                        {
                            if (args.Length < 3)
                            {
                                Console.WriteLine("Usage: import <customerFile> <orderFile>");
                            }
                            else await manager.ImportFromFile(args[1], args[2]);
                            break;
                        }
                        case "clean":
                        {
                            await manager.Purge();
                            break;
                        }
                        case "check":
                        {
                            await manager.Check();
                            break;
                        }
                        default:
                        {
                            Console.WriteLine($"Unknown argument {args[0]}.");
                            break;
                        }
                    }
                }
            }
        }
    }
}