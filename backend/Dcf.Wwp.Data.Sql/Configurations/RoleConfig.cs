using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RoleConfig : BaseCommonModelConfig<Role>
    {
        public RoleConfig()
        {
            #region Relationships

            HasMany(p => p.RoleAuthorizations)
                .WithRequired(p => p.Role)
                .HasForeignKey(p => p.RoleId);

            #endregion

            #region Properties

            ToTable("Role", "sec");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.InheritedRoleId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
