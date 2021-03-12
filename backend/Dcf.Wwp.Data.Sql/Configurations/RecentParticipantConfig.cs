using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RecentParticipantConfig : EntityTypeConfiguration<RecentParticipant>
    {
        public RecentParticipantConfig()
        {
            #region Relationships

            HasRequired(p => p.Worker)
                .WithMany(p => p.RecentParticipants)
                .HasForeignKey(p => p.WorkerId);

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("RecentParticipant");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.LastAccessed)
                .HasColumnType("datetime")
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

            Property(p => p.RowVersion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnType("timestamp")
                .IsRequired();

            #endregion
        }
    }
}
