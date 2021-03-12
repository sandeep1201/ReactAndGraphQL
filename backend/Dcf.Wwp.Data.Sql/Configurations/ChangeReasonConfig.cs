using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ChangeReasonConfig : BaseConfig<ChangeReason>
    {
        public ChangeReasonConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("ChangeReason");

            Property(p => p.Code)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsRequired)
                .HasColumnType("bit")
                .IsOptional(); // property IsRequired is optional ~ hahaha...

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
