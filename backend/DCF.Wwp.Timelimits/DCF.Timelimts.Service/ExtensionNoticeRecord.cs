using System;
using FileHelpers;

namespace DCF.Timelimts.Service
{
    [FixedLengthRecord]
    public class ExtensionNoticeRecord
    {

        [FieldOrder(100)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Right, '0')]
        public String GEN1_GENERIC_IND;

        [FieldOrder(200)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_MAILING_ADDRESS_TYPE;

        [FieldOrder(300)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_WW_RETURN_ADDRESS;

        [FieldOrder(400)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_CC_RETURN_ADDRESS;

        [FieldOrder(500)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_RETURN_ADDRESS_COUNTY;

        [FieldOrder(600)]
        [FieldFixedLength(4)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_RETURN_ADDRESS_OFFICE;

        [FieldOrder(700)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_WW_AGENCY_CONTACT;

        [FieldOrder(800)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_CC_AGENCY_CONTACT;

        [FieldOrder(900)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INCLUDE_HMONG_SW;

        [FieldOrder(1000)]
        [FieldFixedLength(95)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_FILLER;

        [FieldOrder(1100)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_INDICATOR_CNT;

        [FieldOrder(1200)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_CNT;

        [FieldOrder(1300)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_DATE_CNT;

        [FieldOrder(1400)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_CNT;

        [FieldOrder(1500)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_SHORT_TEXT_CNT;

        [FieldOrder(1600)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LONG_TEXT_CNT;

        [FieldOrder(1700)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_1;

        [FieldOrder(1800)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_2;

        [FieldOrder(1900)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_3;

        [FieldOrder(2000)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_4;

        [FieldOrder(2100)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_5;

        [FieldOrder(2200)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_6;

        [FieldOrder(2300)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_7;

        [FieldOrder(2400)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_8;

        [FieldOrder(2500)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_9;

        [FieldOrder(2600)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_INDICATOR_10;

        [FieldOrder(2700)]
        [FieldFixedLength(7)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_1;

        [FieldOrder(2800)]
        [FieldFixedLength(10)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_1;

        [FieldOrder(2900)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_1;

        [FieldOrder(3000)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_1;

        [FieldOrder(3100)]
        [FieldFixedLength(7)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_2;

        [FieldOrder(3200)]
        [FieldFixedLength(10)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_2;

        [FieldOrder(3300)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_2;

        [FieldOrder(3400)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_2;

        [FieldOrder(3500)]
        [FieldFixedLength(7)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_3;

        [FieldOrder(3600)]
        [FieldFixedLength(10)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_3;

        [FieldOrder(3700)]
        [FieldFixedLength(1)]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_3;

        [FieldOrder(3800)]
        [FieldFixedLength(2)]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_3;

        [FieldOrder(3900)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_4;

        [FieldOrder(4000)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_4;

        [FieldOrder(4100)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_4;

        [FieldOrder(4200)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_4;

        [FieldOrder(4300)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_5;

        [FieldOrder(4400)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_5;

        [FieldOrder(4500)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_5;

        [FieldOrder(4600)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right, '0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_5;

        [FieldOrder(4700)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_6;

        [FieldOrder(4800)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_6;

        [FieldOrder(4900)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_6;

        [FieldOrder(5000)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right, '0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_6;

        [FieldOrder(5100)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_7;

        [FieldOrder(5200)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_7;

        [FieldOrder(5300)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_7;

        [FieldOrder(5400)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right, '0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_7;

        [FieldOrder(5500)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_8;

        [FieldOrder(5600)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_8;

        [FieldOrder(5700)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_8;

        [FieldOrder(5800)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right, '0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_8;

        [FieldOrder(5900)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_9;

        [FieldOrder(6000)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_9;

        [FieldOrder(6100)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_9;

        [FieldOrder(6200)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_9;

        [FieldOrder(6300)]
        [FieldFixedLength(7)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_TYPE_10;

        [FieldOrder(6400)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_KEY_10;

        [FieldOrder(6500)]
        [FieldFixedLength(1)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LOOKUP_FIELD_TYPE_10;

        [FieldOrder(6600)]
        [FieldFixedLength(2)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_LOOKUP_FIELD_NUM_10;

        [FieldOrder(6700)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        public DateTime GEN1_DATE_FIELD_1;

        [FieldOrder(6800)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_2;

        [FieldOrder(6900)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_3;

        [FieldOrder(7000)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_4;

        [FieldOrder(7100)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_5;

        [FieldOrder(7200)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_6;

        [FieldOrder(7300)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_7;

        [FieldOrder(7400)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_8;

        [FieldOrder(7500)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_9;

        [FieldOrder(7600)]
        [FieldFixedLength(10)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_DATE_FIELD_10;

        [FieldOrder(7700)]
        [FieldFixedLength(9)]
        [FieldAlign(AlignMode.Right,'0')]
        [FieldConverter(typeof(PrecisionDecimalConverter))]
        public Decimal? GEN1_NUMERIC_FIELD_1;

        [FieldOrder(7800)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_2;

        [FieldOrder(7900)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_3;

        [FieldOrder(8000)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_4;

        [FieldOrder(8100)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_5;

        [FieldOrder(8200)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_6;

        [FieldOrder(8300)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_7;

        [FieldOrder(8400)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_8;

        [FieldOrder(8500)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_9;

        [FieldOrder(8600)]
        [FieldFixedLength(9)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Right,'0')]
        public Int32? GEN1_NUMERIC_FIELD_10;

        [FieldOrder(8700)]
        [FieldFixedLength(20)]
        [FieldAlign(AlignMode.Right)]
        public String GEN1_SHORT_TEXT_1;

        [FieldOrder(8800)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_2;

        [FieldOrder(8900)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_3;

        [FieldOrder(9000)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_4;

        [FieldOrder(9100)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_5;

        [FieldOrder(9200)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_6;

        [FieldOrder(9300)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_7;

        [FieldOrder(9400)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_8;

        [FieldOrder(9500)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_9;

        [FieldOrder(9600)]
        [FieldFixedLength(20)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_SHORT_TEXT_10;

        [FieldOrder(9700)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_1;

        [FieldOrder(9800)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_2;

        [FieldOrder(9900)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_3;

        [FieldOrder(10000)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_4;

        [FieldOrder(10100)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_5;

        [FieldOrder(10200)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_6;

        [FieldOrder(10300)]
        [FieldFixedLength(240)]
        [FieldValueDiscarded]
        [FieldAlign(AlignMode.Left)]
        public String GEN1_LONG_TEXT_7;


    }

    /// <summary>
    /// Convert a value to a decimal value with two points of precision
    /// </summary>
    public class PrecisionDecimalConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return Convert.ToDecimal(Decimal.Parse(from) / 100);
        }

        public override string FieldToString(object fieldValue)
        {
            return String.Format("{0:N2}", fieldValue).Replace(".", "");
                //((decimal)fieldValue).ToString("#.##").Replace(".", "");
        }
    }
}