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
                new Dictionary<int, string>() { { 0, "dollar" }, { 2, "cent" }, { 4, "pip" } },
                "eth.png",
                '$'
                ));
            fiatCurrencies.Add("EUR", new FiatCurrency(
                "EUR",
                "Euro",
                new Dictionary<int, string>() { { 0, "euro" }, { 2, "cent" }, { 4, "pip" } },
                "eth.png",
                '€'
                ));
            fiatCurrencies.Add("GBP", new FiatCurrency(
                "GBP",
                "British Pound",
                new Dictionary<int, string>() { { 0, "pound" }, { 2, "pence" }, { 4, "pip" } },
                "eth.png",
                '£'
                ));
        }

        public static List<string> GetFiatSymbolList() {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, FiatCurrency> entry in fiatCurrencies) {
                list.Add(entry.Value.Symbol);
            }
            return list;
        }
    }
}
