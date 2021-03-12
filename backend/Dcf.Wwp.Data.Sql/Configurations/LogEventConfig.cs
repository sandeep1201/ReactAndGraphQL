using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class LogEventConfig : EntityTypeConfiguration<LogEvent>
    {
        public LogEventConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("LogEvent");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Message)
                .HasColumnType("nvarchar(max)")
                .IsOptional();

            Property(p => p.MessageTemplate)
                .HasColumnType("nvarchar(max)")
                .IsOptional();

            Property(p => p.Level)
                .HasColumnType("tinyint")
                .IsOptional();

            Property(p => p.TimeStamp)
                .HasColumnType("datetimeoffset")
                .IsRequired();

            Property(p => p.Exception)
                .HasColumnType("nvarchar(max)")
                .IsOptional();

            Property(p => p.Properties)
                .HasColumnType("xml")
                .IsOptional();

            Property(p => p.Details)
                .HasColumnName("LogEvent") // can't have the same name as the enclosing class/entity (or column name can't be same as table name)
                .HasColumnType("nvarchar(max)")
                .IsOptional();

            Ignore(p => p.PropertiesXML);

            #endregion
        }
    }
}
