using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
    internal sealed class ReportSubmissionEntityConfiguration : IEntityTypeConfiguration<ReportSubmission>
    {
        public void Configure(EntityTypeBuilder<ReportSubmission> builder)
        {
            builder.HasKey(t => t.Id);

          
        }
    }
}
