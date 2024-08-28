using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
    internal sealed class ReportResponseEntityConfiguration : IEntityTypeConfiguration<ReportResponse>
    {
        public void Configure(EntityTypeBuilder<ReportResponse> builder)
        {
            builder.HasKey(t => t.Id);

           
        }
    }
}
