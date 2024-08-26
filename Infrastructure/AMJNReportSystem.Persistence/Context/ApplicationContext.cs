using AMJNReportSystem.Persistence.Identity;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Context
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDataSection> ReportDataSections { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ReportTypeSection> ReportTypeSections { get; set; }
        public DbSet<ReportSubmission> ReportSubmissions { get; set; }
        public DbSet<SubmissionWindow> SubmissionWindows { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
    }
}
