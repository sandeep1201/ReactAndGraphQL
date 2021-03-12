using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PEPOtherInformationConfig : BaseCommonModelConfig<PEPOtherInformation>
    {
        public PEPOtherInformationConfig()
        {
            #region Relationships

            HasOptional(p => p.ParticipantEnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.PEPId);

            #endregion

            #region Properties

            ToTable("PEPOtherInformation");

            Property(p => p.PEPId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CompletionReasonDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
