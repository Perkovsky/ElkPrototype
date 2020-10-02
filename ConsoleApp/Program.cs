using MG.Logger;
using System;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var _logger = new Logger();

			try
			{
				throw new Exception("Some bad code was executed");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "An unknown error occurred on the Index action of the HomeController {type}", LogType.WindowService);
				_logger.Info(LogType.Library, "Test logger ID: {Id}", 345688);
				_logger.Info(LogType.WindowService, "Test logger without property value");
			}

			Console.WriteLine("Done!");
		}
	}
}
