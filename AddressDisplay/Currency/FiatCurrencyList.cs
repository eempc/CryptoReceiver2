﻿using System.Collections.Generic;

// As with the crypto currency list, this is hard coded in as a static class

namespace AddressDisplay.Currency {
    class FiatCurrencyList {
        public static Dictionary<string, FiatCurrency> fiatCurrencies = new Dictionary<string, FiatCurrency>();
        
        public static void InitiateFiats() {
            fiatCurrencies.Add("USD", 
                new FiatCurrency(
                    "USD",
                    "United States Dollar",
                    new Dictionary<int, string>() { { 0, "dollar" }, { 2, "cent" }, { 4, "pip" } },
                    "eth.png",
                    '$'
                ));

            fiatCurrencies.Add("EUR", 
                new FiatCurrency(
                    "EUR",
                    "Euro",
                    new Dictionary<int, string>() { { 0, "euro" }, { 2, "cent" }, { 4, "pip" } },
                    "eth.png",
                    '€'
                ));

            fiatCurrencies.Add("GBP", 
                new FiatCurrency(
                    "GBP",
                    "British Pound",
                    new Dictionary<int, string>() { { 0, "pound" }, { 2, "pence" }, { 4, "pip" } },
                    "eth.png",
                    '£'
                ));
        }

        // Get symbol list only, i.e. "EUR", not '€' which is a charSymbol
        public static List<string> GetFiatSymbolList() {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, FiatCurrency> entry in fiatCurrencies) {
                list.Add(entry.Value.Symbol);
            }
            return list;
        }
    }
}
