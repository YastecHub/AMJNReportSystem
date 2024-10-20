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


            builder.Property(t => t.ReportSectionName)
                      .IsRequired()
                      .HasMaxLength(100);

            builder.Property(t => t.ReportSectionValue)
                   .IsRequired();

            builder.Property(t => t.Description) 
                   .HasMaxLength(500);

            builder.Property(t => t.IsActive) 
                   .IsRequired()
                   .HasDefaultValue(true);

			builder.HasQueryFilter(rs => !rs.IsDeleted);
        }
    }
}