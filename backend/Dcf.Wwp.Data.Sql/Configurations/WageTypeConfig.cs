using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WageTypeConfig : BaseCommonModelConfig<WageType>
    {
        public WageTypeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("WageType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.DisablesOthersFlag)
                .HasColumnType("bit")
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
                .IsOptional();

            #endregion
        }
    }
}
