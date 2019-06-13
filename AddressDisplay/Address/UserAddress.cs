using SQLite;

// Model object for saving onto the database
namespace AddressDisplay.Address {
    [Table("UserAddress")]
    public class UserAddress {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; } // For identifying in the SQLite database
        [MaxLength(64), Unique]
        public string Name { get; set; } // E.g. "My first Ethereum address"
        [MaxLength(64)]
        public string Address { get; set; } // "E.g. 0x89621f199bbc88a" Technically hexadecimal
        [MaxLength(64)]
        public string Crypto { get; set; } // E.g. a string "Ethereum" 

        //public UserAddress(int id, string name, string address, string crypto) {
        //    this.id = id;
        //    this.name = name;
        //    this.address = address;
        //    this.crypto = crypto;
        //}

        //public UserAddress() {

        //}
    }
}