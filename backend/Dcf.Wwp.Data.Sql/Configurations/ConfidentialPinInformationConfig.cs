using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ConfidentialPinInformationConfig : BaseConfig<ConfidentialPinInformation>
    {
        public ConfidentialPinInformationConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.Worker)
                .WithMany()
                .HasForeignKey(p => p.WorkerId);

            #endregion

            #region Properties

            ToTable("ConfidentialPinInformation");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.IsConfidential)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.WorkerId)
                .HasColumnType("int")
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
