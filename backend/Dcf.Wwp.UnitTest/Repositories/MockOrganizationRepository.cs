using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.UnitTest.Infrastructure;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockOrganizationRepository : MockRepositoryBase<Organization>, IOrganizationRepository
    {
        #region Properties

        public bool IsPOPClaim = false;

        #endregion

        #region Methods

        public new Organization Get(Expression<Func<Organization, bool>> clause)
        {
            var organization = new Organization();

            if (IsPOPClaim)
                organization.Id = 1;

            return organization;
        }

        public new IQueryable<Organization> GetAsQueryable(bool withTracking = true)
        {
            var organization = new Organization
            {
                Id = 6
            };
            var organizations = new List<Organization> { organization };
            return organizations.AsQueryable();
        }

        #endregion
    }
}
