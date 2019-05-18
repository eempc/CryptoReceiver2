using System;
using System.Collections.Generic;
using System.Text;

namespace AddressDisplay.Currency {
    class FiatCurrency : Currency {
        public char symbolChar { get; set; }
        public FiatCurrency(string symbol, string fullName, Dictionary<string, int> unitNames, string imageFile, char symbolChar) 
            : base (symbol, fullName, unitNames, imageFile) {

        }

        public FiatCurrency() {

        }
    }
}
