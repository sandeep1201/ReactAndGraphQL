using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Timelimits.Common.Tests.Specifications;
using DCF.Timelimits.Rules.Actions;
using DCF.Timelimits.Rules.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace DCF.Timelimits.Common.Tests.Operations
{

    [TestClass]
    public class OperationTests
    {
        private TestCustomer customer;
        private Mock<TestCustomer> _mockObj;
        private readonly List<TestCustomer> _customers = new List<TestCustomer>();

        [TestInitialize]
        public void TestInit()
        {
            var mockObj = new Mock<TestCustomer>();
            mockObj.SetupAllProperties();
            this.customer = mockObj.Object;
            this._mockObj = mockObj;
        }

        [TestMethod]
        public void Should_Compile_Single_Action()
        {
            var _nativeExpressionOperation = new ExpressionOperation<Object>(c => new Object());
            _nativeExpressionOperation.ToExpression().Compile().Invoke(this.customer);
        }

        [TestMethod]
        public void Should_Support_Native_Expression()
        {
            var SetCustmerNameOperation = new ExpressionOperation<TestCustomer>( c => c.Name = "Terry");
    
            SetCustmerNameOperation.ToExpression().Compile().Invoke(this.customer);
            this.customer.Name.ShouldBe("Terry");
        }

        [TestMethod]
        public void Should_Support_Native_Expression_Combination()
        {
            var setCustomerNameOperation = new ExpressionOperation<TestCustomer>(c => c.Name = "Terry");
            var setCustomerAgeOperation = new ExpressionOperation<TestCustomer>(c => c.Age = 31);
            var setCustomerBalance = new ExpressionOperation<TestCustomer>(c => c.Balance = 5000);

            var agg1 = setCustomerNameOperation.And(setCustomerAgeOperation);
            var agg2 =  agg1.And(setCustomerBalance);

            agg1.ToExpression().Compile().Invoke(this.customer);
            this.customer.Name.ShouldBe("Terry");
            this.customer.Age.ShouldBe((Byte)31);
            this.customer.Balance.ShouldBe(default(Int64));

            agg2.ToExpression().Compile().Invoke(this.customer);
            this.customer.Name.ShouldBe("Terry");
            this.customer.Age.ShouldBe((Byte)31);
            this.customer.Balance.ShouldBe(5000);
        }

    }
}