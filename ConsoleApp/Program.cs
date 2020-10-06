using MG.MainLogger;
using MG.MainLogger.Models;
using Serilog;
using SimpleInjector;
using System;
using System.Diagnostics;

namespace ConsoleApp
{
	class Program
	{
		static readonly Container _container;

		static Program()
		{
			var elasticsearchEndpoint = "http://localhost:9200";
			MainLoggerConfig.ConfigureLogging(elasticsearchEndpoint);

			_container = new Container();
			_container.RegisterSingleton<ILogger>(() => Log.Logger);
			_container.RegisterSingleton<IMainLogger, MainLogger>();
			_container.Verify();
		}

		static void Main(string[] args)
		{
			var sw = new Stopwatch();
			sw.Start();

			var _logger = _container.GetInstance<IMainLogger>();

			for (int i = 0; i < 20; i++)
			{
				_logger.Information(i.ToString());
			}

			_logger.AddSource<Program>()
				.Information(new SystemLogStructuredLogging
				{
					EntityId = 25,
					Component = "ConloseApp",
					Created = DateTime.UtcNow,
					Description = "System Log Test",
					Type = "SomeLogType"
				});

			_logger
				.AddSource<Program>()
				.AddField("EntityId", "444")
				.AddField("EntityType", "User")
				.Information(new TransactionLogStructuredLogging
				{
					CreateDate = DateTime.UtcNow,
					TenantTransactionId = 344,
					Message = "Transaction Log Test #1"
				});

			_logger
				.AddField("EntityId", "555")
				.AddField("EntityType", "User")
				.Information(new TransactionLogStructuredLogging
				{
					CreateDate = DateTime.UtcNow,
					TenantTransactionId = 2124,
					Message = "Transaction Log Test #2"
				});

			_logger.AddSource<Program>()
				.Information("Test source");

			try
			{
				throw new Exception("Some bad code was executed");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error message...");
				_logger.Error(ex.Message);
				_logger.Fatal(ex, "Fatal message...");
				_logger.Fatal(ex.Message);
				_logger.Information("Test logger");
			}

			sw.Stop();
			Console.WriteLine("Done!");
			Console.WriteLine(new string('-', 20));
			Console.WriteLine($"Spent time : {sw.Elapsed}");

			//Log.CloseAndFlush();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}
	}
}
