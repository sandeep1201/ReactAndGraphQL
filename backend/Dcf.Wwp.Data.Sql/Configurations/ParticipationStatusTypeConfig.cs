using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ParticipationStatusTypeConfig : BaseCommonModelConfig<ParticipationStatusType>
    {
        public ParticipationStatusTypeConfig()
        {
            #region Relationships

            HasMany(p => p.ParticipationStatus)
                .WithOptional(p => p.ParticipationStatusType)
                .HasForeignKey(p => p.StatusId);

            #endregion

            #region Properties

            ToTable("ParticipationStatusType");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(2)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.EffectiveDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsOptional();

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
