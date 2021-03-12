using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.UnitTest.Infrastructure
{
    public class MockRepositoryBase<T> : IRepository<T> where T : class, new()
    {
        public bool AddHasBeenCalled;
        public bool GetAsQueryableHasBeenCalled;
        public int  AddCount;
        public bool NewHasBeenCalled;
        public bool GetManyHasBeenCalled;
        public bool UpdateHasBeenCalled;
        public bool DeleteHasBeenCalled;
        public bool ExecFunctionHasBeenCalled;
        public bool IsPOPClaim                           = false;
        public bool IsGapBetweenEmploymentsGreaterThan14 = false;
        public T    NewValue;
        public T    AddedValues;

        public T Get(Expression<Func<T, bool>> clause)
        {
            return new T();
        }

        public void Add(T entity)
        {
            AddCount++;
            AddHasBeenCalled = true;
            AddedValues      = entity;
        }

        public void AddOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
        }

        public void Delete(Expression<Func<T, bool>> clause)
        {
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public IList<TC> ExecFunction<TC>(string functionName, Dictionary<string, object> parms, string schemaName = "wwp", bool isTableValuedFunction = false)
        {
            ExecFunctionHasBeenCalled = true;
            if (IsPOPClaim)
            {
                List<int> listWithDate  ;
                if (IsGapBetweenEmploymentsGreaterThan14)
                {
                    listWithDate = new List<int>
                                   {
                                       15
                                   };
                }
                else
                {
                    listWithDate = new List<int>
                                   {
                                       13
                                   };
                }

                return listWithDate as List<TC>;
            }
            else
                if (functionName == "FN_GetComputedBusniessDays")
                {
                    return new List<DateTime> { DateTime.Today } as List<TC>;
                }
                else
                {
                    throw new NotImplementedException();
                }
        }

        public IList<TC> ExecStoredProc<TC>(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp")
        {
            throw new NotImplementedException();
        }

        public void ExecuteRawSql(string statement)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAsQueryable(bool withTracking = true)
        {
            GetAsQueryableHasBeenCalled = true;
            return new List<T>().AsQueryable();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public T GetById(long id)
        {
            throw new NotImplementedException();
        }

        public T GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Database GetDataBase()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> clause)
        {
            GetManyHasBeenCalled = true;
            return new List<T>();
        }

        public Task<List<T>> GetManyAsync(Expression<Func<T, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetManyAsync2(Expression<Func<T, bool>> clause)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetManyWithDetails(Expression<Func<T, bool>> clause, string navProps)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetManyWithDetailsAsync(Expression<Func<T, bool>> clause, string details)
        {
            throw new NotImplementedException();
        }

        public string GetServerName()
        {
            throw new NotImplementedException();
        }

        public int GetStoredProcReturnValue(string storedProcName, Dictionary<string, object> parms, string schemaName = "wwp")
        {
            return default(int);
        }

        public T New()
        {
            NewHasBeenCalled = true;
            NewValue         = new T();

            return NewValue;
        }

        public IEnumerable<T> SqlQuery(string statement)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            UpdateHasBeenCalled = true;
        }
    }
}
