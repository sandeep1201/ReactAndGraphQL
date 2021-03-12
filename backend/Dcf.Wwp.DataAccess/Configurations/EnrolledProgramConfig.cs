using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramConfig : BaseConfig<EnrolledProgram>
    {
        public EnrolledProgramConfig()
        {
            #region Relationships

            HasMany(p => p.EnrolledProgramValidities)
                .WithOptional(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.ContractAreas)
                .WithOptional(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EnrolledProgramEPActivityTypeBridges)
                .WithOptional(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.GoalTypes)
                .WithRequired(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.EnrolledProgramPinCommentTypeBridges)
                .WithRequired(p => p.EnrolledProgram)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("EnrolledProgram");

            Property(p => p.ProgramCode)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsOptional();

            Property(p => p.SubProgramCode)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsOptional();

            Property(p => p.ProgramType)
                .HasColumnType("char")
                .HasMaxLength(20)
                .IsOptional();

            Property(p => p.DescriptionText)
                .HasColumnType("varchar")
                .HasMaxLength(100)
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
                .IsOptional();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsOptional();

            Property(p => p.ShortName)
                .HasColumnType("varchar")
                .HasMaxLength(5)
                .IsOptional();

            #endregion
        }
    }
}
