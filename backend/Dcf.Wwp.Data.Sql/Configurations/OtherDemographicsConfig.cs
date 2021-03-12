using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class OtherDemographicsConfig : BaseConfig<OtherDemographic>
    {
        public OtherDemographicsConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.OtherDemographics)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.Language)
                .WithMany()
                .HasForeignKey(p => p.HomeLanguageId);

            HasOptional(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryOfOriginId);

            HasOptional(p => p.CountyAndTribe)
                .WithMany()
                .HasForeignKey(p => p.TribalId);

            #endregion

            #region Properties

            ToTable("OtherDemographics");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HomeLanguageId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsInterpreterNeeded)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InterpreterDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.IsRefugee)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.RefugeeEntryDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.RefugeeEntryDateUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.CountryOfOriginId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TribalIndicator)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.TribalId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TribalDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

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
