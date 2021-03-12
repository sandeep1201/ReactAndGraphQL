using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class DrugScreeningConfig : BaseConfig<DrugScreening>
    {
        public DrugScreeningConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.DrugScreenings)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.DrugScreeningStatuses)
                .WithRequired(p => p.DrugScreening)
                .HasForeignKey(p => p.DrugScreeningId);

            #endregion

            #region Properties

            ToTable("DrugScreening");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.UsedNonRequiredDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.AbusedMoreDrugs)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CannotStopAbusingDrugs)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HadBlackoutsFromDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.FeelGuiltyAboutDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.SpouseComplaintOnDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.NeglectedFamilyForDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IllegalActivitiesForDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.SickFromStoppingDrugs)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.MedicalProblemsFromDrugs)
                .HasColumnType("bit")
                .IsRequired();

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
