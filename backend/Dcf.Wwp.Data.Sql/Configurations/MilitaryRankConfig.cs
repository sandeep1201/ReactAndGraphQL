using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class MilitaryRankConfig : BaseCommonModelConfig<MilitaryRank>
    {
        public MilitaryRankConfig()
        {
            #region Relationships

            HasMany(p => p.MilitaryTrainingSections)
                .WithOptional(p => p.MilitaryRank)
                .HasForeignKey(p => p.MilitaryRankId);

            #endregion

            #region Properties

            ToTable("MilitaryRank");

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
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            #endregion
        }
    }
}
