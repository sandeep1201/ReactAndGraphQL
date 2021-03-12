using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class DrugScreeningStatusConfig : BaseConfig<DrugScreeningStatus>
    {
        public DrugScreeningStatusConfig()
        {
            #region Relationships

            HasRequired(p => p.DrugScreening)
                .WithMany(p => p.DrugScreeningStatuses)
                .HasForeignKey(p => p.DrugScreeningId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.DrugScreeningStatusType)
                .WithMany()
                .HasForeignKey(p => p.DrugScreeningStatusTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("DrugScreeningStatus");

            Property(p => p.DrugScreeningId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.DrugScreeningStatusTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.DrugScreeningStatusDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(380)
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
                .IsRequired();

            #endregion
        }
    }
}
