using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Dcf.Wwp.DataAccess.Base
{
    public abstract class BaseConfig<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseConfig()
        {
            #region Relationships

            #endregion

            #region Properties

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id")
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RowVersion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnType("timestamp")
                //.IsConcurrencyToken() // this will not initialize or update 
                .IsRowVersion()       // use this instead
                .IsRequired();

            #endregion
        }
    }
}
