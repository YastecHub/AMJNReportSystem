﻿using AMJNReportSystem.Domain.Entities;
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

            builder.Property(e => e.ReportTypeId)
                .IsRequired();

            builder.Property(e => e.JammatEmailAddress)
                .IsRequired();

            builder.HasOne(e => e.ReportType)
                .WithMany()
                .HasForeignKey(e => e.ReportTypeId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(e => e.SubmissionWindow)
                .WithMany()
                .HasForeignKey(e => e.SubmissionWindowId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Answers)
                .WithOne()
                .HasForeignKey("ReportSubmissionId")
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(e => e.ReportSubmissionStatus)
                .IsRequired();
			builder.HasQueryFilter(sw => !sw.IsDeleted);

			builder.Property(e => e.ReportTag)
                .IsRequired();
        }
    }

}
