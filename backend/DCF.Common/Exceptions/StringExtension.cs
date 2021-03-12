using System.Globalization;

namespace DCF.Common.Exceptions
{
    public static class StringExtension
    {
        #region Properties

        #endregion

        #region Methods

        public static string ToTitleCaseWithCultureInfo(this string s, string cultureInfo = "en-US")
        {
            var textInfo = new CultureInfo(cultureInfo, false).TextInfo;

            return textInfo.ToTitleCase(s);
        }

        #endregion
    }
}
