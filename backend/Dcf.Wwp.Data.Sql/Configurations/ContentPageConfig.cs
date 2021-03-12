using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ContentPageConfig : BaseConfig<ContentPage>
    {
        public ContentPageConfig()
        {
            #region Relationships

            HasMany(p => p.ContentModules)
                .WithOptional(p => p.ContentPage)
                .HasForeignKey(p => p.ContentPageId);

            #endregion

            #region Properties

            ToTable("ContentPage");

            Property(p => p.Title)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

            Property(p => p.Slug)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

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
