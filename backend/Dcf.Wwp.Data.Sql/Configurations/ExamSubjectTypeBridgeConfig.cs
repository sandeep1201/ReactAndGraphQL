using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ExamSubjectTypeBridgeConfig : BaseConfig<ExamSubjectTypeBridge>
    {
        public ExamSubjectTypeBridgeConfig()
        {
            #region Relationships

            HasOptional(p => p.ExamSubjectType).WithMany(p => p.ExamSubjectTypeBridges).HasForeignKey(p => p.ExamSubjectTypeId);

            HasOptional(p => p.ExamType).WithMany(p => p.ExamSubjectTypeBridges).HasForeignKey(p => p.ExamTypeId);

            #endregion

            #region Properties

            ToTable("ExamSubjectTypeBridge");

            Property(p => p.ExamSubjectTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ExamTypeId)
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
