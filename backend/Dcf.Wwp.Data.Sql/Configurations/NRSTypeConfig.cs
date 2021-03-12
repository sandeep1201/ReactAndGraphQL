using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NRSTypeConfig : BaseCommonModelConfig<NRSType>
    {
        public NRSTypeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("NRSType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Rating)
                .HasColumnType("varchar")
                .HasMaxLength(10)
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
