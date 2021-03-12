using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContactTitleTypeConfig : BaseConfig<ContactTitleType>
    {
        public ContactTitleTypeConfig()
        {
            #region Relationships

            HasMany(p => p.Contacts)
                .WithOptional(p => p.ContactTitleType)
                .HasForeignKey(p => p.TitleId);

            #endregion

            #region Properties

            ToTable("ContactTitleType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
