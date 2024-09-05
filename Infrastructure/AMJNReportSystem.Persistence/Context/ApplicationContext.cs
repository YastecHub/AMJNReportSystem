using AMJNReportSystem.Persistence.Identity;
using AMJNReportSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace AMJNReportSystem.Persistence.Context
{
	public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder); 
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

		public DbSet<ReportType> ReportTypes { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<QuestionOption> QuestionOptions { get; set; }
		public DbSet<ReportSubmission> ReportSubmissions { get; set; }
		public DbSet<SubmissionWindow> SubmissionWindows { get; set; }
		public DbSet<ReportSection> ReportSections { get; set; }
		public DbSet<ReportResponse> ReportResponses { get; set; }
	}

}

