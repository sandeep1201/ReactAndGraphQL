using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TransportationSectionConfig : BaseCommonModelConfig<TransportationSection>
    {
        public TransportationSectionConfig()
        {
            #region Relationships

            HasRequired(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.IsVehicleInsuredYesNoUnknownLookup)
                .WithMany()
                .HasForeignKey(p => p.IsVehicleInsuredId);

            HasOptional(p => p.IsVehicleRegistrationCurrentYesNoUnknownLookup)
                .WithMany()
                .HasForeignKey(p => p.IsVehicleRegistrationCurrentId);

            HasOptional(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.DriversLicenseStateId);

            HasOptional(p => p.DriversLicenseInvalidReasonType)
                .WithMany()
                .HasForeignKey(p => p.DriversLicenseInvalidReasonId);

            HasMany(p => p.TransportationSectionMethodBridges)
                .WithRequired(p => p.TransportationSection)
                .HasForeignKey(p => p.TransportationSectionId);

            #endregion

            #region Properties

            ToTable("TransportationSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.TransporationDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsVehicleInsuredId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.VehicleInsuredDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.IsVehicleRegistrationCurrentId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.VehicleRegistrationCurrentDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.HasValidDrivingLicense)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DriversLicenseStateId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DriversLicenseExpirationDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.DriversLicenseDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.DriversLicenseInvalidReasonId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.DriversLicenseInvalidDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.HadCommercialDriversLicense)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsCommercialDriversLicenseActive)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CommercialDriversLicenseDetails)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.Notes)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
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
