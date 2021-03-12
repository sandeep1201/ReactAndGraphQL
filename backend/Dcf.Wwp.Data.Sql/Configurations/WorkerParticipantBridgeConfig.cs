using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkerParticipantBridgeConfig : EntityTypeConfiguration<WorkerParticipantBridge>
    {
        public WorkerParticipantBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.Worker)
                .WithMany(p => p.WorkerParticipantBridges)
                .HasForeignKey(p => p.WorkerId);

            HasOptional(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            #endregion

            #region Properties

            ToTable("WorkerParticipantBridge");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            #endregion
        }
    }
}
