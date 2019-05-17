using System;
using System.Collections.Generic;
using System.Text;
using AddressDisplay.Currency;

namespace AddressDisplay.Address {
    class ListViewUserAddress : UserAddress {
        // The inheritance of this class simply adds an extra member (the image path) to the base class that I do not want to be stored in the SQLite database
        public string cryptoIconPath { get; set; }

        //public ListViewUserAddress(int id, string name, string address, string crypto) : base(id, name, address, crypto) {
        //    this.id = id;
        //    this.name = name;
        //    this.address = address;
        //    this.crypto = crypto;
        //    this.cryptoIconPath = CryptocurrencyList.cryptocurrencies[crypto].imageFile;
        //}

        //public ListViewUserAddress() {

        //}

    }
}
