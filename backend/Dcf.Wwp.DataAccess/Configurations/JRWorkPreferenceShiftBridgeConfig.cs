using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JRWorkPreferenceShiftBridgeConfig : BaseConfig<JRWorkPreferenceShiftBridge>
    {
        public JRWorkPreferenceShiftBridgeConfig()
        {
            #region Relationships

            HasRequired(p => p.JrWorkPreferences)
                .WithMany(p => p.JrWorkPreferenceShiftBridges)
                .HasForeignKey(p => p.WorkPreferenceId);

            HasRequired(p => p.JrWorkShift)
                .WithMany()
                .HasForeignKey(p => p.WorkShiftId);

            #endregion

            #region Properties

            ToTable("JRWorkPreferenceShiftBridge");

            Property(p => p.WorkPreferenceId)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.WorkShiftId)
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
