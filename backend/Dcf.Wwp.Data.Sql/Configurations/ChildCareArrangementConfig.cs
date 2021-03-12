using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ChildCareArrangementConfig : BaseCommonModelConfig<ChildCareArrangement>
    {
        public ChildCareArrangementConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("ChildCareArrangement");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.SortOrder) // 'Name' can be null, but 'SortOrder' can't?? ~ lol
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
                .IsOptional();

            #endregion
        }
    }
}
