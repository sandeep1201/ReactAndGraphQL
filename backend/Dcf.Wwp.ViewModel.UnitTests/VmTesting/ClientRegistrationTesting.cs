using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.ConnectedServices.Mci;

namespace Dcf.Wwp.ViewModel.UnitTests.VmTesting
{
    [TestClass]
    public class ClientRegistrationTesting
    {

        private Mock<IRepository> mockRepository;
        private Mock<IAuthUser>   mockAuthUser;
        private Mock<IMciService> mockMciService;
        private Mock<ICwwIndService> mockCwwIndService;
        private Mock<ICwwKeySecService> mockCwwKeySecService;
        private Mock<IClientRegistrationViewModel> mockClientRegistrationViewModel;
        private Mock<ClearRequest> mockClearRequest;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRepository>();
            mockAuthUser   = new Mock<IAuthUser>();
            mockMciService = new Mock<IMciService>();
            mockCwwIndService = new Mock<ICwwIndService>();
            mockCwwKeySecService = new Mock<ICwwKeySecService>();

            mockClearRequest = new Mock<ClearRequest>();
            mockMciService.Setup(x => x.Clear(mockClearRequest.Object)).Returns(new ClearResponse()
                                                                                {
                                                                                                
                                                                                });



        }


        [TestMethod]
        public void ClientRegUpsert_NewMci_NewCww()
        {
            // Arrange
            var contract = new ClientRegistrationContract();
            contract.Name = new PersonNameContract();
            contract.Name.FirstName = "Sohinder";
            contract.Name.MiddleInitial = "S";
            contract.Name.LastName = "Singh";


            // Act 
            var vm = new ClientRegistrationViewModel(mockMciService.Object, mockCwwIndService.Object, mockCwwKeySecService.Object, 
                                                     mockRepository.Object, mockAuthUser.Object);



            vm.UpsertClientRegistration(contract);

           
        }
     
    }
}
