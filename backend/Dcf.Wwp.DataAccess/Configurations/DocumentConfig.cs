using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class DocumentConfig : BaseConfig<Document>
    {
        public DocumentConfig()
        {
            #region Relationships

            HasRequired(p => p.EmployabilityPlan)
                .WithMany()
                .HasForeignKey(p => p.EmployabilityPlanId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("Document");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.EmployabilityPlanId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.UploadedDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(p => p.IsScanned)
                .HasColumnType("bit")
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
