using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TransportationTypeConfig : BaseCommonModelConfig<TransportationType>
    {
        public TransportationTypeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("TransportationType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.DisablesOthersFlag)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.RequiresInsurance)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.RequiresCurrentRegistration)
                .HasColumnType("bit")
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
                .IsOptional();

            #endregion
        }
    }
}
