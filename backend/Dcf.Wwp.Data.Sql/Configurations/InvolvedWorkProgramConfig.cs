using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class InvolvedWorkProgramConfig : BaseCommonModelConfig<InvolvedWorkProgram>
    {
        public InvolvedWorkProgramConfig()
        {
            #region Relationships

            HasOptional(p => p.WorkProgramSection)
                .WithMany(p => p.InvolvedWorkPrograms)
                .HasForeignKey(p => p.WorkProgramSectionId);

            HasOptional(p => p.WorkProgramStatus)
                .WithMany(p => p.InvolvedWorkPrograms)
                .HasForeignKey(p => p.WorkProgramStatusId);

            HasOptional(p => p.WorkProgram)
                .WithMany()
                .HasForeignKey(p => p.WorkProgramId);

            HasOptional(p => p.City)
                .WithMany(p => p.InvolvedWorkPrograms)
                .HasForeignKey(p => p.CityId);

            HasOptional(p => p.Contact)
                .WithMany(p => p.InvolvedWorkPrograms)
                .HasForeignKey(p => p.ContactId);

            #endregion

            #region Properties

            ToTable("InvolvedWorkProgram");

            Property(p => p.WorkProgramSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WorkProgramStatusId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.WorkProgramId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CityId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.StartMonth)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.EndMonth)
                .HasColumnType("date")
                .IsOptional();

            Property(p => p.ContactId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ContactInfo)
                .HasColumnType("varchar")
                .HasMaxLength(300)
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(400)
                .IsOptional();

            Property(p => p.SortOrder)
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
