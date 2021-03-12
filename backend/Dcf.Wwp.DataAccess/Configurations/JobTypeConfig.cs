using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Configurations
{
    public class JobTypeConfig : BaseConfig<JobType>
    {
        public JobTypeConfig()
        {
            #region Relationships

            HasMany(p => p.EmploymentInformations)
                .WithRequired(p => p.JobType)
                .HasForeignKey(p => p.JobTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Properties

            ToTable("JobType");

            Property(p => p.Name)
                .HasColumnType("varchar")
                .IsOptional();

            Property(p => p.IsRequired)
                .HasColumnType("bit")
                .IsOptional();


            Property(p => p.SortOrder)
                .HasColumnType("int")
                .IsOptional();

            Property(p => p.IsDeleted)
                .HasColumnType("bit")
                .IsRequired();

            Property(p => p.IsUsedForEmploymentOfRecord)
                .HasColumnType("bit")
                .IsOptional();

            #endregion
        }
    }
}
