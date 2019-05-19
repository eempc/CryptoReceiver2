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

namespace AddressDisplay {
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage {
        List<ListViewUserAddress> addresses; // User addresses list that will be used to populate the wallet area at the bottom (see OnAppearing)
        List<string> fiatList = new List<string>(); // This list is for the picker
        
        Currency.FiatCurrency currentFiat;
        Currency.Cryptocurrency currentCrypto;
        double currentPrice;       

        public MainPage() {
            InitializeComponent();
            InitialisationStuff();
        }

        private void InitialisationStuff() {
            // Here I have chosen to invoke the static methods to initialise the lists (versus instantiating an object)
            Currency.CryptocurrencyList.InitiateCryptos();
            Currency.FiatCurrencyList.InitiateFiats();

            // Populate the fiat picker with data binding (in order)
            fiatList = Currency.FiatCurrencyList.GetFiatSymbolList();
            FiatPicker.ItemsSource = fiatList.OrderBy(c => c).ToList();

            // Set user currency from preferences or default to USD
            string currentFiatCurrency = Preferences.Get("user_currency", "USD"); // string, is this redundant?
            currentFiat = Currency.FiatCurrencyList.fiatCurrencies[currentFiatCurrency]; // object

            // Then default the picker to the user preference by getting the index and setting the picker via index (silly I know)
            int startingIndex = FiatPicker.ItemsSource.IndexOf(currentFiatCurrency);
            FiatPicker.SelectedIndex = startingIndex;

            // Ensure the database file is present
            AddressDatabase.CreateDatabase();

            // Get last known id and set it to current display, this will also set the current cryptocurrency
            int lastId = Preferences.Get("current_id", 0);
            SetAddressView(lastId);
            SetExchangeRate();

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
        //private void FiatAmount_TextChanged(object sender, TextChangedEventArgs e) {
        //    //if (double.TryParse(FiatAmount.Text, out double d) && !Double.IsNaN(d) && d > 0 && !Double.IsInfinity(d)) {
        //    //    UpdateCryptoAmount(d);
        //    //}
        //}

        //public void UpdateCryptoAmount(double fiatAmount) => CryptoAmount.Text = (fiatAmount).ToString();

        // Populate the wallet icon area at the bottom
        protected override void OnAppearing() {            
            PopulateWalletArea(); // At the bottom of the page
        }

        private void PopulateWalletArea() {
            addresses = AddressDatabase.ReadDatabase2();
            BindableLayout.SetItemsSource(WalletArea, addresses); // Data binding part
        }

        // When user clicks on one of the icons depicting their address (this was going to be remade      
        // I don't like this code
        private void ImageButton_Clicked(object sender, EventArgs e) {
            string senderId = ((ImageButton)sender).ClassId; // ID is the database ID
            // Try casting TryParse then LoadAddress(int)
            if (int.TryParse(senderId, out int number)) {
                SetAddressView(number);
            }

            //LoadAddress(id);
        }

        //public void LoadAddress(string x) {
        //    if (int.TryParse(x, out int number)) {
        //        UserAddress address = AddressDatabase.GetItemById(number); // Retrieve from the database
        //        SetAddressView(address);
        //    }
        //}

        public void SetAddressView(int number) {
            UserAddress address = AddressDatabase.GetItemById(number);
            currentCrypto = Currency.CryptocurrencyList.cryptocurrencies[address.crypto];

            string cryptoName = address.crypto;
            Header.Text = cryptoName;
            TopLeftIcon.Source = Currency.CryptocurrencyList.cryptocurrencies[cryptoName].imageFile;
            GivenName.Text = address.name;
            CryptoAddress.Text = address.address; //.yeah well done here rofl
            BarcodeImageView.BarcodeValue = address.address;
            // Etherscan image source
            // Feed box changes

            Preferences.Set("current_id", number);

        }

        private void ExchangeRate_Clicked(object sender, EventArgs e) {
            SetExchangeRate();
        }

        private void SetExchangeRate() {
            double x = Currency.PriceFeed.GetSingleRate(currentCrypto.symbol, currentFiat.symbol);
            ExchangeRate.Text = x.ToString();
            currentPrice = x;
        }

        private void FiatPicker_SelectedIndexChanged(object sender, EventArgs e) {
            // Picker picker = sender as Picker;
            string selectedItem = FiatPicker.SelectedItem.ToString();
            SetUserFiatCurrency(selectedItem);
        }

        // User preference 1, if there are any more, create a new class to store it all, or maybe just do that now?
        private void SetUserFiatCurrency(string fiatSymbol) {
            Preferences.Set("user_currency", fiatSymbol);
            currentFiat = Currency.FiatCurrencyList.fiatCurrencies[fiatSymbol];
            SetExchangeRate();
        }
    }
}
