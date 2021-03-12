using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RequestForAssistanceRuleReasonConfig : BaseCommonModelConfig<RequestForAssistanceRuleReason>
    {
        public RequestForAssistanceRuleReasonConfig()
        {
            #region Relationships

            HasRequired(p => p.RequestForAssistance)
                .WithMany(p => p.RequestForAssistanceRuleReasons)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasRequired(p => p.RuleReason)
                .WithMany()
                .HasForeignKey(p => p.RuleReasonId);

            #endregion

            #region Properties

            ToTable("RequestForAssistanceRuleReason");

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RuleReasonId)
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
