using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class PostSecondaryDegreeConfig : BaseCommonModelConfig<PostSecondaryDegree>
    {
        public PostSecondaryDegreeConfig()
        {
            #region Relationships

            HasRequired(p => p.PostSecondaryEducationSection)
                .WithMany(p => p.PostSecondaryDegrees)
                .HasForeignKey(p => p.PostSecondaryEducationSectionId);

            HasOptional(p => p.DegreeType)
                .WithMany()
                .HasForeignKey(p => p.DegreeTypeId);

            #endregion

            #region Properties

            ToTable("PostSecondaryDegree");

            Property(p => p.PostSecondaryEducationSectionId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.College)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.DegreeTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.YearAttained)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.OriginId)
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
