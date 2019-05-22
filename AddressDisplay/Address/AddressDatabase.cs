using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace AddressDisplay.Address {
    class AddressDatabase {
        static string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string databasePath = Path.Combine(personalFolder, "addresses002.db3");

        // Create the database if it does not already exist and seed it with one example address
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

        // This is the basic way of acquiring a list of user addresses from the database
        public static List<UserAddress> ReadDatabaseUserAddress() {
            List<UserAddress> list = new List<UserAddress>();
            SQLiteConnection db = new SQLiteConnection(databasePath);
            var table = db.Table<UserAddress>();
            foreach (var item in table) {
                list.Add(item);
            }
            db.Close();
            return list;
        }

        // This is an optional method to cast the previous method to list view user addresses (with the extra properties)
        public static List<ListViewUserAddress> ReadDatabaseThenCastListViewUserAddress() {
            List<UserAddress> basicAddresses = ReadDatabaseUserAddress();
            List<ListViewUserAddress> listViewAddresses = new List<ListViewUserAddress>();
            // Then cast it to the ListViewUserAddress and add the icon's path
            foreach (UserAddress userAddress in basicAddresses) {
                ListViewUserAddress addressToBeAdded = new ListViewUserAddress();
                addressToBeAdded.id = userAddress.id;
                addressToBeAdded.name = userAddress.name;
                addressToBeAdded.address = userAddress.address;
                addressToBeAdded.crypto = userAddress.crypto;
                addressToBeAdded.cryptoIconPath = Currency.CryptocurrencyList.cryptocurrencies[addressToBeAdded.crypto].ImageFile; // Was this really the best way to do this?

                //ListViewUserAddress addressToBeAdded = (ListViewUserAddress)userAddress;

                listViewAddresses.Add(addressToBeAdded);
            }

            return listViewAddresses;
        }

        public static void DeleteFromDatabase(int id) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            db.Delete<UserAddress>(id);
            db.Close();
        }

        // This saves or updates, depends on the address id is > 0 or not
        public static void SaveToDatabase(UserAddress address) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            if (address.id <= 0) {
                db.Insert(address);
            } else {
                db.Update(address);
            }
            db.Close();
        }

        public static UserAddress GetItemById(int number) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            UserAddress singleAddress = db.Table<UserAddress>().Where(x => x.id == number).FirstOrDefault();
            db.Close();
            return singleAddress;

        }
    }
}
