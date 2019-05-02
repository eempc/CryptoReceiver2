using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

// Model object for saving onto the database
namespace AddressDisplay.Address {
    [Table("UserAddress")]
    public class UserAddress {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; } // For identifying in the SQLite database
        [MaxLength(64), Unique]
        public string name { get; set; } // E.g. "My first Ethereum address"
        [MaxLength(64)]
        public string address { get; set; } // "E.g. 0x89621f199bbc88a" Technically hexadecimal
        [MaxLength(64)]
        public string crypto { get; set; } // E.g. a string "Ethereum" 
    }
}