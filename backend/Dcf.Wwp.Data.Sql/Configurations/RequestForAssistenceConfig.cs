using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RequestForAssistanceConfig : BaseCommonModelConfig<RequestForAssistance>
    {
        public RequestForAssistanceConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.RequestsForAssistance)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.RequestForAssistanceStatus)
                .WithMany(p => p.RequestsForAssistance)
                .HasForeignKey(p => p.RequestForAssistanceStatusId);

            HasRequired(p => p.EnrolledProgram)
                .WithMany(p => p.RequestForAssistances)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasOptional(p => p.CountyOfResidence)
                .WithMany()
                .HasForeignKey(p => p.CountyOfResidenceId);

            HasOptional(p => p.Office)
                .WithMany(p => p.RequestForAssistances)
                .HasForeignKey(p => p.OfficeId);

            HasMany(p => p.RequestForAssistanceChilds)
                .WithRequired(p => p.RequestForAssistance)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasMany(p => p.FCDPRfaDetails)
                .WithOptional(p => p.RequestForAssistance)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasMany(p => p.CFRfaDetails)
                .WithOptional(p => p.RequestForAssistance)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasMany(p => p.TJTMJRfaDetails)
                .WithOptional(p => p.RequestForAssistance)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasMany(p => p.RequestForAssistanceRuleReasons)
                .WithRequired(p => p.RequestForAssistance)
                .HasForeignKey(p => p.RequestForAssistanceId);

            #endregion

            #region Properties

            ToTable("RequestForAssistance");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RequestForAssistanceStatusId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RequestForAssistanceStatusDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.RfaNumber)
                .HasColumnType("decimal")
                .HasPrecision(12, 0)
                .IsOptional();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CountyOfResidenceId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OfficeId)
                .HasColumnType("int")
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
