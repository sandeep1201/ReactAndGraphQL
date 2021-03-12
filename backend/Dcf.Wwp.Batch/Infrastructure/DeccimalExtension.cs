using System;
using System.Collections.Generic;
using System.Linq;
using Fclp.Internals.Extensions;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public static class DecimalExtensions
    {
        public static decimal DecimalValue1Scale(this decimal? i)
        {
            return i != null ? decimal.Parse(i?.ToString("F1")) : 0.0m;
        }

        public static decimal DecimalValue2Scale(this decimal? i)
        {
            return i != null ? decimal.Parse(i?.ToString("F2")) : 0.00m;
        }

        public static decimal SumDecimal(this List<decimal?> decimalList)
        {
            var addedDecimal = 0.00m;

            decimalList.ForEach(i => addedDecimal += i.GetValueOrDefault());

            return addedDecimal;
        }

        public static decimal SubtractDecimal(this List<decimal?> decimalList)
        {
            var subtractedDecimal = decimalList[0].GetValueOrDefault();

            decimalList.RemoveAt(0);
            decimalList.ForEach(i => subtractedDecimal -= i.GetValueOrDefault());

            return subtractedDecimal;
        }
    }
}
