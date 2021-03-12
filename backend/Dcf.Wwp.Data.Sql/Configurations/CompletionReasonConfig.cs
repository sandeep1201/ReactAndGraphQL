using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class CompletionReasonConfig : BaseCommonModelConfig<CompletionReason>
    {
        public CompletionReasonConfig()
        {
            #region Relationships

            HasRequired(p => p.EnrolledProgram)
                .WithMany(p => p.CompletionReasons)
                .HasForeignKey(p => p.EnrolledProgramId);

            HasMany(p => p.ParticipantEnrolledPrograms)
                .WithOptional(p => p.CompletionReason)
                .HasForeignKey(p => p.CompletionReasonId);

            #endregion

            #region Properties

            ToTable("CompletionReason");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsOptional();

            Property(p => p.Code)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsOptional();

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
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

            Property(p => p.IsSystemUseOnly)
                .HasColumnType("bit")
                .IsRequired();

            #endregion
        }
    }
}
