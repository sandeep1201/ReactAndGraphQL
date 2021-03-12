using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.ConnectedServices.Finalist;
using System.Collections.Generic;
using System.Globalization;
using DCF.Common.Logging;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Utils
{
    public class FinalistService : IFinalistService
    {
        #region Properties

        private readonly USPostalAddress _usPostalAddress;
        private readonly ILog            _logger = LogProvider.GetLogger(typeof(FinalistService));

        #endregion

        #region Methods

        public FinalistService(USPostalAddress usPostalAddress)
        {
            _usPostalAddress = usPostalAddress;
        }

        public FinalistAddressContract GetAddress(FinalistAddressContract contract)
        {
            var response = CallFinalistWebSvc(contract.AddressLine1, contract.City, contract.State);

            if (response?.validateUSPostalAddressReturn.errorMsg != null && response.validateUSPostalAddressReturn.errorMsg.Any())
            {
                var errorMsg = new List<string>();

                response.validateUSPostalAddressReturn.errorMsg.ForEach(error => errorMsg.Add(GetErrorMsg($"{error}")));

                //var jsonError = JsonConvert.SerializeObject(errorMsg);
                //_logger.Info($"Finalist Error: {jsonError}");

                contract.ErrorMsg = errorMsg.ToArray();
            }

            if (response != null)
            {
                var addr = new[]
                           {
                               TitleCase(response.validateUSPostalAddressReturn.streetAddress),
                               TitleCase(response.validateUSPostalAddressReturn.unitdes),
                               TitleCase(response.validateUSPostalAddressReturn.aptnmbr)
                           };
                var stAddr = string.Join(" ", addr.Where(i => !string.IsNullOrWhiteSpace(i)));
                contract.FullAddress = $"{stAddr}, {TitleCase(response.validateUSPostalAddressReturn.city)}," +
                                       $" {response.validateUSPostalAddressReturn.state} {response.validateUSPostalAddressReturn.zipCode.Replace(" ", "-")}";
                contract.IsValid = bool.Parse(response.validateUSPostalAddressReturn.valid);
            }

            return contract;
        }

        public FinalistAddressContract GetAnalyzeAddress(FinalistAddressContract contract)
        {
            var response = CallFinalistAnalyzeWebSvc(contract.AddressLine1, contract.City, contract.State);

            if (response?.validateAnalyzeUSPostalAddressReturn.errorMsg != null && response.validateAnalyzeUSPostalAddressReturn.errorMsg.Any())
            {
                var errorMsg = new List<string>();

                response.validateAnalyzeUSPostalAddressReturn.errorMsg.ForEach(error => errorMsg.Add(GetErrorMsg(error)));

                //var jsonError = JsonConvert.SerializeObject(errorMsg);
                //_logger.Info($"Finalist Error: {jsonError}");

                contract.ErrorMsg = errorMsg.ToArray();
            }

            if (response != null)
            {
                var addr = new[]
                           {
                               TitleCase(response.validateAnalyzeUSPostalAddressReturn.streetAddress),
                               TitleCase(response.validateAnalyzeUSPostalAddressReturn.unitdes),
                               TitleCase(response.validateAnalyzeUSPostalAddressReturn.aptnmbr)
                           };
                var stAddr = string.Join(" ", addr.Where(i => !string.IsNullOrWhiteSpace(i)));
                contract.FullAddress = $"{stAddr}, {TitleCase(response.validateAnalyzeUSPostalAddressReturn.city)}," +
                                       $" {response.validateAnalyzeUSPostalAddressReturn.state} {response.validateAnalyzeUSPostalAddressReturn.zipCode.Replace(" ", "-")}";
                contract.IsValid = bool.Parse(response.validateAnalyzeUSPostalAddressReturn.valid);
            }

            return contract;
        }


        private validateUSPostalAddressResponse CallFinalistWebSvc(string mailingAddressLine1, string mailingCity, string mailingState)
        {
            var req = new validateUSPostalAddressRequest
                      {
                          mailingAddressLine1 = mailingAddressLine1,
                          mailingCity         = mailingCity,
                          mailingState        = mailingState
                      };

            var res = _usPostalAddress.validateUSPostalAddress(req);

            return res;
        }

        private validateAnalyzeUSPostalAddressResponse CallFinalistAnalyzeWebSvc(string mailingAddressLine1, string mailingCity, string mailingState)
        {
            var req = new validateAnalyzeUSPostalAddressRequest
                      {
                          mailingAddressLine1 = mailingAddressLine1,
                          mailingCity         = mailingCity,
                          mailingState        = mailingState
                      };

            var res = _usPostalAddress.validateAnalyzeUSPostalAddress(req);

            return res;
        }

        private string GetErrorMsg(string errorMsg)
        {
            switch (errorMsg)
            {
                case "1":        return "A different address was found in finalist.";
                case "09011801": return "Zip Code: Is not the correct length.";
                case "09011802": return "Zip Code: Contains characters other than 0-9.";
                case "09011901": return "Zip Code Plus: Is not the correct length.";
                case "09011902": return "Zip Code Plus: Contains characters other than 0-9.";
                case "09011903": return "Zip Code Plus: A valid Zip Code was not provided.";
                case "09012001": return "State: Is not two characters.";
                case "09012002": return "State: Contains characters other than A-Z.";
                case "09012003": return "State: Is not a valid state value.";
                case "09012101": return "Address(1): Data present but Address(1) is empty.";
                case "09012301": return "Zip Code: Value not found.";
                case "09012303": return "City: Duplicate city name within the state.";
                case "09012304": return "Carrier: Non-deliverable address.";
                case "09012305": return "Carrier: Carrier route not determined. Non-deliverable";
                case "09012306": return "Address(1): Street name could not be matched to the database.";
                case "09012307": return "Address(1): Street building number is out of range for this street.";
                case "09012308": return "Address(1): Valid building number and street name for multiple ZIP codes.";
                case "09012309": return "Address(1): Valid building number range could not be determined for the street name provided.";
                case "09012310": return "Address(1): Direction (North, East) and suffix (Street, Trail) incorrect.";
                case "09012311": return "Address(1): Suffix (Street, Trail) has multiple choices.";
                case "09012312": return "Address(1): Directional (North, East) has multiple choices.";
                case "09012313": return "Address(1): Directional outside cardinal point.";
                case "09012314": return "Address(1): Suffix (Street, Trail) or Directional (North, East) error.";
                case "09012315": return "Address(1): Suffix(Street, Trail)/Directional(North, East) could not be determined.";
                case "09012316": return "Address(1): Street name is an alias matching multiple streets.";
                case "09012317": return "Address(1): Alias street name could not be matched to the database.";
                case "09012318": return "Address(1): Unit designator (Apartment, Suite) is invalid.";
                case "09012319": return "Address(1): Secondary address error. Unexpected content.";
                case "09012320": return "Address(1): Unit (Apartment, Suite) number invalid.";
                case "09012321": return "Address: Address does not match any expected formats. Non-conventional address.";
                case "09012322": return "Address(1): Apartment, Suite number missing.";
                case "09012324": return "City: Non-mailing name. No Post Office.";
                case "09012325": return "Address(1): Suffix (Street, Trail) missing or incorrect.";
                case "09012326": return "Address(1): Direction (North, East) missing or incorrect.";
                case "09012327": return "Address(1): Unit designator (Apartment, Suite) is missing / No unit (Apartment, Suite) number.";
                case "09012328": return "Address(1): Unit (Apartment, Suite) number is missing.";
                case "09012329": return "Address(1): No match on alpha portion of range.";
                case "09012330": return "Address(1): No input range provided.";
                case "09012331": return "City: City value returned-none input.";
                case "09012332": return "City: City name changed-corrected.";
                case "09012333": return "Address(1): Street found using exceptions table.";
                case "09012334": return "Address(1): Street found using phonetics match.";
                case "09012335": return "Address(1): Street failed because of Early Warning System match.";
                case "09012336": return "Address(1): Street found using dual address rules.";
                case "09012337": return "Address(1): Rural Route or Highway Contract Route corrected.";
                case "09012338": return "Address(1): PO box corrected.";
                case "09012339": return "Address(1): General delivery corrected.";
                default:         return "Address is not valid in finalist";
            }
        }

        private string TitleCase (string place)
        {
            var casePlace = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(place.ToLower());

            return casePlace;
        }

        #endregion
    }
}
