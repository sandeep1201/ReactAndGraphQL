using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dfi.DataAccess.Base.Models;

namespace Dfi.DataAccess.Base.Configurations
{
    public abstract class BaseConfig<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            HasKey(p => p.Id);

            // scott v. - Remember to override the p.Id property in the descendant
            // and add .HasColumnName(<your pkColumnName>) 
            // and set .HasDatabaseGeneratedOption() according to the PK generation scheme 
            // of your table. Composite primary keys are handled by a different base class.

            Property(p => p.Id)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                    .HasColumnName("id")
                    .HasColumnType("int")
                    .IsRequired();

            Property(p => p.CreatedBy)
                    .HasColumnName("idUserCreate")
                    .HasColumnType("varchar")
                    .HasMaxLength(20)
                    .IsOptional();

            Property(p => p.CreatedOn)
                    .HasColumnName("dateCreate")
                    .HasColumnType("datetime")
                    .IsOptional();

            Property(p => p.UpdatedBy)
                    .HasColumnName("idUserUpdate")
                    .HasColumnType("varchar")
                    .HasMaxLength(20)
                    .IsOptional();

            Property(p => p.UpdatedOn)
                    .HasColumnName("dateUpdate")
                    .HasColumnType("datetime")
                    .IsOptional();

            #endregion
        }
    }
}
