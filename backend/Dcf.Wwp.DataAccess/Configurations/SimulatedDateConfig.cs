using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class SimulatedDateConfig : BaseConfig<SimulatedDate>
    {
        public SimulatedDateConfig()
        {
            #region Relationships
            #endregion

            #region Properties

            ToTable("CDOTracking");

            Property(p => p.WUID)
                .HasColumnType("varchar")
                .HasMaxLength(25)
                .IsRequired();

            Property(p => p.StartTimeStamp)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.EndTimeStamp)
                .HasColumnType("datetime")
                .IsOptional();

            Property(p => p.CDODate)
                .HasColumnType("datetime")
                .IsOptional();

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
