using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ElevatedAccessConfig : BaseCommonModelConfig<ElevatedAccess>
    {
        public ElevatedAccessConfig()
        {
            #region Relationships

            HasRequired(p => p.Worker)
                .WithMany(p => p.ElevatedAccesses)
                .HasForeignKey(p => p.WorkerId);

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ElevatedAccessReason)
                .WithMany()
                .HasForeignKey(p => p.ElevatedAccessReasonId);

            #endregion

            #region Properties

            ToTable("ElevatedAccess");

            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.AccessCreateDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.ElevatedAccessReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
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
