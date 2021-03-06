﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Web; // Sometimes this requires an assembly to be added
using Newtonsoft.Json;
using AddressDisplay.ExtraTools;

namespace AddressDisplay.Currency {
    class PriceFeed {
        // Register for an API key from CMC developer portal for free and use the following URLs for the latest market price (JSON)
        private const string API_KEY = "";
        // The standard quote URL
        private static readonly string url1 = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";
        // This tool will do the conversion automatically
        private static readonly string url2 = "https://pro-api.coinmarketcap.com/v1/tools/price-conversion";

        private static string MakeApiCall (string firstCurrency = "BTC", string secondCurrency = "USD", double amount = 1) {
            //if (String.IsNullOrEmpty(API_KEY)) return "0";
            // These are rudimentary checks, to fix the common error of a crypto's full name being fed into the API call, should've used the symbol throughout
            if (firstCurrency.Length > 5) {
                try {
                    firstCurrency = CryptocurrencyList.cryptocurrencies[firstCurrency].Symbol;
                } catch (Exception e) {
                    //firstCurrency = "BTC";                    
                }
            }

            if (secondCurrency.Length > 5) {
                try {
                    secondCurrency = CryptocurrencyList.cryptocurrencies[secondCurrency].Symbol;
                } catch (Exception e) {
                    //secondCurrency = "USD";
                }
            }

            UriBuilder URL = new UriBuilder(url2);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["symbol"] = firstCurrency;
            queryString["amount"] = amount.ToString();
            queryString["convert"] = secondCurrency;

            URL.Query = queryString.ToString();

            WebClient client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", "2977f70f-3ac7-4a5e-833c-7ea88278f25c");
            client.Headers.Add("Accepts", "application/json");

            return client.DownloadString(URL.ToString());
        }

        public static double GetSingleRate(string cryptocurrency = "BTC", string fiatCurrency = "USD") {
            string json = MakeApiCall(cryptocurrency, fiatCurrency);
            Dictionary<string, Dictionary<string, object>> topLevel = JsonParseTopLevel(json);
            string json2 = topLevel["data"]["quote"].ToString();
            Dictionary<string, Dictionary<string, object>> secondLevel = JsonParseTopLevel(json2);
            double targetRate = (double)secondLevel[fiatCurrency]["price"];
            return targetRate;
        }

        // Helper method. Insert JSON string and it will convert it to dictionary. This is for two identical levels hence the second Dictionary
        public static Dictionary<string, Dictionary<string, object>> JsonParseTopLevel(string json) {
            return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
        }

    }
}
