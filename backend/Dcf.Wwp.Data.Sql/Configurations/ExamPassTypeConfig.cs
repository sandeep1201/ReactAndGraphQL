using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamPassTypeConfig : BaseConfig<ExamPassType>
    {
        public ExamPassTypeConfig()
        {
            #region Relationships

            HasMany(p => p.ExamResults)
                .WithOptional(p => p.ExamPassType)
                .HasForeignKey(p => p.ExamPassTypeId);

            #endregion

            #region Properties

            ToTable("ExamPassType");

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
