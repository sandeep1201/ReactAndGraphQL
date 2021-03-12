using System.Text;
using Dcf.Wwp.Api;
using Dcf.Wwp.Api.Library.Contracts;


namespace Dcf.Wwp.TelerikReport.Library
{
    public class EPStockText
    {
        public string GetStockText(string placementType, string type, string beginDate, string endDate, string placement, PrintedEPStockTextConfig stockText)
        {
            var placementCode = "Not Placed";
            var finalString   = "";

            //Placement Code type
            if (placement != null && placement.Equals("CSJ"))
            {
                placementCode = "CommunityService Job (CSJ)";
            }

            var textTypes = new pepStockText();


            //finding and pulling stock text type
            if (!placementType.Equals(""))
            {
                stockText.english.ForEach(c =>
                                          {
                                              if (c.name.Equals(placementType))
                                              {
                                                  textTypes = new pepStockText
                                                              {
                                                                  name = c.name,
                                                                  text = c.text,
                                                              };
                                              }
                                          });

                //finding particular info
                if (type.Equals("plan_title"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("plan"))
                                               {
                                                   finalString = c.detail;
                                               }
                                           });
                }

                if (type.Equals("plan_parag"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("plan"))
                                               {
                                                   finalString = c.paragraph;
                                               }
                                           });
                }

                if (type.Equals("assign_parag"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("assigned"))
                                               {
                                                   finalString = c.paragraph;
                                               }
                                           });
                }

                if (type.Equals("sig_title"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("signature"))
                                               {
                                                   finalString = c.detail;
                                               }
                                           });
                }

                if (type.Equals("sig_detail"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("signature"))
                                               {
                                                   finalString = c.paragraph;
                                               }
                                           });
                }
            }

            //final formatting make sure the [style] is reflected in the json file.
            finalString = finalString.Replace("[beginDate]", beginDate);
            finalString = finalString.Replace("[endDate]",   endDate);
            if (placementType.Equals("W2") && type.Equals("plan_parag"))
            {
                //cannot store + symbol as a name in json
                if (placement != null && placement.Equals("CMF+"))
                {
                    placement = "CMFplus";
                }

                stockText.Placement_Description.ForEach(c =>
                                                                {
                                                                    if (c.name.Equals(placement))
                                                                    {
                                                                        placementCode = c.detail;
                                                                    }
                                                                });
                finalString = finalString.Replace("[placementCode]", placementCode);
            }

            return finalString;
        }

        public string GetStockTextSpanish(string placementType, string type, string beginDate, string endDate, string placement, PrintedEPStockTextConfig stockText)
        {
            var placementCode = "Error - Placement not found";
            var finalString   = "ERROR - String type not found";
            //Placement Code type
            if (placement != null && placement.Equals("CSJ"))
            {
                placementCode = "CommunityService Job (CSJ)";
            }

            var textTypes = new pepStockText();
            //finding and pulling stock text type
            if (!placementType.Equals(""))
            {
                stockText.spanish.ForEach(c =>
                                          {
                                              if (c.name.Equals(placementType))
                                              {
                                                  textTypes = new pepStockText
                                                              {
                                                                  name = c.name,
                                                                  text = c.text,
                                                              };
                                              }
                                          });

                //finding particular info
                if (type.Equals("plan_title"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("plan"))
                                               {
                                                   finalString = c.detail;
                                               }
                                           });
                }

                if (type.Equals("plan_parag"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("plan"))
                                               {
                                                   finalString = c.paragraph;
                                               }
                                           });
                }

                if (type.Equals("assign_parag"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("assigned"))
                                               {
                                                   finalString = c.paragraph;
                                               }
                                           });
                }

                if (type.Equals("sig_title"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("signature"))
                                               {
                                                   finalString = c.detail;
                                               }
                                           });
                }

                if (type.Equals("sig_detail"))
                {
                    textTypes.text.ForEach(c =>
                                           {
                                               if (c.name.Equals("signature"))
                                               {
                                                   finalString = c.paragraph;
                                               }
                                           });
                }
            }

            //final formatting make sure the [style] is reflected in the json file.
            finalString = finalString.Replace("[beginDate]", beginDate);
            finalString = finalString.Replace("[endDate]",   endDate);
            if (placementType.Equals("W2"))
            {
                //cannot store + symbol as a name in json
                if (placement != null && placement.Equals("CMF+"))
                {
                    placement = "CMFplus";
                }

                stockText.Placement_Description.ForEach(c =>
                                                                {
                                                                    if (c.name.Equals(placement))
                                                                    {
                                                                        placementCode = c.detail;
                                                                    }
                                                                });
                finalString = finalString.Replace("[placementCode]", placementCode);
            }

            return finalString;
        }

        public string GetPlacement(EnrolledProgramContract epCon)
        {
            string placementType = "Placement not Found";
            if (epCon.IsW2)
            {
                placementType = "W2";
            }

            if (epCon.IsTmj)
            {
                placementType = "TMJ";
            }

            if (epCon.IsTJ)
            {
                placementType = "TJ";
            }

            if (epCon.IsCF)
            {
                placementType = "Children_First";
            }

            if (epCon.IsLF)
            {
                placementType = "LearnFare";
            }

            return placementType;
        }
    }
}
