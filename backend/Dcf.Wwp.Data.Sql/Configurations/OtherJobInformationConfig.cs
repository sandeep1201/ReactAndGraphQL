using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class OtherJobInformationConfig : BaseCommonModelConfig<OtherJobInformation>
    {
        public OtherJobInformationConfig()
        {
            #region Relationships

            HasOptional(p => p.JobSector)
                .WithMany()
                .HasForeignKey(p => p.JobSectorId);

            HasOptional(p => p.JobFoundMethod)
                .WithMany()
                .HasForeignKey(p => p.JobFoundMethodId);

            HasOptional(p => p.WorkProgram)
                .WithMany()
                .HasForeignKey(p => p.WorkProgramId);

            HasMany(p => p.EmploymentInformations)
                .WithOptional(p => p.OtherJobInformation)
                .HasForeignKey(p => p.OtherJobInformationId);

            #endregion

            #region Properties

            ToTable("OtherJobInformation");

            Property(p => p.ExpectedScheduleDetails)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.JobSectorId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobFoundMethodId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.JobFoundMethodDetails)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(p => p.WorkerId)
                .HasColumnType("varchar")
                .HasMaxLength(120)
                .IsOptional();

            Property(p => p.WorkProgramId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.SortOrder)
                .HasColumnType("int")
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
