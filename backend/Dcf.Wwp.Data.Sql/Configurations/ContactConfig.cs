using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContactConfig : BaseCommonModelConfig<Contact>
    {
        public ContactConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.Contacts)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.ContactTitleType)
                .WithMany(p => p.Contacts)
                .HasForeignKey(p => p.TitleId);

            HasMany(p => p.ActivityContactBridges)
                .WithOptional(p => p.Contact)
                .HasForeignKey(p => p.ContactId);

            #endregion

            #region Properties

            ToTable("Contact");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.TitleId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CustomTitle)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.Email)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ExtensionNo)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.FaxNo)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ReleaseInformationDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Address)
                .HasColumnType("varchar")
                .HasMaxLength(380)
                .IsOptional();

            Property(p => p.LegalIssuesSectionId)
                .HasColumnType("int")
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
