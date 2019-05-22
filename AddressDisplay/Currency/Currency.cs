using System.Collections.Generic;

namespace AddressDisplay.Currency {
    public abstract class Currency {
        public string Symbol { get; set; }
        public string FullName { get; set; }
        public Dictionary<int, string> UnitNames { get; set; }
        public string ImageFile { get; set; }

        public Currency(string symbol, string fullName, Dictionary<int, string> unitNames, string imageFile) {
            this.Symbol = symbol;
            this.FullName = fullName;
            this.UnitNames = unitNames;
            this.ImageFile = imageFile;
        }

        public Currency() {

        }
    }
}
