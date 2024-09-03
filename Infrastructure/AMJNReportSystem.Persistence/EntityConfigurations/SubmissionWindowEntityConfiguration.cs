using AMJNReportSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Persistence.EntityConfigurations
{
	internal sealed class SubmissionWindowConfiguration : IEntityTypeConfiguration<SubmissionWindow>
	{
		public void Configure(EntityTypeBuilder<SubmissionWindow> builder)
		{
			builder.ToTable("SubmissionWindows");

			builder.HasKey(sw => sw.Id);

			builder.Property(sw => sw.StartingDate)
				   .IsRequired();

			builder.Property(sw => sw.EndingDate)
				   .IsRequired();

			builder.Property(sw => sw.Month)
				   .IsRequired();

			builder.Property(sw => sw.Year)
				   .IsRequired();

			builder.Property(sw => sw.IsLocked)
				   .IsRequired();

			builder.HasOne(sw => sw.ReportType)
				   .WithMany()
				   .HasForeignKey(sw => sw.ReportTypeId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasQueryFilter(sw => !sw.IsDeleted);
		}
	}

}
