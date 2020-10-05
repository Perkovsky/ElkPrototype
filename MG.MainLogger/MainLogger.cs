using MG.MainLogger.Extensions;
using MG.MainLogger.Formatter;
using MG.MainLogger.Models;
using Serilog;
using System;

namespace MG.MainLogger
{
	public class MainLogger : IMainLogger
	{
		private readonly ILogger _logger;
		private readonly INameFormatter _formatter;

		public MainLogger(ILogger logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_formatter = NameFormatter.Create("-");
		}

		public IMainLogger AddField(string name, string value)
		{
			return new MainLogger(_logger.ForContext(name, value));
		}

		public IMainLogger AddSource<T>()
			where T : class
		{
			return new MainLogger(_logger.ForContext<T>());
		}

		public void Error(string message)
		{
			_logger.Error(message);
		}

		public void Error(Exception exception, string message)
		{
			_logger.Error(exception, message);
		}

		public void Fatal(string message)
		{
			_logger.Fatal(message);
		}

		public void Fatal(Exception exception, string message)
		{
			_logger.Fatal(exception, message);
		}

		public void Information(string message)
		{
			_logger.Information(message);
		}

		public void Information<T>(T structuredLogging)
			where T : class, IStructuredLogging
		{
			var structuredLoggingTypeName = typeof(T).Name.SanitizeStructuredLoggingName();
			_logger.ForContext(structuredLoggingTypeName, structuredLogging, true)
				// for index/file segment name, see serilog map
				.Information($"{{{MainLoggerConfig.SINK_SELECTOR_KEY_PROPERTY_NAME}}}", _formatter.SanitizeName(structuredLoggingTypeName));
		}
	}
}
