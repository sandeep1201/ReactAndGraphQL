using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DCF.Timelimits.Rules.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DCF.Timelimits.Common.Tests.Specifications
{
    [TestClass]
    public class Specification_Tests
    {
        private readonly IQueryable<TestCustomer> _customers;

        public Specification_Tests()
        {
            _customers = new List<TestCustomer>
                {
                    new TestCustomer("John", 17, 47000, "England"),
                    new TestCustomer("Tuana", 2, 500, "Turkey"),
                    new TestCustomer("Martin", 43, 16000, "USA"),
                    new TestCustomer("Lee", 32, 24502, "China"),
                    new TestCustomer("Douglas", 42, 42000, "England"),
                    new TestCustomer("Abelard", 14, 2332, "German"),
                    new TestCustomer("Neo", 16, 120000, "USA"),
                    new TestCustomer("Daan", 39, 6000, "Netherlands"),
                    new TestCustomer("Alessandro", 22, 8271, "Italy"),
                    new TestCustomer("Noah", 33, 82192, "Belgium")
                }.AsQueryable();
        }

        [TestMethod]
        public void Any_Should_Return_All()
        {
            _customers
                .Where(new AnySpecification<TestCustomer>()) //Implicitly converted to Expression!
                .Count()
                .ShouldBe(_customers.Count());
        }

        [TestMethod]
        public void None_Should_Return_Empty()
        {
            _customers
                .Where(new NoneSpecification<TestCustomer>().ToExpression())
                .Count()
                .ShouldBe(0);
        }

        [TestMethod]
        public void Not_Should_Return_Reverse_Result()
        {
            _customers
                .Where(new EuropeanCustomerSpecification().Not().ToExpression())
                .Count()
                .ShouldBe(3);
        }

        [TestMethod]
        public void Should_Support_Native_Expressions_And_Combinations()
        {
            _customers
                .Where(new ExpressionSpecification<TestCustomer>(c => c.Age >= 18).ToExpression())
                .Count()
                .ShouldBe(6);

            _customers
                .Where(new EuropeanCustomerSpecification().And(new ExpressionSpecification<TestCustomer>(c => c.Age >= 18)).ToExpression())
                .Count()
                .ShouldBe(4);
        }

        [TestMethod]
        public void CustomSpecification_Test()
        {
            _customers
                .Where(new EuropeanCustomerSpecification().ToExpression())
                .Count()
                .ShouldBe(7);

            _customers
                .Where(new Age18PlusCustomerSpecification().ToExpression())
                .Count()
                .ShouldBe(6);

            _customers
                .Where(new BalanceCustomerSpecification(10000, 30000).ToExpression())
                .Count()
                .ShouldBe(2);

            _customers
                .Where(new PremiumCustomerSpecification().ToExpression())
                .Count()
                .ShouldBe(3);
        }

        [TestMethod]
        public void IsSatisfiedBy_Tests()
        {
            new PremiumCustomerSpecification().IsSatisfiedBy(new TestCustomer("David", 49, 55000, "USA")).ShouldBeTrue();

            new PremiumCustomerSpecification().IsSatisfiedBy(new TestCustomer("David", 49, 200, "USA")).ShouldBeFalse();
            new PremiumCustomerSpecification().IsSatisfiedBy(new TestCustomer("David", 12, 55000, "USA")).ShouldBeFalse();
        }

        [TestMethod]
        public void CustomSpecification_Composite_Tests()
        {
            _customers
                .Where(new EuropeanCustomerSpecification().And(new Age18PlusCustomerSpecification()).ToExpression())
                .Count()
                .ShouldBe(4);

            _customers
                .Where(new EuropeanCustomerSpecification().Not().And(new Age18PlusCustomerSpecification()).ToExpression())
                .Count()
                .ShouldBe(2);

            _customers
                .Where(new Age18PlusCustomerSpecification().AndNot(new EuropeanCustomerSpecification()).ToExpression())
                .Count()
                .ShouldBe(2);
        }

        

        private class EuropeanCustomerSpecification : Specification<TestCustomer>
        {
            public override Expression<Func<TestCustomer, bool>> ToExpression()
            {
                return c => c.Location == "England" ||
                            c.Location == "Turkey" ||
                            c.Location == "German" ||
                            c.Location == "Netherlands" ||
                            c.Location == "Italy" ||
                            c.Location == "Belgium";
            }
        }

        private class Age18PlusCustomerSpecification : Specification<TestCustomer>
        {
            public override Expression<Func<TestCustomer, bool>> ToExpression()
            {
                return c => c.Age >= 18;
            }
        }

        private class BalanceCustomerSpecification : Specification<TestCustomer>
        {
            public int MinBalance { get; }

            public int MaxBalance { get; }

            public BalanceCustomerSpecification(int minBalance, int maxBalance)
            {
                MinBalance = minBalance;
                MaxBalance = maxBalance;
            }

            public override Expression<Func<TestCustomer, bool>> ToExpression()
            {
                return c => c.Balance >= MinBalance && c.Balance <= MaxBalance;
            }
        }

        private class PremiumCustomerSpecification : AndSpecification<TestCustomer>
        {
            public PremiumCustomerSpecification()
                : base(new Age18PlusCustomerSpecification(), new BalanceCustomerSpecification(20000, int.MaxValue))
            {
            }
        }
    }
}
