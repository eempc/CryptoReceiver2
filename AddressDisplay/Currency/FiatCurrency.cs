using System;
using System.Collections.Generic;
using System.Text;

namespace AddressDisplay.Currency {
    class FiatCurrency : Currency {
        public FiatCurrency(string symbol, string fullName, Dictionary<string, int> unitNames, string imageFile) 
            : base (symbol, fullName, unitNames, imageFile) {

        }

        public FiatCurrency() {

        }
    }
}
