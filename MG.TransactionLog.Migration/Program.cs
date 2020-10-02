using CommandLine;
using MG.TransactionLog.Migration.DAL;
using MG.TransactionLog.Migration.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MG.TransactionLog.Migration
{
	class Program
	{
		private static LoggerConfiguration GetLoggerConfiguration(string elasticsearchEndpoint)
		{
			var outputTemplate = "{Timestamp:o} [{Level:u3}] {Message}{NewLine}{Exception}";

			return new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Console(
					outputTemplate: outputTemplate
				)
				//.WriteTo.File(
				//	path: @"MG.Logger.Logs\log.txt",
				//	outputTemplate: outputTemplate
				//)
				.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchEndpoint))
				{
					AutoRegisterTemplate = true,
					//AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
					IndexFormat = "mg-logger-index-{0:yyyy.MM}",
					EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
					RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway
				});
		}

		private static void Execute(int startId, int rowsCount = 100000)
		{
			try
			{
				rowsCount = rowsCount > 0 ? rowsCount : 100000;
				Console.OutputEncoding = Encoding.UTF8;
				var sw = new Stopwatch();
				sw.Start();

				IConfigurationRoot config = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.Build();

				int totalFiles;
				int uploadedRows = 0;
				int errors = 0;
				int lastFileId = 0;
				int skipItems = 0;
				int totalCurrentCount = 0;

				var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
				var options = optionsBuilder.UseSqlServer(config.GetConnectionString("mainDb")).Options;
				var elasticsearchEndpoint = config["Elasticsearch:endpoint"];

				// totalFiles
				using (ApplicationDbContext context = new ApplicationDbContext(options))
				{
					totalFiles = (context.TransactionLogs.ToList() as ICollection<DAL.Models.TransactionLog>).Count;
				}

				#region For Testing

				//totalFiles = 3;
				//rowsCount = 3;

				#endregion

				using (LogContext.PushProperty("Sourse", "Migration"))
				using (var log = GetLoggerConfiguration(elasticsearchEndpoint).CreateLogger())
				{
					while (totalFiles > skipItems)
					{
						using (ApplicationDbContext context = new ApplicationDbContext(options))
						{
							var rows = context.TransactionLogs.Where(x => x.Id > startId)
								.Skip(skipItems)
								.Take(rowsCount);

							skipItems += rowsCount;
							int partCount = 0;

							foreach (var row in rows)
							{
								partCount++;
								try
								{
									//Console.WriteLine($"ADDITING - ID:{row.Id}, CreateDate:{row.CreateDate}, TenantTransactionId:{row.TenantTransactionId}");
									log.Information(row.CreateDate, "{@Payload}", row.Map());
									lastFileId = row.Id;
									uploadedRows++;
								}
								catch (Exception ex)
								{
									errors++;
									Console.WriteLine(ex.Message);
									Console.WriteLine($"\tERROR - ID:{row.Id}, CreateDate:{row.CreateDate}, TenantTransactionId:{row.TenantTransactionId}");
								}
							}
							totalCurrentCount += partCount;
							Console.WriteLine($"--- Part count: {partCount}; Total current count: {totalCurrentCount}");
						}
					}
				}

				sw.Stop();
				Console.WriteLine(new string('-', 20));
				Console.WriteLine($"Spent time : {sw.Elapsed}");
				Console.WriteLine($"Start File ID : {startId}");
				Console.WriteLine($"Last File ID : {lastFileId}");
				Console.WriteLine($"Total file(s) in DB : {totalFiles}");
				Console.WriteLine($"Rows count : {(rowsCount == 0 ? totalFiles : rowsCount)}");
				Console.WriteLine($"Uploaded row(s) to Elasticsearch : {uploadedRows}");
				Console.WriteLine($"Errors : {errors}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		static void Main(string[] args)
		{
			try
			{
				Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
				{
					Execute(o.StartTransactionLogId, o.RowsCount);
				});
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
