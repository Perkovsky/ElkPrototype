using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace MG.Logger
{
	public class Logger
	{
		private readonly IConfigurationRoot _config;

		public Logger()
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			_config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment}.json", optional: true)
				.Build();
		}

		#region Private Methods

		//private static void ConfigureLogging()
		//{
		//	var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		//	var configuration = new ConfigurationBuilder()
		//		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		//		.AddJsonFile($"appsettings.{environment}.json", optional: true)
		//		.Build();

		//	Log.Logger = new LoggerConfiguration()
		//		.ReadFrom.Configuration(configuration)
		//		.CreateLogger();

		//}

		#endregion

		public void Info(LogType type, string messageTemplate)
		{
			using (LogContext.PushProperty("LogType", type.ToString()))
			using (var log = new LoggerConfiguration()
				.ReadFrom.Configuration(_config)
				.CreateLogger())
			{
				log.Information(messageTemplate);
			}
		}

		public void Info<T>(LogType type, string messageTemplate, T propertyValue)
		{
			using (LogContext.PushProperty("LogType", type.ToString()))
			using (var log = new LoggerConfiguration()
				.ReadFrom.Configuration(_config)
				.CreateLogger())
			{
				log.Information<string>(messageTemplate, propertyValue.ToString());
			}
		}

		public void Error<T>(string messageTemplate, T propertyValue)
		{
			using (var log = new LoggerConfiguration()
				.ReadFrom.Configuration(_config)
				.CreateLogger())
			{
				log.Error<string>(messageTemplate, propertyValue.ToString());
			}
		}

		public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
		{
			using (var log = new LoggerConfiguration()
				.ReadFrom.Configuration(_config)
				.CreateLogger())
			{
				log.Error<string>(exception, messageTemplate, propertyValue.ToString());
			}
		}
	}
}
