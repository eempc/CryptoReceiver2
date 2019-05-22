using System.Collections.Generic;

namespace AddressDisplay.Currency {
    class FiatCurrency : Currency {
        public char symbolChar { get; set; }
        public FiatCurrency(string symbol, string fullName, Dictionary<int, string> unitNames, string imageFile, char symbolChar) 
            : base (symbol, fullName, unitNames, imageFile) {
        }

        public FiatCurrency() {
        }
    }
}
