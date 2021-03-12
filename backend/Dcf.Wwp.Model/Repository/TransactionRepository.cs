using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ITransactionRepository
    {
        public void NewTransaction(ITransaction transaction)
        {
            _db.Transactions.Add((Transaction) transaction);
        }
    }
}
