using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class AuxiliaryStatusConfig : BaseConfig<AuxiliaryStatus>
    {
        public AuxiliaryStatusConfig()
        {
            #region Relationships

            HasRequired(p => p.Auxiliary)
                .WithMany(p => p.AuxiliaryStatuses)
                .HasForeignKey(p => p.AuxiliaryId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.AuxiliaryStatusType)
                .WithMany()
                .HasForeignKey(p => p.AuxiliaryStatusTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("AuxiliaryStatus");

            Property(p => p.AuxiliaryId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AuxiliaryStatusTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AuxiliaryStatusDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(380)
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
