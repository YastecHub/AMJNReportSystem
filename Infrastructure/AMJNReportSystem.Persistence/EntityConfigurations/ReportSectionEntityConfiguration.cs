using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
    internal sealed class ReportSectionEntityConfiguration : IEntityTypeConfiguration<ReportSection>
    {
        public void Configure(EntityTypeBuilder<ReportSection> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasIndex(u => u.SectionName).IsUnique();
            builder.HasIndex(u => u.SectionValue).IsUnique();
        }
    } 
}
