using Serilog;
using Serilog.Context;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;

namespace MG.Logger
{
	public class Logger
	{
		//private readonly LoggerConfiguration _config;
		private readonly string _elasticsearchEndpoint;

		public Logger()
		{
			_elasticsearchEndpoint = "http://localhost:9200";
			Log.Logger = GetLoggerConfiguration(_elasticsearchEndpoint).CreateLogger();
		}

		#region Private Methods

		private LoggerConfiguration GetLoggerConfiguration(string elasticsearchEndpoint)
		{
			var outputTemplate = "{Timestamp:o} [{Level:u3}] {Message}{NewLine}{Exception}";

			return new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Console(
					outputTemplate: outputTemplate
				)
				.WriteTo.File(
					path: @"MG.Logger.Logs\log.txt",
					outputTemplate: outputTemplate
				)
				.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchEndpoint))
				{
					AutoRegisterTemplate = true,
					//AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
					IndexFormat = "mg-log-index-{0:yyyy.MM}",
					EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
					RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
				});
		}

		#endregion

		public void Info(LogType type, string messageTemplate)
		{
			using (LogContext.PushProperty("LogType", type.ToString()))
			using (var log = GetLoggerConfiguration(_elasticsearchEndpoint).CreateLogger())
			{
				log.Information(messageTemplate);
			}
		}

		public void Info<T>(LogType type, string messageTemplate, T propertyValue)
		{
			using (LogContext.PushProperty("LogType", type.ToString()))
			using (var log = GetLoggerConfiguration(_elasticsearchEndpoint).CreateLogger())
			{
				log.Information<T>(messageTemplate, propertyValue);
			}
		}

		public void Error<T>(string messageTemplate, T propertyValue)
		{
			using (var log = GetLoggerConfiguration(_elasticsearchEndpoint).CreateLogger())
			{
				log.Error<T>(messageTemplate, propertyValue);
			}
		}

		public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
		{
			using (var log = GetLoggerConfiguration(_elasticsearchEndpoint).CreateLogger())
			{
				log.Error<T>(exception, messageTemplate, propertyValue);
			}
		}
	}
}
