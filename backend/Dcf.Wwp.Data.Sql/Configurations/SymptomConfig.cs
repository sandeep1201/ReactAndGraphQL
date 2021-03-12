using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class SymptomConfig : BaseCommonModelConfig<Symptom>
    {
        public SymptomConfig()
        {
            #region Relationships

            HasMany(p => p.FormalAssessments)
                .WithOptional(p => p.Symptom)
                .HasForeignKey(p => p.SymptomId);

            #endregion

            #region Properties

            ToTable("Symptom");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
