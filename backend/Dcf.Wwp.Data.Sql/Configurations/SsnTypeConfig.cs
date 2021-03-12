using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SsnTypeConfig : BaseConfig<SSNType>
    {
        public SsnTypeConfig()
        {
            #region Relationships

            HasMany(p => p.AKAs)
                .WithOptional(p => p.SSNType)
                .HasForeignKey(p => p.SSNTypeId);

            #endregion

            #region Properties

            ToTable("SSNType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.IsDetailsRequired)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
