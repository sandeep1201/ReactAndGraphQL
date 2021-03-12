using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmployabilityPlanConfig : BaseCommonModelConfig<EmployabilityPlan>
    {
        public EmployabilityPlanConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId);

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId);

            HasRequired(p => p.ParticipantEnrolledProgram)
                .WithMany(p => p.EmployabilityPlans)
                .HasForeignKey(p => p.ParticipantEnrolledProgramId);

            HasMany(p => p.SupportiveServices)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasMany(p => p.ActivitySchedules)
                .WithOptional(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasMany(p => p.EmployabilityPlanGoalBridges)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId);

            HasMany(p => p.EmployabilityPlanActivityBridges)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId);

            #endregion

            #region Properties

            ToTable("EmployabilityPlan");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.EmployabilityPlanStatusTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ParticipantEnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.BeginDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.CanSaveWithoutActivity)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CanSaveWithoutActivityDetails)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.CreatedDate)
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
                .IsRequired();

            Property(p => p.SubmitDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
