using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerData
{
    public interface IJWTHelper
    {
        string buildToken(Dictionary<string, string> myMap);

        Dictionary<string, string> decodeToken(string myToken);
    }
}
