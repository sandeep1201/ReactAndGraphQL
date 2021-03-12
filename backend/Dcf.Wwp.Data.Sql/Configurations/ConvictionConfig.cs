using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class ConvictionConfig : BaseConfig<Conviction>
    {
        public ConvictionConfig()
        {
            #region Relationships

            HasOptional(p => p.ConvictionType)
                .WithMany(p => p.Convictions)
                .HasForeignKey(p => p.ConvictionTypeID);

            HasOptional(p => p.LegalIssuesSection)
                .WithMany(p => p.Convictions)
                .HasForeignKey(p => p.LegalSectionId);

            HasOptional(p => p.DeleteReason)
                .WithMany(p => p.Convictions)
                .HasForeignKey(p => p.DeleteReasonId);

            #endregion

            #region Properties

            ToTable("Conviction");

            Property(p => p.LegalSectionId)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.ConvictionTypeID)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsUnknown)
                .HasColumnType("bit")
                .IsOptional();

            Property(p => p.DateConvicted)
                .HasColumnType("datetime")
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
