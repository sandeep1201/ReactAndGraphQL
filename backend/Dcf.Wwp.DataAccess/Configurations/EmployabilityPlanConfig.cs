using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmployabilityPlanConfig : BaseConfig<EmployabilityPlan>
    {
        public EmployabilityPlanConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.EmployabilityPlans)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ParticipantEnrolledProgram)
                .WithMany()
                .HasForeignKey(p => p.ParticipantEnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.EmployabilityPlanStatusType)
                .WithMany()
                .HasForeignKey(p => p.EmployabilityPlanStatusTypeId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.SupportiveServices)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EmploybilityPlanActivityBridges)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.EmployabilityPlanGoalBridges)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(true);

            HasMany(p => p.ParticipationEntries)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EPId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ParticipationEntryHistories)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EPId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.CFParticipationEntries)
                .WithRequired(p => p.EmployabilityPlan)
                .HasForeignKey(p => p.EPId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EmployabilityPlan");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.BeginDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("date")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsOptional();

            Property(p => p.EmployabilityPlanStatusTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
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
                .IsOptional();

            Property(p => p.ParticipantEnrolledProgramId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SubmitDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.DateSigned)
                .HasColumnType("date")
                .IsOptional();

            #endregion
        }
    }
}
