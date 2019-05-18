using System;
using System.Collections.Generic;
using System.Text;

namespace AddressDisplay.Currency {
    class FiatCurrencyList {
        public static Dictionary<string, FiatCurrency> fiatCurrencies = new Dictionary<string, FiatCurrency>();
        
        public static void InitiateFiats() {
            fiatCurrencies.Add("USD", new FiatCurrency(
                "USD",
                "United States Dollar",
                new Dictionary<string, int>() { { "dollar", 0 }, { "cent", 2 }, { "pip", 4 } },
                "eth.png",
                '$'
                ));
            fiatCurrencies.Add("EUR", new FiatCurrency(
                "EUR",
                "Euro",
                new Dictionary<string, int>() { { "euro", 0 }, { "cent", 2 }, { "pip", 4 } },
                "eth.png",
                '€'
                ));
            fiatCurrencies.Add("GBP", new FiatCurrency(
                "GBP",
                "British Pound",
                new Dictionary<string, int>() { { "pound", 0 }, { "pence", 2 }, { "pip", 4 } },
                "eth.png",
                '£'
                ));
        }

        public static List<string> GetFiatSymbolList() {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, FiatCurrency> entry in fiatCurrencies) {
                list.Add(entry.Value.symbol);
            }
            return list;
        }
    }
}
