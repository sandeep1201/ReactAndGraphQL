using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class YesNoUnknownLookupConfig : EntityTypeConfiguration<YesNoUnknownLookup>
    {
        public YesNoUnknownLookupConfig()
        {
            #region Relationships

            HasMany(p => p.JrApplicationInfos)
                .WithOptional(p => p.NeedDocumentLookup)
                .HasForeignKey(p => p.NeedDocumentLookupId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("YesNoUnknownLookup");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            #endregion
        }
    }
}
