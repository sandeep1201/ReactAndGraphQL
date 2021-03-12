using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RelationshipConfig : BaseCommonModelConfig<Relationship>
    {
        public RelationshipConfig()
        {
            #region Relationships

            HasMany(p => p.FamilyMembers)
                .WithOptional(p => p.Relationship)
                .HasForeignKey(p => p.RelationshipId);

            #endregion

            #region Properties

            ToTable("Relationship");

            Property(p => p.RelationName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
