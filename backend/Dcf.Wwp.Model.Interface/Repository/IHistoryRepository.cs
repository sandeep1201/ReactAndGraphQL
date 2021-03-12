using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IHistoryRepository
    {
        string SectionHistory(string storedProcedureName, string tableName, string pin, int? id);
    }
}