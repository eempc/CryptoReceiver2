using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AddressDisplay.Address;
using ZXing;
using Xamarin.Essentials;
using AddressDisplay.Currency;
using AddressDisplay.ExtraTools;

namespace AddressDisplay {
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage {
        List<ListViewUserAddress> addresses; // User addresses list that will be used to populate the wallet area at the bottom (see OnAppearing)
        List<string> fiatList = new List<string>(); // This list is for the picker

        //FiatCurrency currentFiat = new Currency.FiatCurrency();
        Cryptocurrency currentCryptoObject = new Cryptocurrency();

        // If I want to avoid API problems then these two variables should be set as preferences
        string currentFiatCurrencySymbol;
        string currentCryptoCurrencyFullName;
        double currentPrice;

        string currentAddress;

        public MainPage() {
            InitializeComponent();
            InitialisationStuff();
        }

        private void InitialisationStuff() {
            // Ensure the database file is present
            AddressDatabase.CreateDatabase();

            // Here I have chosen to invoke the static methods to initialise the lists (versus instantiating an object)
            CryptocurrencyList.InitiateCryptos();
            FiatCurrencyList.InitiateFiats();

            // string currentFiatCurrency and currentCryptoCurrency take priority in here
            currentCryptoCurrencyFullName = Preferences.Get("current_crypto", "Bitcoin");
            currentCryptoObject = CryptocurrencyList.cryptocurrencies[currentCryptoCurrencyFullName];

            // Populate the fiat picker with data binding (in order)
            fiatList = Currency.FiatCurrencyList.GetFiatSymbolList();
            FiatPicker.ItemsSource = fiatList.OrderBy(c => c).ToList();

            // Set user currency from preferences or default to USD
            currentFiatCurrencySymbol = Preferences.Get("user_currency", "USD"); // string, is this redundant?

            // Then default the picker to the user preference by getting the index and setting the picker via index (silly I know)
            int startingIndex = FiatPicker.ItemsSource.IndexOf(currentFiatCurrencySymbol);
            FiatPicker.SelectedIndex = startingIndex;
            SetExchangeRate();

            // Get last known id and set it to current display, this will also set the current cryptocurrency
            int lastId = Preferences.Get("current_id", 0);
            if (lastId != 0) SetAddressView(lastId);

            // Update with default amount
            UpdateCryptoAmount();
        }
       
        // To navigate to the listview address page - DO NOT DELETE
        //public void InitialiseImageButton() {
        //    TapGestureRecognizer iconTap = new TapGestureRecognizer();
        //    iconTap.Tapped += (object sender, EventArgs e) => { DoSomething(); };
        //    Image ic = X:NameInXAML;
        //    ic.GestureRecognizers.Add(iconTap);
        //}

        private void Burger_Clicked(object sender, EventArgs e) => GoToAddPage();        

        // Page navigation to the address page
        public async void GoToAddPage() => await Navigation.PushModalAsync(new AddressPage (), false);

        // When user enters a fiat amount into the box
        private void FiatAmount_TextChanged(object sender, TextChangedEventArgs e) {
            UpdateCryptoAmount();            
        }

        public void UpdateCryptoAmount() {
            if (double.TryParse(FiatAmount.Text, out double fiatAmount) && !Double.IsNaN(fiatAmount) && fiatAmount > 0 && !Double.IsInfinity(fiatAmount)) {
                CryptoAmount.Text = (fiatAmount / currentPrice).ToString("0.####");
            }
        }        

        // Populate the wallet icon area at the bottom
        protected override void OnAppearing() {            
            PopulateWalletArea(); // At the bottom of the page
        }

        private void PopulateWalletArea() {
            addresses = AddressDatabase.ReadDatabaseThenCastListViewUserAddress();
            BindableLayout.SetItemsSource(WalletArea, addresses); // Data binding part
        }

        // When user clicks on one of the icons depicting their address (this was going to be remade      
        private void ImageButton_Clicked(object sender, EventArgs e) {
            string senderId = ((ImageButton)sender).ClassId; // Unbox the sender object to ImageButton. Then retrieve its ID, which is the database ID
            // Try casting TryParse then LoadAddress(int)
            if (int.TryParse(senderId, out int number)) {
                SetAddressView(number);
                SetExchangeRate();
                UpdateCryptoAmount();
            }
        }

        public void SetAddressView(int number) {
            UserAddress address = AddressDatabase.GetItemById(number);
            //currentCrypto = Currency.CryptocurrencyList.cryptocurrencies[address.crypto];
            currentCryptoCurrencyFullName = address.crypto; // not the best way to do it
            currentCryptoObject = CryptocurrencyList.cryptocurrencies[currentCryptoCurrencyFullName];
            Preferences.Set("current_crypto", currentCryptoCurrencyFullName);

            //string cryptoName = address.crypto;
            Header.Text = currentCryptoObject.FullName;
            TopLeftIcon.Source = currentCryptoObject.ImageFile; // Image URL
            GivenName.Text = address.name;
            CryptoAddress.Text = address.address; //.yeah well done here rofl
            BarcodeImageView.BarcodeValue = address.address;
            // Etherscan image source
            // Feed box changes

            Preferences.Set("current_id", number);
            currentAddress = address.address;
        }

        private void ExchangeRate_Clicked(object sender, EventArgs e) {
            SetExchangeRate();
            UpdateCryptoAmount();
        }

        private void SetExchangeRate() {
            string currentCryptoCurrencySymbol = currentCryptoObject.Symbol;
            double x = PriceFeed.GetSingleRate(currentCryptoCurrencySymbol, currentFiatCurrencySymbol);
            currentPrice = x;
            ExchangeRate.Text = x.ToString("0.###"); // This will require significant figures calculation (in a new class) - case less than 1, less than 0.001, etc
            UpdateCryptoLabels();
        }

        private void UpdateCryptoLabels() {
            CryptoUnits.Text = currentCryptoObject.UnitNames[0];
            ExternalLink.Text = (currentCryptoObject.ExternalUrl != "") ? StringManipulation.RemoveHttps(currentCryptoObject.ExternalUrl) : "None"; // Regex trim to the base address            
        }

        private void FiatPicker_SelectedIndexChanged(object sender, EventArgs e) {
            // Picker picker = sender as Picker;
            string selectedItem = FiatPicker.SelectedItem.ToString();
            SetUserFiatCurrency(selectedItem);
        }

        // User preference 1, if there are any more, create a new class to store it all, or maybe just do that now?
        private void SetUserFiatCurrency(string fiatSymbol) {
            Preferences.Set("user_currency", fiatSymbol);
            //currentFiat = Currency.FiatCurrencyList.fiatCurrencies[fiatSymbol];
            currentFiatCurrencySymbol = fiatSymbol;
            SetExchangeRate();
            UpdateCryptoAmount();
        }

        private void ExternalLink_Clicked(object sender, EventArgs e) {
            OpenBrowser();
        }

        // This browser could perhaps be in a new class. It could be replaced by a transaction verification with the etherscan API
        private async void OpenBrowser() {
            if (currentCryptoObject.ExternalUrl != "") await Browser.OpenAsync(currentCryptoObject.ExternalUrl, BrowserLaunchMode.SystemPreferred);
        }
    }
}
