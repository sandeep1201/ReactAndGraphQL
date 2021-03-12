using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class WorkerContactInfoConfig : BaseConfig<WorkerContactInfo>
    {
        public WorkerContactInfoConfig()
        {
            #region Relationships

            HasRequired(p => p.Worker)
                .WithMany(p => p.WorkerContactInfos)
                .HasForeignKey(p => p.WorkerId);

            #endregion

            #region Properties

            ToTable("WorkerContactInfo");

            Property(p => p.WorkerId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PhoneNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.Email)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
