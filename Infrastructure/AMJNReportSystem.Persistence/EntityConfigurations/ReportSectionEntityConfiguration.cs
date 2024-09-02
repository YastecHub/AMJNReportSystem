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

            builder.HasIndex(u => u.ReportSectionName).IsUnique();
            builder.HasIndex(u => u.ReportSectionValue).IsUnique();
            builder.HasIndex(u => u.Description).IsUnique();


            builder.HasOne<ReportType>()
                  .WithMany()
                  .HasForeignKey(t => t.ReportTypeId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    } 
}
