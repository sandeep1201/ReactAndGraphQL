using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class LanguageSectionConfig : BaseCommonModelConfig<LanguageSection>
    {
        public LanguageSectionConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId);

            HasMany(p => p.KnownLanguages)
                .WithRequired(p => p.LanguageSection)
                .HasForeignKey(p => p.LanguageSectionId);

            #endregion

            #region Properties

            ToTable("LanguageSection");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.IsAbleToReadEnglish)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAbleToWriteEnglish)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAbleToSpeakEnglish)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsNeedingInterpreter)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.InterpreterDetails)
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
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
