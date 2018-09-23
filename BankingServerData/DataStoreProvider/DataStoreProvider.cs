using System;
using System.Collections.Generic;
using System.Text;
using BankingServerData.Models;
using System.Linq;
namespace BankingServerData.DataStoreProvider
{
    public class DataStoreProvider : IDataStoreProvider
    {
        List<DataStore> myCachedData = new List<DataStore>();



        private IJWTHelper myJWTHelper;
        public DataStoreProvider(IJWTHelper _myJWTHelper)
        {
            this.myJWTHelper = _myJWTHelper;
        }
        public DataStore getDataStore(string userName)
        {
            return myCachedData.Find(x => x.accountInformation.userName == userName);
        }

        public void setData(DataStore newDataStore)
        {
            var existingData = myCachedData.Find(x => x.accountInformation.userName == newDataStore.accountInformation.userName);
            if (existingData == null)
            {
                myCachedData.Add(newDataStore);
            }
            else
            {
                //Not a good way of doing this. It would be better to manipulate the already inserted objects, but this is quicker.
                myCachedData.Remove(existingData);
                myCachedData.Add(newDataStore);
            }
        }
        public string attemptLogin(string userName, string password)
        {
            var existingData = myCachedData.Find(x => x.accountInformation.userName == userName);
            if (existingData == null)
            {
                return null;
            }
            string token = null;
            if (existingData.accountInformation.userName == userName && existingData.accountInformation.password == password)
            {
                //this is bad, shoudl be comparing it to a salt!
                if (existingData.currentToken != null)
                {
                    token = existingData.currentToken;
                }
                token = myJWTHelper.buildToken(userName);
                existingData.currentToken = token;
                setData(existingData);
            }
            return token;
        }

        public bool attemptLogout(string token)
        {
            var existingData = getRecordFromToken(token);

            if (existingData == null)
            {
                return false;
            }
            existingData.currentToken = null;
            setData(existingData);
            return true;
        }

        public bool checkAuthentication(string token)
        {
            var existingData = getRecordFromToken(token);

            if (existingData == null || existingData.currentToken != token)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public decimal getCurrentBalance(string token)
        {
            var existingData = getRecordFromToken(token);

            return existingData.currentBalance;
        }



        public bool processWithdrawl(string token, decimal amount)
        {
            var existingData = getRecordFromToken(token);
            if (amount > existingData.currentBalance)
            {
                return false;
            }

            existingData.transactionHistory.Add(new TransactionHistory(DateTime.UtcNow, "Withdrawl", amount, existingData.currentBalance));
            existingData.currentBalance = existingData.currentBalance - amount;
            return true;
        }

        public bool processDeposit(string token, decimal amount)
        {
            var existingData = getRecordFromToken(token);

            existingData.transactionHistory.Add(new TransactionHistory(DateTime.UtcNow, "Deposit", amount, existingData.currentBalance));
            existingData.currentBalance = existingData.currentBalance + amount;
            return true;
        }

        public List<TransactionHistory> getTransactionhistory(string token)
        {
            var existingData = getRecordFromToken(token);
            return existingData.transactionHistory.OrderByDescending(x => x.timeOfTransaction).ToList();
        }

        #region helpers
        private DataStore getRecordFromToken(string token)
        {
            string userName;
            try
            {
                userName = myJWTHelper.decodeToken(token);
            }
            catch
            {
                return null;
            }
            return myCachedData.Find(x => x.accountInformation.userName == userName);
        }

        public List<DataStore> getCachedData()
        {
            return myCachedData;
        }
        public void setCachedData(List<DataStore> newData)
        {
            myCachedData = newData;
        }
        #endregion
    }
}
