﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Data.Sql.Configurations
{
    public abstract class BaseConfig<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseConfig()
        {
            #region Relationships

            // none at this level

            #endregion

            #region Properties

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.RowVersion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnType("timestamp")
                .IsRowVersion()
                .IsRequired();

            #endregion
        }
    }
}
