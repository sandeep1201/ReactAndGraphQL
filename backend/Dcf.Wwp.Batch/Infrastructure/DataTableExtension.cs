using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public static class DataTableExtension
    {
        #region Properties

        #endregion

        #region Methods

        public static IEnumerable<T> ConvertDataTable<T>(this DataTable dt, DataTable dataTable = null)
        {
            var data = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                var item = GetItem<T>(row);

                data.Add(item);
                dataTable?.ImportRow(row);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            var temp = typeof(T);
            var obj  = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties().Where(pro => pro.Name == column.ColumnName))
                {
                    pro.SetValue(obj, dr[column.ColumnName] != DBNull.Value ? dr[column.ColumnName] : null, null);
                }
            }

            return obj;
        }

        #endregion
    }
}
