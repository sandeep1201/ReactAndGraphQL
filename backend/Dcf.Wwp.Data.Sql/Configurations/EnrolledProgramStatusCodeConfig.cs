using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class EnrolledProgramStatusCodeConfig : BaseConfig<EnrolledProgramStatusCode>
    {
        public EnrolledProgramStatusCodeConfig()
        {
            #region Relationships

            // none 

            #endregion

            #region Properties

            ToTable("EnrolledProgramStatusCode");

            Property(p => p.StatusCode)
                .HasColumnType("varchar")
                .HasMaxLength(50)
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
