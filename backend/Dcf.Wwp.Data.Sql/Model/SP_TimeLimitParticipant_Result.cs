using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SP_TimeLimitParticipant_Result
    {
        public int       Id                             { get; set; }
        public decimal?  PinNumber                      { get; set; }
        public string    FirstName                      { get; set; }
        public string    MiddleInitialName              { get; set; }
        public string    LastName                       { get; set; }
        public string    SuffixName                     { get; set; }
        public DateTime? DateOfBirth                    { get; set; }
        public DateTime? DateOfDeath                    { get; set; }
        public string    GenderIndicator                { get; set; }
        public string    AliasResponse                  { get; set; }
        public string    BirthVerificationCode          { get; set; }
        public string    BirthPlaceCode                 { get; set; }
        public string    CitizenshipVerificationCode    { get; set; }
        public string    DCLCitizenshipSwitch           { get; set; }
        public string    DeathVerificationCode          { get; set; }
        public string    LanguageCode                   { get; set; }
        public short?    MaxHistorySequenceNumber       { get; set; }
        public decimal?  PrimarySSNNumber               { get; set; }
        public decimal?  PseudoSSNNumber                { get; set; }
        public string    RaceCode                       { get; set; }
        public DateTime? SSNAppointmentDate             { get; set; }
        public string    SSNAppointmentVerificationCode { get; set; }
        public string    SSNValidatedCode               { get; set; }
        public DateTime? CaresUpdatedDate               { get; set; }
        public string    USCitizenSwitch                { get; set; }
        public string    WorkerAlert1Code               { get; set; }
        public string    WorkerAlert2Code               { get; set; }
        public decimal?  MaidNumber                     { get; set; }
        public string    ChildElsewhereSwitch           { get; set; }
        public string    ChildVerificationCode          { get; set; }
        public string    AmericanIndianIndicator        { get; set; }
        public string    AsianIndicator                 { get; set; }
        public string    BlackIndicator                 { get; set; }
        public string    HispanicIndicator              { get; set; }
        public string    PacificIslanderIndicator       { get; set; }
        public string    WhiteIndicator                 { get; set; }
        public decimal?  MCI_ID                         { get; set; }
        public string    MACitizenVerificationCode      { get; set; }
        public string    TribeChildMemberIndicator      { get; set; }
        public string    TribeChildVerificationCode     { get; set; }
        public string    TribalMemberIndicator          { get; set; }
        public string    TribalMemberVerificationCode   { get; set; }
        public string    DeathDateSourceCode            { get; set; }
        public string    WorkerOverideVerificationCode  { get; set; }
        public string    ConversionProjectDetails       { get; set; }
        public DateTime? ConversionDate                 { get; set; }
        public bool      IsDeleted                      { get; set; }
        public DateTime? CreatedDate                    { get; set; }
        public string    ModifiedBy                     { get; set; }
        public DateTime? ModifiedDate                   { get; set; }
        public byte[]    RowVersion                     { get; set; }
        public bool?     TimeLimitStatus                { get; set; }
        public decimal?  CASENumber                     { get; set; }
    }
}
