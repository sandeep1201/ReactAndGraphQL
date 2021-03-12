using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EmployabilityPlanStatusTypeConfig : BaseConfig<EmployabilityPlanStatusType>
    {
        public EmployabilityPlanStatusTypeConfig()
        {
            //#region Relationships

            //HasMany(p => p.EnrolledProgramEPActivityTypeBridges)
            //    .WithOptional(p => p.ActivityType)
            //    .HasForeignKey(p => p.ActivityTypeId)
            //    .WillCascadeOnDelete(false);

            //#endregion

            #region Properties

            ToTable("EmployabilityPlanStatusType");


            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
                .IsRequired();

            #endregion
        }
    }
}
