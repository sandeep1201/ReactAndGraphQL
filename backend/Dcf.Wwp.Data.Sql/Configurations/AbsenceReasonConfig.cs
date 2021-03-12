using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AbsenceReasonConfig : BaseCommonModelConfig<AbsenceReason>
    {
        public AbsenceReasonConfig()
        {
            #region Relationships

            HasMany(p => p.Absences)
                .WithOptional(p => p.AbsenceReason)
                .HasForeignKey(p => p.AbsenceReasonId);

            #endregion

            #region Properties

            ToTable("AbsenceReason");

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
