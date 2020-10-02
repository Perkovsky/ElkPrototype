using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MG.TransactionLog.Migration.DAL.Models
{
	public class TransactionLog
	{
		[Key]
		public int Id { get; set; }

		[StringLength(7)]
		public string BatchId { get; set; }

		[StringLength(512)]
		[Required(AllowEmptyStrings = true)]
		public string Message { get; set; }

		public DateTime CreateDate { get; set; }

		[Column("AchFile")]
		public int? AchFileId { get; set; }

		[Column("DistributionLine")]
		public int? DistributionLineId { get; set; }

		[Column("TenantTransaction")]
		public int? TenantTransactionId { get; set; }
	}
}
