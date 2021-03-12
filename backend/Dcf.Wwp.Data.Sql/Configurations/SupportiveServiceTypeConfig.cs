using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SupportiveServiceTypeConfig : BaseCommonModelConfig<SupportiveServiceType>
    {
        public SupportiveServiceTypeConfig()
        {
            #region Relationships

            HasMany(p => p.SupportiveServices)
                .WithRequired(p => p.SupportiveServiceType)
                .HasForeignKey(p => p.SupportiveServiceTypeId);
            #endregion

            #region Properties

            ToTable("SupportiveServiceType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
