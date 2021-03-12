using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TransactionConfig : BaseCommonModelConfig<Transaction>
    {
        public TransactionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.Worker)
                .WithMany()
                .HasForeignKey(p => p.WorkerId);

            HasRequired(p => p.Office)
                .WithMany()
                .HasForeignKey(p => p.OfficeId);

            #endregion

            #region Properties

            ToTable("Transaction");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OfficeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.TransactionTypeId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.CreatedDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
