using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContentModuleMetaConfig : BaseConfig<ContentModuleMeta>
    {
        public ContentModuleMetaConfig()
        {
            #region Relationships

            HasOptional(p => p.ContentModule)
                .WithMany(p => p.ContentModuleMetas)
                .HasForeignKey(p => p.ContentModuleId);

            #endregion

            #region Properties

            ToTable("ContentModuleMeta");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsOptional();

            Property(p => p.Data)
                .HasColumnType("varchar(max)")
                .IsOptional();

            Property(p => p.ContentModuleId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
