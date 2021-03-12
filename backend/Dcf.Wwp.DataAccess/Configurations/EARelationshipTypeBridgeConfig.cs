using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARelationshipTypeBridgeConfig : BaseConfig<EARelationshipTypeBridge>
    {
        public EARelationshipTypeBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRelationshipType)
                .WithMany()
                .HasForeignKey(p => p.RelationshipTypeId);

            #endregion

            #region Properties

            ToTable("EARelationshipTypeBridge");

            Property(p => p.RelationshipTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsOnlyForCR)
                .HasColumnType("bit")
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
                .IsRequired();

            #endregion
        }
    }
}
