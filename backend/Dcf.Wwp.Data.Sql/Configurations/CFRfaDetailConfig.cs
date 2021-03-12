using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CFRfaDetailConfig : BaseCommonModelConfig<CFRfaDetail>
    {
        public CFRfaDetailConfig()
        {
            #region Relationships

            HasOptional(p => p.RequestForAssistance)
                .WithMany(p => p.CFRfaDetails)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.CourtOrderedCountyId);

            #endregion

            #region Properties

            ToTable("CFRfaDetails");

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CourtOrderedCountyId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CourtOrderEffectiveDate)
                .HasColumnType("date")
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
