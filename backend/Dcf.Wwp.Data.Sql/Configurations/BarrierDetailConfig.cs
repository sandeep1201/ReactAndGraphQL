using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierDetailConfig : BaseCommonModelConfig<BarrierDetail>
    {
        public BarrierDetailConfig()
        {
            #region Relationships

            HasOptional(p => p.BarrierSection)
                .WithMany(p => p.BarrierDetails)
                .HasForeignKey(p => p.BarrierSectionId);

            HasOptional(p => p.BarrierType)
                .WithMany()
                .HasForeignKey(p => p.BarrierTypeId);

            HasOptional(p => p.Participant)
                .WithMany(p => p.BarrierDetails)
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.BarrierAccommodations)
                .WithOptional(p => p.BarrierDetail)
                .HasForeignKey(p => p.BarrierDetailsId);

            HasMany(p => p.BarrierDetailContactBridges)
                .WithOptional(p => p.BarrierDetail)
                .HasForeignKey(p => p.BarrierDetailId);

            HasMany(p => p.BarrierTypeBarrierSubTypeBridges)
                .WithOptional(p => p.BarrierDetail)
                .HasForeignKey(p => p.BarrierDetailId);

            HasMany(p => p.FormalAssessments)
                .WithOptional(p => p.BarrierDetail)
                .HasForeignKey(p => p.BarrierDetailsId);

            #endregion

            #region Properties

            ToTable("BarrierDetail");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BarrierTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BarrierSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OnsetDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsAccommodationNeeded)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.WasClosedAtDisenrollment)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsConverted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
