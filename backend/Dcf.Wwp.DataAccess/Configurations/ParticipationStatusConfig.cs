using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ParticipantProfileStatusConfig : BaseConfig<ParticipantProfileStatus>
    {
        public ParticipantProfileStatusConfig()
        {
            #region Relationships
        
            HasOptional(p => p.Participant)
                .WithMany(p => p.ParticipantProfileStatuses)
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId)
                .WillCascadeOnDelete(false);
                
            #endregion

            #region Properties

            ToTable("ParticipantProfileStatus");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StatusId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.isCurrent)
                .HasColumnType("bit")
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
