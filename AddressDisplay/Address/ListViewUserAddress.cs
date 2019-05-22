namespace AddressDisplay.Address {
    class ListViewUserAddress : UserAddress {
        // The inheritance of this class simply adds an extra member (the image path) to the base class that I do not want to be stored in the SQLite database
        public string cryptoIconPath { get; set; }
    }
}
