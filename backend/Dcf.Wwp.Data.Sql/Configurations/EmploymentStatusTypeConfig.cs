using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EmploymentStatusTypeConfig : BaseCommonModelConfig<EmploymentStatusType>
    {
        public EmploymentStatusTypeConfig()
        {
            #region Relationships

            HasMany(p => p.WorkHistorySections)
                .WithOptional(p => p.EmploymentStatusType)
                .HasForeignKey(p => p.EmploymentStatusTypeId);

            #endregion

            #region Properties

            ToTable("EmploymentStatusType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
