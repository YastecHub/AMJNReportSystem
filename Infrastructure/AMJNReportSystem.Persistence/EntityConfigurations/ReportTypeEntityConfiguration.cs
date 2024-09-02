using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
    internal sealed class ReportTypeEntityConfiguration : IEntityTypeConfiguration<ReportType>
    {
        public void Configure(EntityTypeBuilder<ReportType> builder)
        {

            builder.ToTable("ReportType");
          
            builder.HasKey(t => t.Id);

            
            builder.Property(t => t.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.Description)
                   .HasMaxLength(500);

            builder.Property(t => t.Year)
                   .IsRequired();

            builder.Property(t => t.ReportTag)
                   .IsRequired();


            builder.Property(t => t.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);


           

        }
    }
}
