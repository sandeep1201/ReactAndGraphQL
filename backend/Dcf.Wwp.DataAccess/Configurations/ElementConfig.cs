using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class ElementConfig : BaseConfig<Element>
    {
        public ElementConfig()
        {
            #region Properties

            ToTable("Element");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();
            Property(p => p.Name)
                .HasColumnType("varchar")
                .IsRequired();
            Property(p => p.EffectiveDate)
                .HasColumnType("datetime")
                .IsRequired();
            Property(p => p.EndDate)
                .HasColumnType("datetime")
                .IsOptional();
            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();
            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();
            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();

            #endregion
        }
    }
}
