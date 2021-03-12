using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ConvictionTypeConfig : BaseConfig<ConvictionType>
    {
        public ConvictionTypeConfig()
        {
            #region Relationships

            HasMany(p => p.PendingCharges)
                .WithOptional(p => p.ConvictionType)
                .HasForeignKey(p => p.ConvictionTypeID);

            HasMany(p => p.Convictions)
                .WithOptional(p => p.ConvictionType)
                .HasForeignKey(p => p.ConvictionTypeID);

            #endregion

            #region Properties

            ToTable("ConvictionType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

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
