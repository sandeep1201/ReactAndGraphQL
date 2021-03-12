using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class HousingHistoryConfig : BaseCommonModelConfig<HousingHistory>
    {
        public HousingHistoryConfig()
        {
            #region Relationships

            HasOptional(p => p.HousingSection)
                .WithMany(p => p.HousingHistories)
                .HasForeignKey(p => p.HousingSectionId);

            HasOptional(p => p.HousingSituation)
                .WithMany(p => p.HousingHistories)
                .HasForeignKey(p => p.HousingSituationId);

            #endregion

            #region Properties

            ToTable("HousingHistory");

            Property(p => p.HousingSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.HousingSituationId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.BeginDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EndDate)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.HasEvicted)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.MonthlyAmount)
                .HasColumnType("decimal")
                .HasPrecision(7, 2)
                .IsOptional();

            Property(p => p.IsAmountUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.OriginId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
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
