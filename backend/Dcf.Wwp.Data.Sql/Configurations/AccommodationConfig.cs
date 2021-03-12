using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AccommodationConfig : BaseCommonModelConfig<Accommodation>
    {
        public AccommodationConfig()
        {
            #region Relationships

            HasMany(p => p.BarrierAccommodations)
                .WithOptional(p => p.Accommodation)
                .HasForeignKey(p => p.AccommodationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Accommodation");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
