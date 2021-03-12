using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class StateConfig : BaseCommonModelConfig<State>
    {
        public StateConfig()
        {
            #region Relationships

            HasOptional(p => p.Country)
                .WithMany(p => p.States)
                .HasForeignKey(p => p.CountryId);

            HasMany(p => p.Cities)
                .WithOptional(p => p.State)
                .HasForeignKey(p => p.StateId);

            #endregion

            #region Properties

            ToTable("State");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.CountryId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsNonStandard)
                .HasColumnType("bit")
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
                .IsOptional();

            #endregion
        }
    }
}
