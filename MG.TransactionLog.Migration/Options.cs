using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace MG.TransactionLog.Migration
{
	public class Options
	{
		[Option('i', "id", Required = false, HelpText = "Set start TransactionLog ID.")]
		public int StartTransactionLogId { get; set; }

		[Option('r', "rows", Required = false, HelpText = "Set count of rows to be uploaded.")]
		public int RowsCount { get; set; }

		[Option('s', "skip", Required = false, HelpText = "Skip rows.")]
		public int Skip { get; set; }

		[Usage(ApplicationAlias = "MG.TransactionLog.Migration.exe")]
		public static IEnumerable<Example> Examples
		{
			get
			{
				yield return new Example("normal scenario", "");
				yield return new Example("or with args", UnParserSettings.WithGroupSwitchesOnly(), new Options { StartTransactionLogId = 1234, RowsCount = 100000, Skip = 1000 });
			}
		}
	}
}
