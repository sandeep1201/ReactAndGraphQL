using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class OtherDemographicInformationContract
    {

        public bool?        IsInterpreterNeeded   { get; set; }
        public bool?        HasAlias              { get; set; }
        public string       HomeLanguageName      { get; set; }
        public bool?        IsRufugee             { get; set; }
        public DateTime?    MonthOfEntry          { get; set; }
        public string       CountryOfOriginName   { get; set; }
        public bool?        IsTribalMember        { get; set; }
        public int?         TribeId               { get; set; }
        public string       TribeName             { get; set; }
        public string TribeDetails { get; set; }
        public string       CountyOfResidenceName { get; set; }
        //public string       StreetAddress         { get; set; }
        //public string       AptUnit               { get; set; }
        public PlaceAddress HouseholdAddress      { get; set; }
        public PlaceAddress MailingAddress        { get; set; }
        public string       PrimaryPhoneNumber    { get; set; }
        public string       SecondaryPhoneNumber  { get; set; }
        public string       EmailAddress          { get; set; }
        public bool?      HomeLessIndicator { get; set; }

        public static OtherDemographicInformationContract Create(bool? isInterpreterNeeded, bool? hasAlias, string homeLanguageName, bool? isRufugee, DateTime? monthOfEntry, 
                                                   string countryOfOriginName, bool? isTribalMember, int? tribeId, string tribeName, string tribeDetails, string countyOfResidenceName, 
                                                   PlaceAddress householdAddress, PlaceAddress mailingAddress, string primaryPhoneNumber, 
                                                   string secondaryPhoneNumber, string emailAddress, bool? homeLessIndicator)
        {
            var c = new OtherDemographicInformationContract
            {
                IsInterpreterNeeded = isInterpreterNeeded,
                HasAlias = hasAlias,
                HomeLanguageName = homeLanguageName,
                IsRufugee = isRufugee,
                MonthOfEntry = monthOfEntry,
                CountryOfOriginName = countryOfOriginName,
                IsTribalMember = isTribalMember,
                TribeId = tribeId,
                TribeName = tribeName,
                TribeDetails = tribeDetails,
                CountyOfResidenceName = countyOfResidenceName,             
                HouseholdAddress = householdAddress,
                MailingAddress = mailingAddress,
                PrimaryPhoneNumber = primaryPhoneNumber,
                SecondaryPhoneNumber = secondaryPhoneNumber,
                EmailAddress = emailAddress,
                HomeLessIndicator = homeLessIndicator
            };

            return c;
        }

     
    }
}
