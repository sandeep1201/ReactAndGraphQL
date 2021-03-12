using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EAImpendingHomelessnessConfig : BaseConfig<EAImpendingHomelessness>
    {
        public EAImpendingHomelessnessConfig()
        {
            #region Relationships

            HasRequired(p => p.EaRequest)
                .WithMany(p => p.EaImpendingHomelessnesses)
                .HasForeignKey(p => p.RequestId);

            HasOptional(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.LandLordCityId);

            HasRequired(p => p.AddressVerificationTypeLookup)
                .WithMany()
                .HasForeignKey(p => p.AddressVerificationTypeLookupId);

            #endregion

            #region Properties

            ToTable("EAImpendingHomelessness");

            Property(p => p.RequestId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.HaveEvictionNotice)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DateOfEvictionNotice)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.DifficultToPayDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.IsCurrentLandLordUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.LandLordName)
                .HasColumnType("varchar")
                .HasMaxLength(125)
                .IsOptional();

            Property(p => p.ContactPerson)
                .HasColumnType("varchar")
                .HasMaxLength(125)
                .IsOptional();

            Property(p => p.LandLordPhone)
                .HasColumnType("varchar")
                .HasMaxLength(20)
                .IsOptional();

            Property(p => p.LandLordAddress)
                .HasColumnType("varchar")
                .HasMaxLength(250)
                .IsOptional();

            Property(p => p.LandLordCityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.LandLordZip)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.AddressVerificationTypeLookupId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.NeedingDifferentHomeForAbuse)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.NeedingDifferentHomeForRentalForeclosure)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DateOfFamilyDeparture)
                .HasColumnType("date")
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
