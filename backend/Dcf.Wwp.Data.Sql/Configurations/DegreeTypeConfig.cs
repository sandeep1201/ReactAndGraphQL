using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class DegreeTypeConfig : BaseConfig<DegreeType>
    {
        public DegreeTypeConfig()
        {
            #region Relationships

            HasMany(p => p.PostSecondaryDegrees)
                .WithOptional(p => p.DegreeType)
                .HasForeignKey(p => p.DegreeTypeId);

            #endregion

            #region Properties

            ToTable("DegreeType");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

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
