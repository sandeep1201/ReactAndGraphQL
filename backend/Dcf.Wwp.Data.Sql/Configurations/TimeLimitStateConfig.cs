using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class TimeLimitStateConfig : BaseCommonModelConfig<TimeLimitState>
    {
        public TimeLimitStateConfig()
        {
            #region Relationships

            HasOptional(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId);

            #endregion

            #region Properties

            ToTable("TimeLimitState");

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.CountryId)
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
