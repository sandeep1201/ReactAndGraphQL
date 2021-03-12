// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCF.Core.Domain
{

    // ApprovalReason
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
    public class ApprovalReasonConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ApprovalReason>
    {
        public ApprovalReasonConfiguration()
            : this("wwp")
        {
        }

        public ApprovalReasonConfiguration(string schema)
        {
            ToTable("ApprovalReason", schema);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("varchar").IsOptional().IsUnicode(false);
            Property(x => x.IsDeleted).HasColumnName(@"IsDeleted").HasColumnType("bit");
            Property(x => x.CreatedDate).HasColumnName(@"CreatedDate").HasColumnType("datetime").IsOptional();
            Property(x => x.ModifiedBy).HasColumnName(@"ModifiedBy").HasColumnType("varchar").IsUnicode(false);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").HasColumnType("datetime").IsOptional();
            Property(x => x.RowVersion).HasColumnName(@"RowVersion").HasColumnType("timestamp").IsFixedLength().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }

}
// </auto-generated>