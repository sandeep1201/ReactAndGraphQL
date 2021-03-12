using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipantPlacementConfig : BaseConfig<ParticipantPlacement>
    {
        public ParticipantPlacementConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.ParticipantPlacements)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.PlacementType)
                .WithMany()
                .HasForeignKey(p => p.PlacementTypeId);

            #endregion

            #region Properties

            ToTable("ParticipantPlacement");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.CaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.PlacementTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PlacementStartDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.PlacementEndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
