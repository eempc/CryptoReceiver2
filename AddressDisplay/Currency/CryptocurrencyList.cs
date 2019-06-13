using System.Collections.Generic;

namespace AddressDisplay.Currency {
    class CryptocurrencyList {
        // Hard coded list of cryptocurrencies, an SQLite version for user-generated addresses may exist in future

        public static Dictionary<string, Cryptocurrency> cryptocurrencies = new Dictionary<string, Cryptocurrency>();

        public static void InitiateCryptos() {
            cryptocurrencies.Add("Ethereum", 
                new Cryptocurrency(
                    "ETH", // Symbol
                    "Ethereum", //Fullname
                    new Dictionary<int, string>() { { 0, "ether" }, { 3, "finney"}, { 6, "szabo" }, { 9, "shannon" }, { 12, "lovelace" }, { 15, "babbage" }, { 18, "wei" } }, // denominations
                    "eth.png", // Image resource
                    @"https://etherscan.io/address/" // External site to check address details where address can be appended to the end to return result
                ));

            cryptocurrencies.Add("Bitcoin", 
                new Cryptocurrency(
                "BTC", 
                "Bitcoin", 
                new Dictionary<int, string>() { { 0, "bitcoin" }, { 8, "satoshi" } }, 
                "btc.png", 
                @"https://www.blockchain.com/btc/address/"
                ));

            cryptocurrencies.Add("Monero", 
                new Cryptocurrency(
                "XMR", 
                "Monero", 
                new Dictionary<int, string>() { { 0, "monero" }, { 12, "piconero" } }, 
                "xmr.png", 
                ""
                ));
        }

        public static List<string> GetCryptoList() {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, Cryptocurrency> entry in cryptocurrencies) {
                list.Add(entry.Value.FullName); // Same as entry.Key but probably best to use the object property
            }
            return list;
        }
    }
}
