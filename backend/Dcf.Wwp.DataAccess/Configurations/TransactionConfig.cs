using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class TransactionConfig : BaseConfig<Transaction>
    {
        public TransactionConfig()
        {
            #region Relationship

            HasRequired(p => p.Participant)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.ParticipantId);

            HasRequired(p => p.Worker)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.WorkerId);

            HasRequired(p => p.Office)
                .WithMany()
                .HasForeignKey(p => p.OfficeId);

            HasRequired(p => p.TransactionType)
                .WithMany()
                .HasForeignKey(p => p.TransactionTypeId);

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
