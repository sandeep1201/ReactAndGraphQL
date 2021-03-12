using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RequestForAssistanceChildConfig : BaseCommonModelConfig<RequestForAssistanceChild>
    {
        public RequestForAssistanceChildConfig()
        {
            #region Relationships

            HasRequired(p => p.RequestForAssistance)
                .WithMany(p => p.RequestForAssistanceChilds)
                .HasForeignKey(p => p.RequestForAssistanceId);

            HasRequired(p => p.Child)
                .WithMany(p => p.RequestForAssistanceChildren)
                .HasForeignKey(p => p.ChildId);

            #endregion

            #region Properties

            ToTable("RequestForAssistanceChild");

            Property(p => p.RequestForAssistanceId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.ChildId)
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
