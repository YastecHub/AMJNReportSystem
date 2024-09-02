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

            
            builder.Property(t => t.QuestionId)
                .IsRequired();

            builder.Property(t => t.TextAnswer)
                .HasMaxLength(1000);

            
            builder.Property(t => t.QuestionOptionId)
                .IsRequired(false);

           
            builder.Property(t => t.Report)
                .HasMaxLength(2000); 

          
            builder.HasOne(t => t.Question)
                .WithMany() 
                .HasForeignKey(t => t.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(t => t.QuestionOption)
                .WithMany() 
                .HasForeignKey(t => t.QuestionOptionId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}



