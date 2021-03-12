using System;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Model;

namespace Dcf.Wwp.UnitTest.Infrastructure
{
    public class MockUnitOfWork : IUnitOfWork
    {
        public int CommitCalled;

        public int Commit()
        {
            CommitCalled++;
            return 0;
        }

        public CommitStatus CommitWithStatus()
        {
            throw new NotImplementedException();
        }

        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void SetEntityModified<T>(int id) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
