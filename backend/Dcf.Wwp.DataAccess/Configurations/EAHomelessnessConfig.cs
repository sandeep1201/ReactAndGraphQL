using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAHomelessnessConfig : BaseConfig<EAHomelessness>
    {
        public EAHomelessnessConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaHomelessnesses)
                .HasForeignKey(p => p.RequestId);

            #endregion

            #region Properties

            ToTable("EAHomelessness");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.InLackOfPlace)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DateOfStart)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.PlanOnPermanentPlace)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InShelterForDomesticAbuse)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsYourBuildingDecidedUnSafe)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DateBuildingWasDecidedUnSafe)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.IsInspectionReportAvailable)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
