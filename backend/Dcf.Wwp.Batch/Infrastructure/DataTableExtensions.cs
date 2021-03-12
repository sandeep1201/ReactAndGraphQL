using System.Data;

namespace Dcf.Wwp.Batch.Infrastructure
{
    public static class DataTableExtensions
    {
        #region Properties

        #endregion

        #region Methods

        //public static IEnumerable<DataRow> AsEnumerable(this DataTable table)
        //{
        //    for (var i = 0; i < table.Rows.Count; i++)
        //    {
        //        yield return table.Rows[i];
        //    }
        //}

        public static bool HasColumn(this DataTable dt, string columnName)
        {
            var exists  = false;
            var colName = columnName.ToLower();

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToLower() == colName)
                {
                    exists = true;
                    break;
                }
            }

            return (exists);
        }

        #endregion
    }
}
