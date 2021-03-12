using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class FrequencyTypeConfig : BaseCommonModelConfig<FrequencyType>
    {
        public FrequencyTypeConfig()
        {
            #region Relationships

            HasMany(p => p.ActivitySchedules)
                .WithOptional(p => p.FrequencyType)
                .HasForeignKey(p => p.FrequencyTypeId);

            #endregion

            #region Properties

            ToTable("FrequencyType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
