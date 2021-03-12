using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ParticipationStatusConfig : BaseCommonModelConfig<ParticipationStatu>
    {
        public ParticipationStatusConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.ParticipationStatus)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ParticipationStatusType)
                .WithMany(p => p.ParticipationStatus)
                .HasForeignKey(p => p.StatusId);

            HasOptional(p => p.EnrolledProgram)
                .WithMany(p => p.ParticipationStatus)
                .HasForeignKey(p => p.EnrolledProgramId);

            #endregion

            #region Properties

            ToTable("ParticipationStatus");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StatusId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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
                .IsRequired();

            Property(p => p.IsCurrent)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
