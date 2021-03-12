using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipationMakeUpEntryConfig : BaseConfig<ParticipationMakeUpEntry>
    {
        public ParticipationMakeUpEntryConfig()
        {
            #region Relationships

            HasRequired(p => p.ParticipationEntry)
                .WithMany(p => p.ParticipationMakeUpEntries)
                .HasForeignKey(p => p.ParticipationEntryId)
                .WillCascadeOnDelete(true);

            #endregion

            #region Properties

            ToTable("ParticipationMakeUpEntry");

            Property(p => p.ParticipationEntryId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.MakeupDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.MakeupHours)
                .HasColumnType("decimal")
                .HasPrecision(3, 1)
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
