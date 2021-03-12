using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ApplicationStatusTypeConfig : BaseCommonModelConfig<ApplicationStatusType>
    {
        public ApplicationStatusTypeConfig()
        {
            #region Relationships

            HasMany(p => p.FamilyBarriersSections)
                .WithOptional(p => p.ApplicationStatusType)
                .HasForeignKey(p => p.SsiApplicationStatusId);

            #endregion

            #region Properties

            ToTable("ApplicationStatusType");

            Property(p => p.ApplicationStatusName)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.IsRequired)
                .HasColumnType("bit")
                .IsOptional();

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
                .IsOptional();

            #endregion
        }
    }
}
