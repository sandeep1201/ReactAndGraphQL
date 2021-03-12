using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WebServiceMessageConfig : EntityTypeConfiguration<WebServiceMessage>
    {
        public WebServiceMessageConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("WebServiceMessage");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.MsgId)
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(p => p.MsgDateTime)
                .HasColumnType("datetime2")
                .IsRequired();

            Property(p => p.MsgEndpoint)
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsRequired();

            Property(p => p.MsgOperation)
                .HasColumnType("varchar")
                .HasMaxLength(75)
                .IsOptional();

            Property(p => p.MsgDirection)
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired();

            Property(p => p.MsgXml)
                .HasColumnType("varchar")
                .HasMaxLength(4000)
                .IsRequired();

            Property(p => p.MsgLocalDateTime)
                .HasColumnType("datetime2")
                .IsOptional();

            #endregion
        }
    }
}
