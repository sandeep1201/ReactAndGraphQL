using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class WebPerformanceConfig : EntityTypeConfiguration<WebPerformance>
    {
        public WebPerformanceConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("WebPerformance");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.MethodName)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.StartTime)
                .HasColumnType("datetime2")
                .IsRequired();

            Property(p => p.StopTime)
                .HasColumnType("datetime2")
                .IsRequired();

            Property(p => p.Elapsed)
                .HasColumnType("time")
                .IsRequired();

            Property(p => p.Cached)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Web)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Retries)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Total)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.UserId)
                .HasColumnType("varchar")
                .HasMaxLength(20)
                .IsRequired();

            #endregion
        }
    }
}
