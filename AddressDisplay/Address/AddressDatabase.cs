﻿using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

// All SQLite stuff goes in this file

namespace AddressDisplay.Address {
    class AddressDatabase {
        private static string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string databasePath = Path.Combine(personalFolder, "addresses003.db3");

        // Create the database if it does not already exist and seed it with one example address
        public static void CreateDatabase() {
            if (!File.Exists(databasePath)) {
                SQLiteConnection db = new SQLiteConnection(databasePath);
                db.CreateTable<UserAddress>(); // Create table based on the class model UserAddress

                UserAddress seedingAddress = new UserAddress();
                seedingAddress.Name = "My first Ethereum address";
                seedingAddress.Address = "0xd26114cd6EE289AccF82350c8d8487fedB8A0C07";
                seedingAddress.Crypto = "Ethereum";      
                
                db.Insert(seedingAddress);
            }
        }

        // This is the basic way of acquiring a list of user addresses from the database
        public static List<UserAddress> ReadDatabaseUserAddress() {
            List<UserAddress> list = new List<UserAddress>();
            SQLiteConnection db = new SQLiteConnection(databasePath);
            TableQuery<UserAddress> table = db.Table<UserAddress>();

            foreach (var item in table) {
                list.Add(item);
            }

            db.Close();
            return list;
        }

        // This is an optional method to cast the previous method's retrieval of UserAddress model to ListViewUserAddress model (with the extra properties)
        // There has to be a better way to cast a base class into a derived class?
        // Maybe an extension method for the derived class
        public static List<ListViewUserAddress> ReadDatabaseThenCastListViewUserAddress() {
            List<UserAddress> basicAddresses = ReadDatabaseUserAddress();
            List<ListViewUserAddress> listViewAddresses = new List<ListViewUserAddress>();

            // Then cast it to the ListViewUserAddress and add the icon's path
            foreach (UserAddress userAddress in basicAddresses) {
                ListViewUserAddress addressToBeAdded = new ListViewUserAddress();
                addressToBeAdded.Id = userAddress.Id;
                addressToBeAdded.Name = userAddress.Name;
                addressToBeAdded.Address = userAddress.Address;
                addressToBeAdded.Crypto = userAddress.Crypto;
                addressToBeAdded.CryptoIconPath = Currency.CryptocurrencyList.cryptocurrencies[addressToBeAdded.Crypto].ImageFile; // Was this really the best way to do this?

                listViewAddresses.Add(addressToBeAdded);
            }

            return listViewAddresses;
        }

        // Self identifying method names
        public static void DeleteFromDatabase(int id) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            db.Delete<UserAddress>(id);
            db.Close();
        }

        // This saves or updates, depends on the address id is > 0 or not
        public static void SaveToDatabase(UserAddress address) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            if (address.Id <= 0) {
                db.Insert(address);
            } else {
                db.Update(address);
            }
            db.Close();
        }

        public static UserAddress GetItemById(int number) {
            SQLiteConnection db = new SQLiteConnection(databasePath);
            UserAddress singleAddress = db.Table<UserAddress>().Where(x => x.Id == number).FirstOrDefault();
            db.Close();
            return singleAddress;

        }
    }
}
