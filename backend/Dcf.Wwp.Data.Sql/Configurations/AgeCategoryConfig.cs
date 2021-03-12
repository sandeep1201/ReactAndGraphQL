using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AgeCategoryConfig : EntityTypeConfiguration<AgeCategory>
    {
        public AgeCategoryConfig()
        {
            #region Relationships

            HasMany(p => p.ChildYouthSectionChilds)
                .WithOptional(p => p.AgeCategory)
                .HasForeignKey(p => p.AgeCategoryId);

            #endregion

            #region Properties

            ToTable("AgeCategory");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AgeRange)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsOptional();

            Property(p => p.DescriptionText)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
