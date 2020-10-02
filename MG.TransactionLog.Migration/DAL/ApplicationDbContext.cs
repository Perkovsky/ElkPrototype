using MG.TransactionLog.Migration.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MG.TransactionLog.Migration.DAL
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Models.TransactionLog> TransactionLogs { get; set; }
		public DbSet<Settings> Settings { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			//Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			foreach (var entity in builder.Model.GetEntityTypes())
			{
				entity.SetTableName(entity.ClrType.Name);
			}
		}
	}
}
