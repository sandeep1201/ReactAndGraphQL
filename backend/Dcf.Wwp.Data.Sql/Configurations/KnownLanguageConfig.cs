using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class KnownLanguageConfig : BaseCommonModelConfig<KnownLanguage>
    {
        public KnownLanguageConfig()
        {
            #region Relationships

            HasRequired(p => p.LanguageSection)
                .WithMany(p => p.KnownLanguages)
                .HasForeignKey(p => p.LanguageSectionId);

            HasRequired(p => p.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId);

            #endregion

            #region Properties

            ToTable("KnownLanguage");

            Property(p => p.PinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.LanguageSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.LanguageId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsPrimary)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAbleToRead)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAbleToWrite)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.IsAbleToSpeak)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
