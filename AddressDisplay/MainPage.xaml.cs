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
        List<ListViewUserAddress> addresses; // Addresses list that will be used to populate the wallet area at the bottom (see OnAppearing)
        List<string> fiatList = new List<string>();
        string currentFiatCurrency;


        public MainPage() {
            InitializeComponent();
            InitialisationStuff();
        }

        private void InitialisationStuff() {
            // Here I have chosen to invoke the static methods to initialise the lists (versus instantiating an object)
            Currency.CryptocurrencyList.InitiateCryptos();
            Currency.FiatCurrencyList.InitiateFiats();

            // Populate the fiat picker with data binding
            fiatList = Currency.FiatCurrencyList.GetFiatSymbolList();
            FiatPicker.ItemsSource = fiatList.OrderBy(c => c).ToList();

            // Set user currency from preferences or default to USD
            currentFiatCurrency = Preferences.Get("user_currency", "USD");

            // Default the picker to the user preferences
            int startingIndex = FiatPicker.ItemsSource.IndexOf(currentFiatCurrency);
            FiatPicker.SelectedIndex = startingIndex;

            // Ensure the database file is present
            AddressDatabase.CreateDatabase();
        }

        // To navigate to the listview address page - DO NOT DELETE
        //public void InitialiseImageButton() {
        //    TapGestureRecognizer iconTap = new TapGestureRecognizer();
        //    iconTap.Tapped += (object sender, EventArgs e) => { DoSomething(); };
        //    Image ic = X:NameInXAML;
        //    ic.GestureRecognizers.Add(iconTap);
        //}

        private void Burger_Clicked(object sender, EventArgs e) {
            GoToAddPage();
        }

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
        //private void ImageButtonFlex_Clicked(object sender, EventArgs e) {
        //    //var id = ((ImageButton)sender).ClassId;

        //    ////await DisplayAlert("Alert", id, "OK");

        //    //LoadAddress(id);
        //}

        private void ImageButton_Clicked(object sender, EventArgs e) {
            string id = ((ImageButton)sender).ClassId;
            LoadAddress(id);
        }

        public void LoadAddress(string x) {
            if (int.TryParse(x, out int number)) {
                //await DisplayAlert("Alert", i.ToString(), "OK");
                UserAddress address = AddressDatabase.GetItemById(number);
                SetAddressView(address);
            }
        }

        public void SetAddressView(UserAddress address) {
            string cryptoName = address.crypto;
            Header.Text = cryptoName;
            TopLeftIcon.Source = Currency.CryptocurrencyList.cryptocurrencies[cryptoName].imageFile;
            GivenName.Text = address.name;
            CryptoAddress.Text = address.address; //.yeah well done here rofl
            BarcodeImageView.BarcodeValue = address.address;
            // Etherscan image source
            // Feed box changes
        }

        private void ExchangeRate_Clicked(object sender, EventArgs e) {
            //string price = Currency.PriceFeed.MakeApiCall("ETH");
            //ExchangeRate.Text = "$300";
            var x = Currency.PriceFeed.GetSingleRate("ETH", "USD");
            ExchangeRate.Text = x.ToString();
        }

        private void FiatPicker_SelectedIndexChanged(object sender, EventArgs e) {
            // Picker picker = sender as Picker;
            string selectedItem = FiatPicker.SelectedItem.ToString();
            SetUserFiatCurrency(selectedItem);
        }

        // User preference 1, if there are any more, create a new class to store it all, or maybe just do that now?
        private void SetUserFiatCurrency(string fiatSymbol) {
            Preferences.Set("user_currency", fiatSymbol);
        }
    }
}
