using System;

namespace MG.TransactionLog.Migration.DAL.Models
{
	public class TransactionLogPayload
	{
		public string BatchId { get; set; }
		public string Message { get; set; }
		public DateTime CreateDate { get; set; }
		public int? AchFileId { get; set; }
		public int? DistributionLineId { get; set; }
		public int? TenantTransactionId { get; set; }
	}
}
