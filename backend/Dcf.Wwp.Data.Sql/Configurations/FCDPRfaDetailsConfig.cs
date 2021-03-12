using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FCDPRfaDetailsConfig : BaseCommonModelConfig<FCDPRfaDetail>
    {
        public FCDPRfaDetailsConfig()
        {
            #region Relationships

            HasOptional(p => p.RequestForAssistance)
                .WithMany(p => p.FCDPRfaDetails)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.CourtOrderedCountyId);

            #endregion

            #region Properties

            ToTable("FCDPRfaDetails");

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsVoluntary)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CourtOrderedCountyId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CourtOrderEffectiveDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.KIDSPinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
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

            Property(p => p.ReferralSource)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsOptional();

            #endregion
        }
    }
}
