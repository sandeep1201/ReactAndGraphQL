using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class NonSelfDirectedActivityConfig : BaseCommonModelConfig<NonSelfDirectedActivity>
    {
        public NonSelfDirectedActivityConfig()
        {
            #region Relationships

            HasRequired(p => p.Activity)
                .WithMany(p => p.NonSelfDirectedActivities)
                .HasForeignKey(p => p.ActivityId);

            HasOptional(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId);

            #endregion

            #region Properties

            ToTable("NonSelfDirectedActivity");

            Property(p => p.ActivityId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.BusinessName)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(p => p.PhoneNumber)
                .HasColumnType("decimal")
                .HasPrecision(18, 0)
                .IsOptional();

            Property(p => p.StreetAddress)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.CityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ZipAddress)
                .HasColumnType("varchar")
                .HasMaxLength(9)
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
                .IsRequired();

            #endregion
        }
    }
}
