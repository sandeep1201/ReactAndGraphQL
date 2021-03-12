using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ChildConfig : BaseCommonModelConfig<Child>
    {
        public ChildConfig()
        {
            #region Relationships

            HasOptional(p => p.GenderType)
                .WithMany()
                .HasForeignKey(p => p.GenderTypeId);

            HasMany(p => p.ChildYouthSectionChilds)
                .WithOptional(p => p.Child)
                .HasForeignKey(p => p.ChildId);

            HasMany(p => p.ParticipantChildRelationshipBridges)
                .WithOptional(p => p.Child)
                .HasForeignKey(p => p.ChildId);

            HasMany(p => p.RequestForAssistanceChildren)
                .WithRequired(p => p.Child)
                .HasForeignKey(p => p.ChildId);

            #endregion

            #region Properties

            ToTable("Child");

            Property(p => p.PinNumber)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsOptional();

            Property(p => p.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.MiddleInitialName) // this just makes things more sense
                .HasColumnName("MiddleInitialName")
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(140)
                .IsOptional();

            Property(p => p.SuffixName)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.DateOfBirth)
                .HasColumnType("date") // should be a DateTimeOffset type, it's an exact point in time
                .IsOptional();

            Property(p => p.DateOfDeath)
                .HasColumnType("date") // should be a DateTimeOffset type, it's an exact point in time
                .IsOptional();

            Property(p => p.GenderTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.GenderIndicator) // you already have a 'GenderTypeId', do you really need an indicator too?
                .HasColumnType("char")
                .HasMaxLength(1)
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
