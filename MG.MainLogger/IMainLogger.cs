using MG.MainLogger.Models;
using System;

namespace MG.MainLogger
{
	public interface IMainLogger
	{
		IMainLogger AddField(string name, string value);

		IMainLogger AddSource<T>()
		   where T : class;

		void Error(string message);

		void Error(Exception exception, string message);

		void Fatal(string message);

		void Fatal(Exception exception, string message);

		void Information(string message);

		void Information<T>(T structuredLogging)
			where T : class, IStructuredLogging;
	}
}
