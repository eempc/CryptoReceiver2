using System.Collections.Generic;

namespace AddressDisplay.Currency {
    class Cryptocurrency : Currency {
        public string ExternalUrl { get; set; }
        public Cryptocurrency(string symbol, string fullName, Dictionary<int, string> unitNames, string imageFile, string externalUrl) :
            base(symbol, fullName, unitNames, imageFile) {
            ExternalUrl = externalUrl;
        }

        public Cryptocurrency() {

        }
    }
}
