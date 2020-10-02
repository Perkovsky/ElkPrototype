using MG.TransactionLog.Migration.DAL.Models;

namespace MG.TransactionLog.Migration.Extensions
{
	public static class TransactionLogExtensions
	{
		public static TransactionLogPayload Map(this DAL.Models.TransactionLog target)
		{
			return new TransactionLogPayload
			{
				AchFileId = target.AchFileId,
				BatchId = target.BatchId,
				CreateDate = target.CreateDate,
				DistributionLineId = target.DistributionLineId,
				Message = target.Message,
				TenantTransactionId = target.TenantTransactionId
			};
		}
	}
}
