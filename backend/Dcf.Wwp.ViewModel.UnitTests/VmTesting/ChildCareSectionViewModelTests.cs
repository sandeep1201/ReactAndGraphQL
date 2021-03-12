using Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Api.Library.InformalAssessment;
using Dcf.Wwp.Api.Library.ViewModels.InformalAssessment;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Cww;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.ViewModel.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dcf.Wwp.SqlCompactEditionTesting.VmTesting
{
    [TestClass]
    public class ChildCareSectionViewModelTests : BaseUnitTest
    {
        #region Upsert 

        [TestMethod]
        public void SimpleUpsertChildCareSection_SaveChildCareData_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithChildCareSection(Db);
            IRepository repo = new Repository(Db);

            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareSectionContract();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ccd.Children = children;
            ccd.Teens = teens;

            for (int i = 0; i < 1000; i++)
            {
                var x = new ChildCareContract();
                x.Name = i.ToString();
                //Odd
                if (i%2 != 0)
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
            ccd.Teens = teens;
            string assistdetails = null;
            var actionneededtypes = new List<int> { 1, 2, 3, 4, 5, 6 };
            var actionneeded = new ActionNeededTypeContract();
            
            actionneeded.ActionNeededTypes = actionneededtypes;
            ccd.ActionNeeded = actionneeded;
            for (int i = 0; i < 999; i++)
            {
                
                assistdetails += "x";
            }
            actionneeded.AssistDetails = assistdetails;

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = Db.ChildCareSections.SingleOrDefault(cc => cc.Id == 1).RowVersion;
            ccd.RowVersion = version;
            var dbData2 = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            // Act - Controller.
            // Act - Controller
            var vm = new ChildCareSectionViewModel(repo);
            vm.InitializeFromPin("123");
            var hasUpserted = vm.PostData("123", ccd, "Ahsan");
            if (hasUpserted == false)
                Assert.IsFalse(true);


            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }

            Assert.IsTrue(dbData?.Notes == longString);
            Assert.IsTrue(dbData?.ActionDetails == longString);
            Assert.IsTrue(dbData?.Notes == longString);
            Assert.IsTrue(dbData?.ActionDetails == longString);
            Assert.IsTrue(dbData?.ChildCareChilds.Count == 2000);
            Assert.IsTrue(dbData?.ChildCareActionBridges.Count == 6);
            Assert.IsTrue(dbData?.HasChildren12OrUnder == true);


        }

        [TestMethod]
        public void SimpleUpsertChildCareSection_SaveChildCareDataWithOutChildCareSection_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithNoSections(Db);
            IRepository repo = new Repository(Db);


            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareSectionContract();

            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ccd.Children = children;
            ccd.Teens = teens;

            for (int i = 0; i < 1000; i++)
            {
                var x = new ChildCareContract();
                x.Name = i.ToString();
                //Odd
                if (i%2 != 0)
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
            ccd.Teens = teens;
            var actionneededtypes = new List<int> {1, 2, 3, 4, 5, 6};
            string assistdetails = null;
            var actionneeded = new ActionNeededTypeContract();
            actionneeded.ActionNeededTypes = actionneededtypes;
            ccd.ActionNeeded = actionneeded;
            for (int i = 0; i < 999; i++)
            {
                assistdetails += "x"; ;
            }
            actionneeded.AssistDetails = assistdetails;

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = new byte[2] {1, 2};
            ccd.RowVersion = version;
            var vm = new ChildCareSectionViewModel(repo);

            var hasUpserted = vm.PostData("123", ccd, "Ahsan");
            if (hasUpserted == false)
                Assert.IsFalse(true);

            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }

            Assert.IsTrue(dbData?.Notes == longString);
            Assert.IsTrue(dbData?.ChildCareActionBridges.Count == 6);
            Assert.IsTrue(dbData?.ActionDetails == longString);
            Assert.IsTrue(dbData?.Notes == longString);
            Assert.IsTrue(dbData?.ChildCareChilds.Count == 2000);
            Assert.IsTrue(dbData?.HasChildren12OrUnder == true);


        }

        [TestMethod]
        public void SimpleUpsertChildCareSection_WithoutOutChildren_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithChildCareSection(Db);
            IRepository repo = new Repository(Db);


            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareSectionContract();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ccd.Children = children;
            ccd.Teens = teens;

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
            ccd.Teens = teens;

            var actionneededtypes = new List<int> { 1, 2, 3, 4, 5, 6 };
            string assistdetails = null;
            var actionneeded = new ActionNeededTypeContract();
            actionneeded.ActionNeededTypes = actionneededtypes;
            ccd.ActionNeeded = actionneeded;
            for (int i = 0; i < 999; i++)
            {
                assistdetails += "x"; ;
            }
            actionneeded.AssistDetails = assistdetails;

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = Db.ChildCareSections.SingleOrDefault(cc => cc.Id == 1).RowVersion;
            ccd.RowVersion = version;
            var dbData2 = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            // Act - Controller.
            var vm = new ChildCareSectionViewModel(repo);

            var hasUpserted = vm.PostData("123", ccd, "Ahsan");
            if (hasUpserted == false)
                Assert.IsFalse(true);

            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault(); ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }

            Assert.IsTrue(dbData?.Notes == longString);
            Assert.IsTrue(dbData?.ActionDetails == longString);
            Assert.IsTrue(dbData?.ChildCareChilds.Count == 100);
            Assert.IsTrue(dbData?.HasChildren12OrUnder == false);
            Assert.IsTrue(dbData?.HasChildrenWithDisabilityInNeedOfChildCare == true);

        }
        [TestMethod]
        public void SimpleUpsertChildCareSection_WithoutOutTeens_SuccessfulSave()
        {
            // Set up in Memory Database.
            VMTest.IaWithChildCareSection(Db);
            IRepository repo = new Repository(Db);


            // Act - Simuate API POST call with JSON data binding.
            var ccd = new ChildCareSectionContract();
            var children = new List<ChildCareContract>();
            var teens = new List<TeenCareContract>();
            ccd.Children = children;
            ccd.Teens = teens;

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


             var actionneededtypes = new List<int> { 1, 2, 3, 4, 5, 6 };
            string assistdetails = null;
            var actionneeded = new ActionNeededTypeContract();
            actionneeded.ActionNeededTypes = actionneededtypes;
            ccd.ActionNeeded = actionneeded;
            for (int i = 0; i < 999; i++)
            {
                assistdetails += "x"; ;
            }
            actionneeded.AssistDetails = assistdetails;

            for (int i = 0; i < 999; i++)
            {
                ccd.Notes += "x";
            }

            var version = Db.ChildCareSections.SingleOrDefault(cc => cc.Id == 1).RowVersion;
            ccd.RowVersion = version;
            var dbData2 = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault();
            // Act - Controller.
            var vm = new ChildCareSectionViewModel(repo);

            var hasUpserted = vm.PostData("123", ccd, "Ahsan");
            if (hasUpserted == false)
                Assert.IsFalse(true);

            // Assert.
            var dbData = (from ccs in Db.ChildCareSections where ccs.Id == 1 select ccs).SingleOrDefault(); ;
            String longString = null;
            for (int i = 0; i < 999; i++)
            {
                longString += "x";
            }
            Assert.IsTrue(dbData?.Notes == longString);
            Assert.IsTrue(dbData?.ActionDetails == longString);
            Assert.IsTrue(dbData?.ChildCareChilds.Count == 1000);
            Assert.IsTrue(dbData?.HasChildren12OrUnder == true);
            Assert.IsTrue(dbData?.HasChildrenWithDisabilityInNeedOfChildCare == false);
        }
        #endregion


        #region Get
        [TestMethod]
        public void GetChildCareData_WithSingleChildAndNoTeens_SuccessfulGet()
        {
            //Arrange
            // Mock CWW Data
            //VMTest.IaWithHeavyChildCareSection(Db);
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.CwwCurrentChildren("300")).Returns(new List<ICurrentChild>()
                 {
                    new CurrentChild() {LastName = "sohi"},
                    new CurrentChild() {FirstName = "Reddy"},
                    new CurrentChild() {Relationship = "brother"}

                 });
            mockRepo.Setup(x => x.ParticipantByPin("300")).Returns(new Participant()
            {
                PinNumber = 300,
                FirstName = "Dinesh",
                LastName = "cheedu",
                InformalAssessments = new List<InformalAssessment>()
                {
                    new InformalAssessment()
                    {

                ParticipantId = 1,
                PinNumber = 300,
                ChildCareSection = new ChildCareSection()
                {
                    HasChildren12OrUnder = true,
                    ChildCareChilds = new List<ChildCareChild>()
                   {
                       new ChildCareChild()
                       {
                           Name = "sohinder",
                           DateOfBirth = DateTime.Parse("05/19/1986"),
                           AgeCategoryId = 1,
                           IsDeleted = false
                       }
                   },

                    HasChildrenWithDisabilityInNeedOfChildCare = true,
                    HasFutureChildCareNeed = true,
                    FutureChildCareNeedNotes = "Notes",
                }
                    }
         
                }
            });
                                        
            var vm = new ChildCareSectionViewModel(mockRepo.Object);
            vm.InitializeFromPin("300");
            var test = vm.GetData("300", 300m);

            //Assert

            Assert.IsTrue(test.HasChildren.HasValue);
            Assert.IsTrue(test.HasChildrenWithDisabilityInNeedOfChildCare.HasValue);
            Assert.IsTrue(test.Children.Count.Equals(1));
            Assert.IsTrue(test.Teens.Count.Equals(0));
        }

        [TestMethod]
        public void GetChildCareData_WithNoChildAndSingleTeen_SuccessfulGet()
        {
            //Arrange
            // Mock CWW Data
            //VMTest.IaWithHeavyChildCareSection(Db);
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.CwwCurrentChildren("300")).Returns(new List<ICurrentChild>()
                 {
                    new CurrentChild() {LastName = "sohi"},
                    new CurrentChild() {FirstName = "Reddy"},
                    new CurrentChild() {Relationship = "brother"}

                 });
            mockRepo.Setup(x => x.ParticipantByPin("300")).Returns(new Participant()
            {
                PinNumber = 300,
                FirstName = "Dinesh",
                LastName = "cheedu",
                InformalAssessments = new List<InformalAssessment>()
                {
                    new InformalAssessment()
                    {

                ParticipantId = 1,
                PinNumber = 300,
                ChildCareSection = new ChildCareSection()
                {
                    HasChildren12OrUnder = true,
                    ChildCareChilds = new List<ChildCareChild>()
                   {
                       new ChildCareChild()
                       {
                           Name = "sohinder",
                           DateOfBirth = DateTime.Parse("05/19/1986"),
                           AgeCategoryId = 2,
                           IsDeleted = false
                       }
                   },

                    HasChildrenWithDisabilityInNeedOfChildCare = true,
                    HasFutureChildCareNeed = true,
                    FutureChildCareNeedNotes = "Notes",
                }
                    }

                }
            });

            var vm = new ChildCareSectionViewModel(mockRepo.Object);
            vm.InitializeFromPin("300");
            var test = vm.GetData("300", 300m);

            //Assert

            Assert.IsTrue(test.HasChildren.HasValue);
            Assert.IsTrue(test.HasChildrenWithDisabilityInNeedOfChildCare.HasValue);
            Assert.IsTrue(test.Children.Count.Equals(0));
            Assert.IsTrue(test.Teens.Count.Equals(1));
        }
        [TestMethod]
        public void GetChildCareData_WithMultipleChildAndTeen_SuccessfulGet()
        {
            //Arrange
            // Mock CWW Data
            //VMTest.IaWithHeavyChildCareSection(Db);
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.CwwCurrentChildren("300")).Returns(new List<ICurrentChild>()
                 {
                    new CurrentChild() {LastName = "sohi"},
                    new CurrentChild() {FirstName = "Reddy"},
                    new CurrentChild() {Relationship = "brother"}

                 });
            mockRepo.Setup(x => x.ParticipantByPin("300")).Returns(new Participant()
            {
                PinNumber = 300,
                FirstName = "Dinesh",
                LastName = "cheedu",
                InformalAssessments = new List<InformalAssessment>()
                {
                    new InformalAssessment()
                    {

                ParticipantId = 1,
                PinNumber = 300,
                ChildCareSection = new ChildCareSection()
                {
                    HasChildren12OrUnder = true,
                    ChildCareChilds = new List<ChildCareChild>()
                   {
                       new ChildCareChild()
                       {
                           Name = "sohinder",
                           DateOfBirth = DateTime.Parse("05/19/1986"),
                           AgeCategoryId = 1,
                           IsDeleted = false
                       },
                       new ChildCareChild()
                       {
                           Name = "dinesh",
                           DateOfBirth = DateTime.Parse("05/19/1986"),
                           AgeCategoryId = 1,
                           IsDeleted = false
                       },
                       new ChildCareChild()
                       {
                           Name = "palani",
                           DateOfBirth = DateTime.Parse("05/19/1986"),
                           AgeCategoryId = 2,
                           IsDeleted = false
                       },
                       new ChildCareChild()
                       {
                           Name = "Ahsan",
                           DateOfBirth = DateTime.Parse("05/19/1986"),
                           AgeCategoryId = 2,
                           IsDeleted = false
                       }

                   },

                    HasChildrenWithDisabilityInNeedOfChildCare = true,
                    HasFutureChildCareNeed = true,
                    FutureChildCareNeedNotes = "Notes",
                }
                    }

                }
            });

            var vm = new ChildCareSectionViewModel(mockRepo.Object);
            vm.InitializeFromPin("300");
            var test = vm.GetData("300", 300m);

            //Assert

            Assert.IsTrue(test.HasChildren.HasValue);
            Assert.IsTrue(test.HasChildrenWithDisabilityInNeedOfChildCare.HasValue);
            Assert.IsTrue(test.Children.Count.Equals(2));
            Assert.IsTrue(test.Teens.Count.Equals(2));
        }
        #endregion

    }
}
