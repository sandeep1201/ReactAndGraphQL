using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ClientRegistrationContract
    {
        #region Properties

        public PersonNameContract       Name                            { get; set; }
        public DateTime                 DateOfBirth                     { get; set; }
        public string                   GenderIndicator                 { get; set; }
        public string                   Ssn                             { get; set; }
        public bool?                    IsNoSsn                         { get; set; }
        public string                   SsnVerificationCode             { get; set; }
        public string                   SsnVerificationCodeDescription  { get; set; }
        public decimal?                 MciId                           { get; set; }
        public decimal?                 PinNumber                       { get; set; }
        public List<PersonNameContract> Aliases                         { get; set; }
        public List<PersonNameContract> DeletedAliases                  { get; set; }
        public List<SsnAliasContract>   AltSsns                         { get; set; }
        public bool?                    IsPinConfidential               { get; set; }
        public RaceEthnicityContract    RaceEthnicity                   { get; set; }
        public PhoneContract            PrimaryPhone                    { get; set; }
        public PhoneContract            SecondaryPhone                  { get; set; }
        public int?                     HomeLanguageId                  { get; set; }
        public bool?                    IsInterpreterNeeded             { get; set; }
        public string                   InterpreterDetails              { get; set; }
        public bool?                    IsRefugee                       { get; set; }
        public int?                     CountryOfOriginId               { get; set; }
        public DateTime?                EntryDate                       { get; set; }
        public bool?                    RefugeeEntryDateUnknown         { get; set; }
        public bool?                    IsInTribe                       { get; set; }
        public int?                     TribeId                         { get; set; }
        public string                   TribalDetails                   { get; set; }
        public string                   EmailAddress                    { get; set; }
        public int?                     CountyOfResidenceId             { get; set; }
        public bool?                    IsHomeless                      { get; set; }
        public FinalistAddressContract  HouseholdAddress                { get; set; }
        public bool?                    IsMailingSameAsHouseholdAddress { get; set; }
        public FinalistAddressContract  MailingAddress                  { get; set; }
        public string                   Notes                           { get; set; }
        public string                   EaPin                           { get; set; }
        public int?                     EaRequestId                     { get; set; }
        public bool?                    InEAMode                        { get; set; }

        #endregion

        #region Methods

        public ClientRegistrationContract()
        {
            Name           = new PersonNameContract();
            RaceEthnicity  = new RaceEthnicityContract();
            PrimaryPhone   = new PhoneContract();
            SecondaryPhone = new PhoneContract();
            Aliases        = new List<PersonNameContract>();
            AltSsns        = new List<SsnAliasContract>();
        }

        #endregion
    }
}
