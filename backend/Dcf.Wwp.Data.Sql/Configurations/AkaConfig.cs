using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class AkaConfig : BaseCommonModelConfig<AKA>
    {
        public AkaConfig()
        {
            #region Relationships

            HasOptional(p => p.Participant)
                .WithMany(p => p.AKAs)
                .HasForeignKey(p => p.ParticipantId);

            HasOptional(p => p.SSNType)
                .WithMany()
                .HasForeignKey(p => p.SSNTypeId);

            #endregion

            #region Properties

            ToTable("AKA");

            Property(p => p.ParticipantId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SSNNumber)
                .HasColumnType("decimal") // I'd have made this an nvarchar(11) including the '-'s  (it's not used in a computation, so why store it as a number? ~ lol)
                .HasPrecision(9, 0)
                .IsOptional();

            Property(p => p.SSNTypeId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.CreatedDate)
                .HasColumnType("datetime")
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
