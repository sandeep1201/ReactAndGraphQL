using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class BarrierAccommodationConfig : BaseConfig<BarrierAccommodation>
    {
        public BarrierAccommodationConfig()
        {
            #region Relationships

            HasOptional(p => p.BarrierDetail)
                .WithMany(p => p.BarrierAccommodations)
                .HasForeignKey(p => p.BarrierDetailsId);

            HasOptional(p => p.Accommodation)
                .WithMany(p => p.BarrierAccommodations)
                .HasForeignKey(p => p.AccommodationId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.BarrierAccommodations)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("BarrierAccommodation");

            Property(p => p.BarrierDetailsId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AccommodationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

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
