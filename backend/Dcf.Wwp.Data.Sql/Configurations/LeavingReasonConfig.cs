using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class LeavingReasonConfig : BaseCommonModelConfig<LeavingReason>
    {
        public LeavingReasonConfig()
        {
            #region Relationships

            HasMany(p => p.EmploymentInformations)
                .WithOptional(p => p.LeavingReason)
                .HasForeignKey(p => p.LeavingReasonId);

            HasMany(p => p.JobTypeLeavingReasonBridges)
                .WithRequired(p => p.LeavingReason)
                .HasForeignKey(p => p.LeavingReasonId);

            #endregion

            #region Properties

            ToTable("LeavingReason");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
