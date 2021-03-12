using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamLevelTypeConfig : BaseConfig<ExamLevelType>
    {
        public ExamLevelTypeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("ExamLevelType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.ExamType)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
