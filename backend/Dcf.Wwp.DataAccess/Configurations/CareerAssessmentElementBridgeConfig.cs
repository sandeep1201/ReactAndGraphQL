using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class CareerAssessmentElementBridgeConfig : BaseConfig<CareerAssessmentElementBridge>
    {
        public CareerAssessmentElementBridgeConfig()
        {
            #region Relationship

            HasRequired(p => p.CareerAssessment)
                .WithMany()
                .HasForeignKey(p => p.CareerAssessmentId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Element)
                .WithMany()
                .HasForeignKey(p => p.ElementId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("CareerAssessmentElementBridge");

            Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();
            Property(p => p.CareerAssessmentId)
                .HasColumnType("int")
                .IsRequired();
            Property(p => p.ElementId)
                .HasColumnType("int")
                .IsRequired();
            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();
            Property(p => p.ModifiedDate)
                .HasColumnType("datetime")
                .IsRequired();
            Property(p => p.ModifiedBy)
                .HasColumnType("varchar")
                .IsRequired();

            #endregion
        }
    }
}
