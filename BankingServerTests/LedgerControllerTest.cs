using Xunit;
using Moq;
using BankingServerProvider.IProviders;
using AutoFixture;
using System.Threading.Tasks;
using BankingServer.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BankingServerTests
{
    public class LedgerControllerTest
    {

        private Mock<ILedgerProvider> mockLedgerProvider;
        private Mock<IUserAccountProvider> mockUserAccountProvider;
        private Fixture fixture;


        public LedgerControllerTest()
        {
            mockLedgerProvider = new Mock<ILedgerProvider>();
            mockUserAccountProvider = new Mock<IUserAccountProvider>();
            fixture = new Fixture();
        }


        //This is an example of some Contorller level tests.  Not much meat to them, just mocking the one or 2 provider methods that exist, and 
        //making sure that mocks were executed/object types are returned back ok
        [Fact]
        public void RequestCurrentBalanceWithInvalidToken_ShouldReturnBadRequest()
        {
            //Arrange
            var token = fixture.Create<string>();
            mockUserAccountProvider.Setup(x => x.isLoggedIn(token)).Returns(false);
            //Act
            var ledgerController = new LedgerController(mockUserAccountProvider.Object, mockLedgerProvider.Object);
            var result = ledgerController.CurrentBalance(token) as BadRequestObjectResult;

            //Asert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not presently logged in",result.Value);
            mockUserAccountProvider.VerifyAll();
                    
        }

        [Fact]
        public void RequestCurrentBalanceWithInvalidToken_ShouldReturnCurrentBalance()
        {
            //Arrange
            var token = fixture.Create<string>();
            var balance = fixture.Create<decimal>();
            mockUserAccountProvider.Setup(x => x.isLoggedIn(token)).Returns(true);
            mockLedgerProvider.Setup(x => x.getCurrentBalance(token)).Returns(balance);
            //Act
            var ledgerController = new LedgerController(mockUserAccountProvider.Object, mockLedgerProvider.Object);
            var result = ledgerController.CurrentBalance(token) as OkObjectResult;

            //Asert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(balance, result.Value);
            mockUserAccountProvider.VerifyAll();
            mockLedgerProvider.VerifyAll();

        }
    }
}
