using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContactIntervalConfig : BaseCommonModelConfig<ContactInterval>
    {
        public ContactIntervalConfig()
        {
            #region Relationships

            HasMany(p => p.NonCustodialCaretakers)
                .WithOptional(p => p.ContactInterval)
                .HasForeignKey(p => p.ContactIntervalId);

            HasMany(p => p.NonCustodialChilds)
                .WithOptional(p => p.ContactInterval)
                .HasForeignKey(p => p.ContactIntervalId);

            #endregion

            #region Properties

            ToTable("ContactInterval");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(p => p.SortOrder)
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
