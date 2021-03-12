using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RoleAuthorizationConfig : BaseConfig<RoleAuthorization>
    {
        public RoleAuthorizationConfig()
        {
            #region Relationships

            HasRequired(p => p.Role).WithMany().HasForeignKey(p => p.RoleId);

            HasRequired(p => p.Authorization).WithMany().HasForeignKey(p => p.AuthorizationId);

            #endregion

            #region Properties

            ToTable("RoleAuthorization", "sec");

            Property(p => p.RoleId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AuthorizationId)
                .HasColumnType("int")
                .IsRequired();

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
