using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JRWorkPreferencesConfig : BaseConfig<JRWorkPreferences>
    {
        public JRWorkPreferencesConfig()
        {
            #region Relationships

            HasRequired(p => p.JobReadiness)
                .WithMany()
                .HasForeignKey(p => p.JobReadinessId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.JrWorkPreferenceShiftBridges)
                .WithRequired(p => p.JrWorkPreferences)
                .HasForeignKey(p => p.WorkPreferenceId);

            #endregion

            #region Properties

            ToTable("JRWorkPreferences");

            Property(p => p.JobReadinessId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.KindOfJobDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.JobInterestDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.TrainingNeededForJobDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.SomeOtherPlacesJobAvailableDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.SomeOtherPlacesJobAvailableUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SituationsToAvoidDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.WorkScheduleBeginTime)
                .HasColumnType("time")
                .IsOptional();

            Property(p => p.WorkScheduleEndTime)
                .HasColumnType("time")
                .IsOptional();

            Property(p => p.WorkScheduleDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.TravelTimeToWork)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.DistanceHomeToWork)
                .HasColumnType("varchar")
                .HasMaxLength(120)
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

            #endregion
        }
    }
}
