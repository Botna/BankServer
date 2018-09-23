using Xunit;
using Moq;
using BankingServerProvider.IProviders;
using AutoFixture;
using System.Threading.Tasks;
using BankingServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BankingServerData.Models;
using BankingServerData.DataStoreProvider;

namespace BankingServerTests
{
    public class DataProviderTests
    {
        private Fixture fixture;

        public DataProviderTests()
        {
            fixture = new Fixture();
        }
        //This is an example of some Provider level tests that actually ahve some business logic in them.
        //Since this is just a demonstration, i will not adequatey mock my JWTHelper class.
        [Fact]
        public void RequestSetDataWithNewData_DataGetsSet()
        {
            //Arrange
            List<DataStore> newList = fixture.Create<List<DataStore>>();
            var numberOfItems = newList.Count;
            var dataStoreProvider = new DataStoreProvider(null);
            dataStoreProvider.setCachedData(newList);

            var newDatum = fixture.Create<DataStore>();

            //Act
            dataStoreProvider.setData(newDatum);
            var existingData = dataStoreProvider.getCachedData();

            Assert.Equal(numberOfItems + 1, existingData.Count);
            Assert.NotNull(existingData.Find(x => x.accountInformation.userName == newDatum.accountInformation.userName));

        }

        [Fact]
        public void RequestSetDataWithExistingData_DataGetsSet()
        {
            //Arrange
            List<DataStore> newList = fixture.Create<List<DataStore>>();
            var dataStoreProvider = new DataStoreProvider(null);
            dataStoreProvider.setCachedData(newList);

            var newDatum = fixture.Create<DataStore>();
            newDatum.accountInformation.userName = newList[0].accountInformation.userName;
            //Act
            dataStoreProvider.setData(newDatum);
            var existingData = dataStoreProvider.getCachedData();

            Assert.Equal(newList.Count , existingData.Count);
            Assert.NotNull(existingData.Find(x => x.accountInformation.userName == newDatum.accountInformation.userName));


        }
    }
}
