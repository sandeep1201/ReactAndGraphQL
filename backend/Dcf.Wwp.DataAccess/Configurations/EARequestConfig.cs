using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EARequestConfig : BaseConfig<EARequest>
    {
        public EARequestConfig()
        {
            #region Relationships

            HasOptional(p => p.Organization)
                .WithMany()
                .HasForeignKey(p => p.OrganizationId);

            HasOptional(p => p.EaApplicationInitiationMethodLookUp)
                .WithMany()
                .HasForeignKey(p => p.ApplicationInitiatedMethodId);

            HasMany(p => p.EaImpendingHomelessnesses)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaHomelessnesses)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaEnergyCrises)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaRequestContactInfos)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaComments)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaRequestParticipantBridges)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.EARequestId);

            HasMany(p => p.EaRequestEmergencyTypeBridges)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaRequestStatuses)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaPayments)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            HasMany(p => p.EaFinancialNeeds)
                .WithRequired(p => p.EaRequest)
                .HasForeignKey(p => p.RequestId);

            #endregion

            #region Properties

            ToTable("EARequest");

            Property(p => p.RequestNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.ApplicationDate)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.DidApplicantTakeCareOfAnyChild)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.WillTheChildStayInApplicantCare)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.EmergencyDetails)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.HasNoIncome)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasNoAssets)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.HasNoVehicles)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ApprovedPaymentAmount)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.IsPreviousMemberClicked)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CaresCaseNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.OrganizationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ApplicationInitiatedMethodId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AccessTrackingNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
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
