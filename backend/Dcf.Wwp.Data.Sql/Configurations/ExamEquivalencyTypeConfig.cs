using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamEquivalencyTypeConfig : BaseConfig<ExamEquivalencyType>
    {
        public ExamEquivalencyTypeConfig()
        {
            #region Relationships

            HasMany(p => p.ExamResults)
                .WithOptional(p => p.ExamEquivalencyType)
                .HasForeignKey(p => p.ExamEquivalencyTypeId);

            #endregion

            #region Properties

            ToTable("ExamEquivalencyType");

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
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
