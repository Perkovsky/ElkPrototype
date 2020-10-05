using Serilog;
using Serilog.Debugging;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MG.MainLogger
{
	public static class MainLoggerConfig
	{
		public const string SINK_SELECTOR_KEY_PROPERTY_NAME = "SinkSelectorKeyProperty";
		private const string BASE_LOG_FOLDER = @"MG.MainLogger.Logs";

		public static LoggerConfiguration GetConfig(string elasticsearchEndpoint)
		{
			var outputTemplate = "{Timestamp:o} [{Level:u3}] {Message:lj}{Properties:j}{NewLine}{Exception}";

			return new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Map(SINK_SELECTOR_KEY_PROPERTY_NAME, "log", (name, wt) =>
				{
					wt.File(
						path: $@"{BASE_LOG_FOLDER}\{name}.txt",
						outputTemplate: outputTemplate
					);
					wt.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchEndpoint))
					{
						AutoRegisterTemplate = true,
						OverwriteTemplate = true,
						DetectElasticsearchVersion = true,
						//AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,

						//BufferBaseFilename = $@"{BASE_LOG_FOLDER}\buffer-",
						//SingleEventSizePostingLimit = null,
						//BufferFileSizeLimitBytes = 1000 * 1000 * 100, // 100 MB
						//BufferFileCountLimit = 50,
						////BatchPostingLimit = 1,
						////BufferBaseFilename = null,
						////BufferFileCountLimit = 1,
						////BatchAction = ElasticOpType.Create,
						////QueueSizeLimit = 1,
						////Period = TimeSpan.FromSeconds(0),
						
					    IndexFormat = $"mg-{name}-index-{{0:yyyy.MM}}",
						EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
						RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
					});
				})
				.WriteTo.Console(
					//LogEventLevel.Error,
					outputTemplate: outputTemplate
				);
		}

		public static void ConfigureLogging(string elasticsearchEndpoint)
		{
			SelfLog.Enable(File.CreateText($@"{BASE_LOG_FOLDER}\errors.txt"));
			Log.Logger = GetConfig(elasticsearchEndpoint).CreateLogger();
			AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
		}
	}
}
