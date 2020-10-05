using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;

namespace MG.MainLogger
{
	public static class MainLoggerConfig
	{
		public const string SINK_SELECTOR_KEY_PROPERTY_NAME = "SinkSelectorKeyProperty";

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
						path: $@"MG.MainLogger.Logs\{name}.txt",
						outputTemplate: outputTemplate
					);
					wt.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchEndpoint))
					{
						AutoRegisterTemplate = true,
						//AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
						IndexFormat = $"mg-{name}-index-{{0:yyyy.MM}}",
						EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
						RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
					});
				})
				.WriteTo.Console(
					//LogEventLevel.Error,
					outputTemplate: outputTemplate
				);
				//.WriteTo.File(
				//	path: $@"MG.MainLogger.Logs\log.txt",
				//	outputTemplate: outputTemplate
				//)
				//.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchEndpoint))
				//{
				//	AutoRegisterTemplate = true,
				//	//AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
				//	IndexFormat = "mg-log-index-{0:yyyy.MM}",
				//	EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
				//	RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
				//});
		}

		public static void ConfigureLogging(string elasticsearchEndpoint)
		{
			Log.Logger = GetConfig(elasticsearchEndpoint).CreateLogger();
			AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
		}
	}
}
