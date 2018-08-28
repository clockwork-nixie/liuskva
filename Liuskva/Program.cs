using System;
using Liuskva.Utilities;
using JetBrains.Annotations;
using NLog;
using Liuskva.OedApi;

namespace Liuskva
{

    public class Program : IBootstrap
    {
        // ReSharper disable once AssignNullToNotNullAttribute
        [NotNull] private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        [NotNull] public static IFactory Factory { get; set; } = DependencyInjection.CreateFactory();


        public static void Main(params string[] args)
        {
            try
            {
                Factory.GetInstance<IBootstrap>().Run(args ?? new string[0]);
            }
            catch (Exception exception)
            {
                _logger.Fatal(exception, "Unhandled top-level exception.");
                Console.WriteLine($"Unhandled top-level exception: {exception.Message}");
            }
        }


        public void Run(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ApplicationException("Two argument are required.");
            }
            else switch (args[0])
            {
                case "entries":
                    Console.WriteLine(Factory.GetInstance<IOedClient>().FetchEntries(args[1]));
                    break;

                case "lemma":
                    Console.WriteLine(Factory.GetInstance<IOedClient>().FetchDesignation(args[1]));
                    break;

                default:
                    Console.WriteLine("Unknown directive.");
                    break;
            }
        }
    }
}
