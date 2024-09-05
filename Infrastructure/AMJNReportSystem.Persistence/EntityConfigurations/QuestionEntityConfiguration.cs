using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMJNReportSystem.Domain.EntityConfigurations
{
	internal sealed class QuestionEntityConfiguration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{
			builder.ToTable("Questions");

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

			builder.Property(q => q.SectionId)
				   .IsRequired();

			// Configure the relationship with ReportSection
			builder.HasOne(q => q.ReportSection)
				   .WithMany()  // Ensure to specify the collection property if it exists
				   .HasForeignKey(q => q.SectionId)
				   .OnDelete(DeleteBehavior.Restrict);  // Optional: change to `SetNull` or `NoAction` if necessary

			// Configure the relationship with QuestionOption
			builder.HasMany(q => q.Options)
				   .WithOne(o => o.Question)
				   .HasForeignKey(o => o.QuestionId)
				   .OnDelete(DeleteBehavior.Cascade);

			// Define global query filter
			builder.HasQueryFilter(q => !q.IsDeleted);

			// Optional: Define additional query filters for related entities if needed
		}
	}

}


