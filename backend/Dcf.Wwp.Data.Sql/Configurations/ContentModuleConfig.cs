using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContentModuleConfig : BaseConfig<ContentModule>
    {
        public ContentModuleConfig()
        {
            #region Relationships

            HasOptional(p => p.ContentPage)
                .WithMany(p => p.ContentModules)
                .HasForeignKey(p => p.ContentPageId);

            HasMany(p => p.ContentModuleMetas)
                .WithOptional(p => p.ContentModule)
                .HasForeignKey(p => p.ContentModuleId);

            #endregion

            #region Properties

            ToTable("ContentModule");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            Property(p => p.Title)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsOptional();

            Property(p => p.ShowTitle)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.ShowDescription)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.Status)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ContentPageId)
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
