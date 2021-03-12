using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EnrolledProgramConfig : BaseCommonModelConfig<EnrolledProgram>
    {
        public EnrolledProgramConfig()
        {
            #region Relationships

            HasMany(p => p.ContractAreas)
                .WithOptional(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.EnrolledProgramOrganizationPopulationTypeBridges)
                .WithRequired(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.CompletionReasons)
                .WithRequired(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.GoalTypes)
                .WithRequired(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.ParticipantEnrolledPrograms)
                .WithOptional(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.EmployabilityPlans)
                .WithRequired(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.ParticipationStatus)
                .WithOptional(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId);

            #endregion

            #region Properties

            ToTable("EnrolledProgram");

            Property(p => p.ProgramCode)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.SubProgramCode)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.ProgramType)
                .HasColumnType("char")
                .HasMaxLength(20)
                .IsOptional();

            Property(p => p.DescriptionText)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ShortName)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

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
