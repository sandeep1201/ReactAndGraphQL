using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WorkerConfig : BaseConfig<Worker>
    {
        public WorkerConfig()
        {
            #region Relationships

            HasOptional(p => p.Organization)
                .WithMany(p => p.Workers)
                .HasForeignKey(p => p.OrganizationId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.WorkerContactInfos)
                .WithRequired(p => p.Worker)
                .HasForeignKey(p => p.WorkerId);

            HasMany(p => p.Transactions)
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

            Property(p => p.Roles)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.WorkerActiveStatusCode)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.LastLogin)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WIUId)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsOptional();

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
