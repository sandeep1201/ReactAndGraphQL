using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CourtDateConfig : BaseCommonModelConfig<CourtDate>
    {
        public CourtDateConfig()
        {
            #region Relationships

            HasOptional(p => p.LegalIssuesSection)
                .WithMany(p => p.CourtDates)
                .HasForeignKey(p => p.LegalSectionId);

            #endregion

            #region Properties

            ToTable("CourtDate");

            Property(p => p.LegalSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Location)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.Date)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
