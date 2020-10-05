﻿using System;

namespace MG.MainLogger.Models
{
	public class SystemLogStructuredLogging : IStructuredLogging
	{
		public DateTime Created { get; set; }
		public string Type { get; set; }
		public string Component { get; set; }
		public string Description { get; set; }
		public int? EntityId { get; set; }
	}
}