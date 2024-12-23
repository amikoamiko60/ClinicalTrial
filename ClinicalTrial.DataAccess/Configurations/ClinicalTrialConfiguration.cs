using ClinicalTrial.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalTrial.DataAccess.Configurations
{
    internal sealed class ClinicalTrialConfiguration : IEntityTypeConfiguration<ClinicalTrialEntity>
    {
        public void Configure(EntityTypeBuilder<ClinicalTrialEntity> builder)
        {
            builder.ToTable("ClinicalTrial");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.TrialId)
              .IsRequired()
              .HasMaxLength(255);

            builder.Property(a => a.Title)
             .IsRequired()
             .HasMaxLength(255);

            builder.Property(a => a.StartDate)
               .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        }
    }
}
