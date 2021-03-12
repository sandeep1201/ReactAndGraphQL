using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IHistoryRepository
    {
        public string SectionHistory(string storedProcedureName, string tableName, string pin, int? id)
        {
            return _db.SPReadCDCHistory(storedProcedureName, tableName, pin, id);
        }
    }
}