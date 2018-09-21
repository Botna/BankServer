using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingServerData
{
    public class JWTHelper : IJWTHelper
    {
        //Ill admit, this was shameless.
        const string notSoSecretSecret = "PleaseGiveMeAJob";

        public string buildToken(Dictionary<string, string> myMap)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(myMap, notSoSecretSecret);
            return token;
        }

        public Dictionary<string, string> decodeToken(string myToken)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            var json = decoder.Decode(myToken, notSoSecretSecret, verify: true);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        }
    }
}
