using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonCustodialParentRelationshipConfig : BaseCommonModelConfig<NonCustodialParentRelationship>
    {
        public NonCustodialParentRelationshipConfig()
        {
            #region Relationships

            HasMany(p => p.NonCustodialCaretakers)
                .WithOptional(p => p.NonCustodialParentRelationship)
                .HasForeignKey(p => p.NonCustodialParentRelationshipId);

            #endregion

            #region Properties

            ToTable("NonCustodialParentRelationship");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
                .IsOptional();

            #endregion
        }
    }
}
