using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WorkerConfig : BaseConfig<Worker>
    {
        public WorkerConfig()
        {
            #region Relationships

            HasMany(p => p.ConfidentialPinInformations)
                .WithOptional(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            HasMany(p => p.ElevatedAccesses)
                .WithRequired(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            HasOptional(p => p.Organization)
                .WithMany(p => p.Workers)
                .HasForeignKey(p => p.OrganizationId);

            HasMany(p => p.ParticipantEnrolledPrograms)
                .WithOptional(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            HasMany(p => p.ParticipantEnrolledPrograms1)
                .WithOptional(p => p.LFFEP)
                .HasForeignKey(p => p.LFFEPId);

            HasMany(p => p.RecentParticipants)
                .WithRequired(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            HasMany(p => p.WorkerParticipantBridges)
                .WithOptional(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            HasMany(p => p.WorkerTaskLists)
                .WithRequired(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            #endregion

            #region Properties

            ToTable("Worker");

            Property(p => p.WAMSId)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.MFUserId)
                .HasColumnName("MFUserId")
                .HasColumnType("varchar")
                .HasMaxLength(6)
                .IsOptional();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.MiddleInitial)
                .HasColumnType("varchar")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.SuffixName)
                .HasColumnType("varchar")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.Roles)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.WorkerActiveStatusCode)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.LastLogin)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.WIUID)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsOptional();

            #endregion
        }
    }
}
