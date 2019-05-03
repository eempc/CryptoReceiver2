using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

namespace AddressDisplay.Address {
    class AddressDatabase {
        static string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string databasePath = Path.Combine(personalFolder, "addresses002.db3");

        public static void CreateDatabase() {
            if (!File.Exists(databasePath)) {
                SQLiteConnection db = new SQLiteConnection(databasePath);
                db.CreateTable<UserAddress>(); // Create table based on the class model UserAddress
                UserAddress seedingAddress = new UserAddress();
                seedingAddress.name = "My first Ethereum address";
                seedingAddress.address = "0xd26114cd6EE289AccF82350c8d8487fedB8A0C07";
                seedingAddress.crypto = "Ethereum";                
                db.Insert(seedingAddress);
            }
        }

        public static List<UserAddress> ReadDatabase() {
            List<UserAddress> list = new List<UserAddress>();
            SQLiteConnection db = new SQLiteConnection(databasePath);
            var table = db.Table<UserAddress>();
            foreach (var item in table) {
                list.Add(item);
            }
            return list;
        }

        public static List<ListViewUserAddress> ReadDatabase2() {
            List<UserAddress> basicAddresses = ReadDatabase();
            List<ListViewUserAddress> listViewAddresses = new List<ListViewUserAddress>();
            // Then cast it to the ListViewUserAddress and add the icon's path
            foreach (UserAddress userAddress in basicAddresses) {
                ListViewUserAddress addressToBeAdded = new ListViewUserAddress();
                addressToBeAdded.id = userAddress.id;
                addressToBeAdded.name = userAddress.name;
                addressToBeAdded.address = userAddress.address;
                addressToBeAdded.crypto = userAddress.crypto;
                addressToBeAdded.cryptoIconPath = Currency.CryptocurrencyList.cryptocurrencies[addressToBeAdded.crypto].imageFile; // Was this really the best way to do this?
                listViewAddresses.Add(addressToBeAdded);
            }
            return listViewAddresses;
        }

        public static void DeleteFromDatabase(int id) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            db.Delete<UserAddress>(id);
        }

        public static void SaveToDatabase(UserAddress address) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            if (address.id <= 0) {
                db.Insert(address);
            } else {
                db.Update(address);
            }
        }

        public static UserAddress GetItemById(int number) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            return db.Table<UserAddress>().Where(x => x.id == number).FirstOrDefault();
        }
    }
}
