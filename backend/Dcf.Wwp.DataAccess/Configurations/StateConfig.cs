using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class StateConfig : BaseConfig<State>
    {
        public StateConfig()
        {
            #region Relationships

            HasOptional(p => p.Country)
                .WithMany(p => p.States)
                .HasForeignKey(p => p.CountryId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Cities)
                .WithOptional(p => p.State)
                .HasForeignKey(p => p.StateId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("State");

            Property(p => p.Id)
               .HasColumnType("int")
               .IsOptional();

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
