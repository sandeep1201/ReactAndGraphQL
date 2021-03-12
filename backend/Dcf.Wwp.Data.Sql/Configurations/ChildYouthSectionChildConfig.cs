using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ChildYouthSectionChildConfig : BaseConfig<ChildYouthSectionChild>
    {
        public ChildYouthSectionChildConfig()
        {
            #region Relationships

            HasOptional(p => p.ChildYouthSection)
                .WithMany(p => p.ChildYouthSectionChilds)
                .HasForeignKey(p => p.ChildYouthSectionId);

            HasOptional(p => p.Child)
                .WithMany(p => p.ChildYouthSectionChilds)
                .HasForeignKey(p => p.ChildId);

            HasOptional(p => p.ChildCareArrangement)
                .WithMany()
                .HasForeignKey(p => p.CareArrangementId);

            HasOptional(p => p.AgeCategory)
                .WithMany(p => p.ChildYouthSectionChilds)
                .HasForeignKey(p => p.AgeCategoryId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.ChildYouthSectionChilds)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("ChildYouthSectionChild");

            Property(p => p.ChildYouthSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ChildId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.CareArrangementId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.AgeCategoryId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsSpecialNeeds)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsOptional();

            Property(p => p.DeleteReasonId)
                .HasColumnType("int")
                .IsOptional();

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
