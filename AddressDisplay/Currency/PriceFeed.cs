using System;
using System.Net;
using System.Web; // Sometimes this requires an assembly to be added

namespace AddressDisplay.Currency {
    class PriceFeed {
        // Register for an API key from CMC developer portal for free and use the following URL for the latest market price (JSON)
        private static string API_KEY = "2977f70f-3ac7-4a5e-833c-7ea88278f25c";
        private static string url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";

        public static string MakeApiCall (string symbol) {
            UriBuilder URL = new UriBuilder(url);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["symbol"] = symbol; // parameters go in here, you can have more than one symbol entered in here separated by comma

            URL.Query = queryString.ToString();

            WebClient client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY); // API key is inserted as a header
            client.Headers.Add("Accepts", "application/json");

            return client.DownloadString(URL.ToString());
        }

        public static void GetSinglePrice(string symbol) {
            string json = MakeApiCall(symbol);
        }

        // Question remains whether it is better to instantiate an object than go through all these instructions, there are only 8 lines though
    }
}
