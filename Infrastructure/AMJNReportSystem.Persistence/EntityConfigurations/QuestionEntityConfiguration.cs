using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
	internal sealed class QuestionEntityConfiguration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{

			builder.HasKey(q => q.Id);

			builder.Property(q => q.QuestionName)
				   .IsRequired()
				   .HasMaxLength(700);

			builder.Property(q => q.QuestionType)
				   .IsRequired();

			builder.Property(q => q.ResponseType)
				   .IsRequired();

			builder.Property(q => q.IsRequired)
				   .IsRequired();

			builder.Property(q => q.IsActive)
				   .IsRequired();

			builder.Property(q => q.ReportSectionId)
				   .IsRequired();

			

			// Define global query filter
			builder.HasQueryFilter(q => !q.IsDeleted);
		}
	}

}


