using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PostSecondaryLicenseConfig : BaseCommonModelConfig<PostSecondaryLicense>
    {
        public PostSecondaryLicenseConfig()
        {
            #region Relationships

            HasOptional(p => p.PolarLookup)
                .WithMany()
                .HasForeignKey(p => p.ValidInWIPolarLookupId);

            HasOptional(p => p.LicenseType)
                .WithMany()
                .HasForeignKey(p => p.LicenseTypeId);

            HasRequired(p => p.PostSecondaryEducationSection)
                .WithMany(p => p.PostSecondaryLicenses)
                .HasForeignKey(p => p.PostSecondaryEducationSectionId);

            #endregion

            #region Properties

            ToTable("PostSecondaryLicense");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.Issuer)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.AttainedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.ExpiredDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsInProgress)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DoesNotExpire)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.ValidInWIPolarLookupId)
                .HasColumnName("ValidInWIPolarLookupId")
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.LicenseTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.PostSecondaryEducationSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.OriginId)
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
                .IsOptional();

            #endregion
        }
    }
}
