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

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.30.0.0")]
    public class SpTimeLimitParticipantReturnModel
    {
        public System.Int32 Id { get; set; }
        public System.Decimal? PinNumber { get; set; }
        public System.String FirstName { get; set; }
        public System.String MiddleInitialName { get; set; }
        public System.String LastName { get; set; }
        public System.String SuffixName { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public System.DateTime? DateOfDeath { get; set; }
        public System.String GenderIndicator { get; set; }
        public System.String AliasResponse { get; set; }
        public System.String BirthVerificationCode { get; set; }
        public System.String BirthPlaceCode { get; set; }
        public System.String CitizenshipVerificationCode { get; set; }
        public System.String DCLCitizenshipSwitch { get; set; }
        public System.String DeathVerificationCode { get; set; }
        public System.String LanguageCode { get; set; }
        public System.Int16? MaxHistorySequenceNumber { get; set; }
        public System.Decimal? PrimarySSNNumber { get; set; }
        public System.Decimal? PseudoSSNNumber { get; set; }
        public System.String RaceCode { get; set; }
        public System.DateTime? SSNAppointmentDate { get; set; }
        public System.String SSNAppointmentVerificationCode { get; set; }
        public System.String SSNValidatedCode { get; set; }
        public System.DateTime? CaresUpdatedDate { get; set; }
        public System.String USCitizenSwitch { get; set; }
        public System.String WorkerAlert1Code { get; set; }
        public System.String WorkerAlert2Code { get; set; }
        public System.Decimal? MaidNumber { get; set; }
        public System.String ChildElsewhereSwitch { get; set; }
        public System.String ChildVerificationCode { get; set; }
        public System.String AmericanIndianIndicator { get; set; }
        public System.String AsianIndicator { get; set; }
        public System.String BlackIndicator { get; set; }
        public System.String HispanicIndicator { get; set; }
        public System.String PacificIslanderIndicator { get; set; }
        public System.String WhiteIndicator { get; set; }
        public System.Decimal? MCI_ID { get; set; }
        public System.String MACitizenVerificationCode { get; set; }
        public System.String TribeChildMemberIndicator { get; set; }
        public System.String TribeChildVerificationCode { get; set; }
        public System.String TribalMemberIndicator { get; set; }
        public System.String TribalMemberVerificationCode { get; set; }
        public System.String DeathDateSourceCode { get; set; }
        public System.String WorkerOverideVerificationCode { get; set; }
        public System.String ConversionProjectDetails { get; set; }
        public System.DateTime? ConversionDate { get; set; }
        public System.Boolean IsDeleted { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.String ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public System.Byte[] RowVersion { get; set; }
        public System.Decimal? CASENumber { get; set; }
    }

}
// </auto-generated>