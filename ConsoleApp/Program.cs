﻿using MG.Logger;
using System;

namespace ConsoleApp
{
	public enum EntityType
	{
		AuditLog, TransactionLog, JobLog
	}

	public class Payload
	{
		public int EntityId { get; set; }
		public EntityType EntityType { get; set; }

		public Payload(int entityId, EntityType EntityType)
		{
			EntityId = entityId;
			EntityType = EntityType;
		}
	}

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
				_logger.Info(LogType.Library, "Test logger");
				//_logger.Error(ex, "An unknown error occurred on the Index action of the HomeController {type}", LogType.WindowService);
				//_logger.Info(LogType.Library, "Test logger ID: {Id}", 345688);
				//_logger.Info(LogType.Library, "Test logger some property: {MySomeProperty}", "just simple text");
				//_logger.Info(LogType.WindowService, "Payload: {@Payload}", new Payload(442, EntityType.AuditLog));
				//_logger.Info(LogType.Web, "Payload: {@Payload}", new { Fistname = "Ivan", Lastname = "Dukin", Age = 13 });
				//_logger.Info(LogType.WindowService, "Test logger without property value");
				//_logger.Info(LogType.Web, "Payload: {@Payload}", 3432);
				//_logger.Info(LogType.Web, "Payload: {Payload}", 111);
				//_logger.Info(LogType.Web, "Payload: {@Payload2}", new { TestInt = 12, Message = new { Data = "This is test" } });
				//_logger.Info(LogType.Web, "Payload: {@Payload}", new { Message = 8881 });
			}

			Console.WriteLine("Done!");
		}
	}
}
