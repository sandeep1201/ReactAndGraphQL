using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ContactConfig : BaseConfig<Contact>
    {
        public ContactConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany()
                .HasForeignKey(p => p.ParticipantId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Title)
                .WithMany()
                .HasForeignKey(p => p.TitleId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ActivityContactBridges)
                .WithOptional(p => p.Contact)
                .HasForeignKey(p => p.ContactId)
                .WillCascadeOnDelete(false);

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
                .HasMaxLength(140)
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
                .IsOptional();

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
