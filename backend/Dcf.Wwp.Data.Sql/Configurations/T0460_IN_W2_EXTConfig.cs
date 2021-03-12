using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public class T0460_IN_W2_EXTConfig : EntityTypeConfiguration<T0460_IN_W2_EXT>
    {
        public T0460_IN_W2_EXTConfig()
        {
            #region Relationships

            // none

            #endregion

            #region Properties

            ToTable("T0460_IN_W2_EXT");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.PIN_NUM)
                .HasColumnType("decimal")
                .HasPrecision(10, 0)
                .IsRequired();

            Property(p => p.CLOCK_TYPE_CD)
                .HasColumnType("char")
                .HasMaxLength(4)
                .IsRequired();

            Property(p => p.EXT_SEQ_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.HISTORY_SEQ_NUM)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.AGY_DCSN_CD)
                .HasColumnType("char")
                .HasMaxLength(3)
                .IsRequired();

            Property(p => p.AGY_DCSN_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.BENEFIT_MM)
                .HasColumnName("BENEFIT_MM")
                .HasColumnType("decimal")
                .HasPrecision(6, 0)
                .IsRequired();

            Property(p => p.DELETE_REASON_CD)
                .HasColumnType("char")
                .HasMaxLength(2)
                .IsRequired();

            Property(p => p.EXT_BEG_MM)
                .HasColumnType("decimal")
                .HasPrecision(6, 0)
                .IsRequired();

            Property(p => p.EXT_END_MM)
                .HasColumnType("decimal")
                .HasPrecision(6, 0)
                .IsRequired();

            Property(p => p.EXT_REQ_PRC_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.HISTORY_CD)
                .HasColumnType("smallint")
                .IsRequired();

            Property(p => p.STA_DCSN_CD)
                .HasColumnType("char")
                .HasMaxLength(4)
                .IsRequired();

            Property(p => p.UPDATED_DT)
                .HasColumnType("date")
                .IsRequired();

            Property(p => p.USER_ID)
                .HasColumnName("USER_ID")
                .HasColumnType("char")
                .HasMaxLength(6)
                .IsRequired();

            #endregion
        }
    }
}
