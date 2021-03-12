using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkProgramSectionConfig : BaseCommonModelConfig<WorkProgramSection>
    {
        public WorkProgramSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.WorkProgramSections)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.InvolvedWorkPrograms)
                .WithOptional(p => p.WorkProgramSection)
                .HasForeignKey(p => p.WorkProgramSectionId);

            #endregion

            #region Properties

            ToTable("WorkProgramSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsInOtherPrograms)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
