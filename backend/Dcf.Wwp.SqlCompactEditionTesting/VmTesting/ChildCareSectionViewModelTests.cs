using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Cww;
using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.Web.Controllers;
using Dcf.Wwp.Web.ViewModels;
using Dcf.Wwp.Web.ViewModels.Contracts;
using Dcf.Wwp.Web.ViewModels.Contracts.CWW;
using Dcf.Wwp.Web.ViewModels.InformalAssessment;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
    public class ChildCareSectionViewModelTests : BaseUnitTest
    {
        #region Upsert 

        [Fact]
        public void SimpleUpsertChildCareSection_SaveChildCareData_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithChildCareSection(Db);
            IRepository repo = new Repository(Db);
            BaseViewModel.InjectDependencies(repo);

            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareData();
            var ia = new InformalAssessmentData();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ia.ChildCareData = ccd;
            ia.ChildCareData.Children = children;
            ia.ChildCareData.Teen = teens;

            for (int i = 0; i < 1000; i++)
            {
                var x = new ChildCareContract();
                x.Name = i.ToString();
                //Odd
                if (i % 2 != 0)
                {
                    x.IsSpecialNeeds = true;
                }
                else
                {
                    x.IsSpecialNeeds = false;
                }
                children.Add(x);
            }

            for (int i = 0; i < 1000; i++)
            {
                var y = new TeenCareContract();
                y.Name = i.ToString();
                //Odd               

                teens.Add(y);
            }

            ccd.HasChildren = true;
            ccd.Children = children;
            ccd.HasChildrenWithDisabilityInNeedOfChildCare = true;
            ccd.Teen = teens;

            for (int i = 0; i < 999; i++)
            {
                ccd.AssistDetails += "x";
            }

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = Db.ChildCareSections.SingleOrDefault(cc => cc.Id == 1).RowVersion;
            ccd.RowVersion = version;
            var dbData2 = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            // Act - Controller.
            var HasUpserted = ChildCareSectionViewModel.UpsertData(repo, "123", ccd, "test");

            if (HasUpserted == false)
                Assert.False(true);


            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault(); ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }

            Assert.True(dbData?.Notes == longString);
            Assert.True(dbData?.ActionDetails == longString);
            Assert.True(dbData?.Notes == longString);
            Assert.True(dbData?.ActionDetails == longString);
            Assert.True(dbData?.ChildCareChilds.Count == 2000);
            Assert.True(dbData?.HasChildren12OrUnder == true);


        }
        [Fact]
        public void SimpleUpsertChildCareSection_SaveChildCareDataWithOutChildCareSection_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithNoSections(Db);
            IRepository repo = new Repository(Db);
            BaseViewModel.InjectDependencies(repo);

            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareData();
            var ia = new InformalAssessmentData();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ia.ChildCareData = ccd;
            ia.ChildCareData.Children = children;
            ia.ChildCareData.Teen = teens;

            for (int i = 0; i < 1000; i++)
            {
                var x = new ChildCareContract();
                x.Name = i.ToString();
                //Odd
                if (i % 2 != 0)
                {
                    x.IsSpecialNeeds = true;
                }
                else
                {
                    x.IsSpecialNeeds = false;
                }
                children.Add(x);
            }

            for (int i = 0; i < 1000; i++)
            {
                var y = new TeenCareContract();
                y.Name = i.ToString();
                //Odd               

                teens.Add(y);
            }

            ccd.HasChildren = true;
            ccd.Children = children;
            ccd.HasChildrenWithDisabilityInNeedOfChildCare = true;
            ccd.Teen = teens;
            for (int i = 0; i < 999; i++)
            {
                ccd.AssistDetails += "x";
            }

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = new byte[2] { 1, 2 };
            ccd.RowVersion = version;

            var HasUpserted = ChildCareSectionViewModel.UpsertData(repo, "123", ccd, "test");

            if (HasUpserted == false)
                Assert.False(true);


            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault(); ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }

            Assert.True(dbData?.Notes == longString);
            Assert.True(dbData?.ActionDetails == longString);
            Assert.True(dbData?.Notes == longString);
            Assert.True(dbData?.ActionDetails == longString);
            Assert.True(dbData?.ChildCareChilds.Count == 2000);
            Assert.True(dbData?.HasChildren12OrUnder == true);


        }
        [Fact]
        public void SimpleUpsertChildCareSection_WithoutOutChildren_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithChildCareSection(Db);
            IRepository repo = new Repository(Db);
            BaseViewModel.InjectDependencies(repo);

            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareData();
            var ia = new InformalAssessmentData();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ia.ChildCareData = ccd;
            ia.ChildCareData.Children = children;
            ia.ChildCareData.Teen = teens;

            for (int i = 0; i < 100; i++)
            {
                var y = new TeenCareContract();
                y.Name = i.ToString();
                //Odd               

                teens.Add(y);
            }

            ccd.HasChildren = false;
            ccd.HasChildrenWithDisabilityInNeedOfChildCare = true;

            //ccd.Children = children;
            ccd.Teen = teens;


            for (int i = 0; i < 999; i++)
            {
                ccd.AssistDetails += "x";
            }

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = Db.ChildCareSections.SingleOrDefault(cc => cc.Id == 1).RowVersion;
            ccd.RowVersion = version;
            var dbData2 = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            // Act - Controller.
            var HasUpserted = ChildCareSectionViewModel.UpsertData(repo, "123", ccd, "test");

            if (HasUpserted == false)
                Assert.False(true);


            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault(); ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }

            Assert.True(dbData?.Notes == longString);
            Assert.True(dbData?.ActionDetails == longString);
            Assert.True(dbData?.ChildCareChilds.Count == 100);
            Assert.True(dbData?.HasChildren12OrUnder == false);
            Assert.True(dbData?.HasChildrenWithDisabilityInNeedOfChildCare == true);

        }
        [Fact]
        public void SimpleUpsertChildCareSection_WithoutOutTeens_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithChildCareSection(Db);
            IRepository repo = new Repository(Db);
            BaseViewModel.InjectDependencies(repo);

            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareData();
            var ia = new InformalAssessmentData();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ia.ChildCareData = ccd;
            ia.ChildCareData.Children = children;
            ia.ChildCareData.Teen = teens;

            for (int i = 0; i < 1000; i++)
            {
                var x = new ChildCareContract();
                x.Name = i.ToString();
                //Odd
                if (i % 2 != 0)
                {
                    x.IsSpecialNeeds = true;
                }
                else
                {
                    x.IsSpecialNeeds = false;
                }
                children.Add(x);
            }

            ccd.HasChildren = true;
            ccd.HasChildrenWithDisabilityInNeedOfChildCare = false;

            ccd.Children = children;


            for (int i = 0; i < 999; i++)
            {
                ccd.AssistDetails += "x";
            }

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = Db.ChildCareSections.SingleOrDefault(cc => cc.Id == 1).RowVersion;
            ccd.RowVersion = version;
            var dbData2 = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            // Act - Controller.
            var HasUpserted = ChildCareSectionViewModel.UpsertData(repo, "123", ccd, "test");

            if (HasUpserted == false)
                Assert.False(true);

            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault(); ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }
            Assert.True(dbData?.Notes == longString);
			Assert.True(dbData?.ActionDetails == longString);
			Assert.True(dbData?.ChildCareChilds.Count == 1000);
            Assert.True(dbData?.HasChildren12OrUnder == true);
            Assert.True(dbData?.HasChildrenWithDisabilityInNeedOfChildCare == false);
        }
        #endregion


        #region Get
        [Fact]
        public void GetChildCareData_SuccessfulGet()
        {
            //Arrange
            // Mock CWW Data
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.CwwCurrentChildren("300", "123")).Returns(new List<ICurrentChild>()
            {
               new CurrentChild() {LastName = "sohi"},
               new CurrentChild() {FirstName = "Reddy"},
               new CurrentChild() {Relationship = "brother"}
                
            });
            
            VMTest.IaWithHeavyChildCareSection(Db);
            IRepository repo = new Repository(Db);
            ILogger<ApiController> logger = null;
            BaseViewModel.InjectDependencies(repo);
          
            // Act - Controller.
           var ia = repo.InformalAssessmentById(1);
            var test = ChildCareSectionViewModel.GetData(ia, mockRepo.Object, "123", 300m);

            //Assert

            Assert.True(test.HasChildren.HasValue);
            Assert.True(test.HasChildrenWithDisabilityInNeedOfChildCare.HasValue);
            Assert.True(test.Children.Count.Equals(0));
            Assert.True(test.Teen.Count.Equals(2));


        }

        #endregion
    }
}
