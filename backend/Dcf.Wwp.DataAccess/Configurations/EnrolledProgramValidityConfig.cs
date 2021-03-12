using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class EnrolledProgramValidityConfig : BaseConfig<EnrolledProgramValidity>
    {
        public EnrolledProgramValidityConfig()
        {
            #region Relationships

            HasOptional(p => p.EnrolledProgram)
                .WithMany(p => p.EnrolledProgramValidities)
                .HasForeignKey(p => p.EnrolledProgramId)
                .WillCascadeOnDelete(false);
                
            #endregion

            #region Properties

            ToTable("EnrolledProgramValidity");

            Property(p => p.EnrolledProgramId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.MaxDaysCanBackDate)
                .HasColumnType("int")
                .IsOptional();
            
            Property(p => p.MaxDaysInProgressStatus)
                .HasColumnType("int")
                .IsOptional();
            Property(p => p.MaxDaysCanBackDatePS)
                .HasColumnType("int")
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
                .IsRequired();

            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
