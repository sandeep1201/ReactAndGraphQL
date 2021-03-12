using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class RequestForAssistanceStatusConfig : BaseCommonModelConfig<RequestForAssistanceStatus>
    {
        public RequestForAssistanceStatusConfig()
        {
            #region Relationships

            HasMany(p => p.RequestsForAssistance)
                .WithRequired(p => p.RequestForAssistanceStatus)
                .HasForeignKey(p => p.RequestForAssistanceStatusId);

            #endregion

            #region Properties

            ToTable("RequestForAssistanceStatus");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

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
