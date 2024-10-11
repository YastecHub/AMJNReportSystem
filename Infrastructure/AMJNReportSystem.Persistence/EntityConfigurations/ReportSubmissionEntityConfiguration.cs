using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
    internal sealed class ReportSubmissionConfiguration : IEntityTypeConfiguration<ReportSubmission>
    {
        public void Configure(EntityTypeBuilder<ReportSubmission> builder)
        {
            builder.HasKey(e => e.Id); 

            builder.Property(e => e.JamaatId)
                .IsRequired();

            builder.Property(e => e.JammatEmailAddress)
                .IsRequired();

            builder.Property(e => e.ReportSubmissionStatus)
                .IsRequired();
			builder.HasQueryFilter(sw => !sw.IsDeleted);

			builder.Property(e => e.ReportTag)
                .IsRequired();
        }
    }

}
