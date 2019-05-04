using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AddressDisplay.Address;

namespace AddressDisplay {
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage {

        //int currentAddressIndex;
        List<ListViewUserAddress> addresses;

        public MainPage() {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
            Currency.CryptocurrencyList.InitiateCryptos();
            AddressDatabase.CreateDatabase();

            var iconTap = new TapGestureRecognizer();
            iconTap.Tapped += (object sender, EventArgs e) => { GoToAddPage(); };
            Image ic = Burger;
            ic.GestureRecognizers.Add(iconTap);

        }

        // Page navigation to the address page
        public async void GoToAddPage() => await Navigation.PushAsync(new AddressPage { Title = "Address CRUD Page" });

        // When user enters a fiat amount into the box
        //private void FiatAmount_TextChanged(object sender, TextChangedEventArgs e) {
        //    //if (double.TryParse(FiatAmount.Text, out double d) && !Double.IsNaN(d) && d > 0 && !Double.IsInfinity(d)) {
        //    //    UpdateCryptoAmount(d);
        //    //}
        //}

        //public void UpdateCryptoAmount(double fiatAmount) => CryptoAmount.Text = (fiatAmount).ToString();

        // Populate the wallet icon area
        protected override void OnAppearing() {
            PopulateWalletArea();
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

        public void LoadAddress(string x) {
            //if (int.TryParse(x, out int number)) {
            //    //await DisplayAlert("Alert", i.ToString(), "OK");
            //    UserAddress address = AddressDatabase.GetItemById(number);
            //    SetAddressView(address);
            //}
        }

        public void SetAddressView(UserAddress address) {
            //string cryptoName = address.crypto;
            //Header.Text = cryptoName;
            //TopLeftIcon.Source = Currency.CryptocurrencyList.cryptocurrencies[cryptoName].imageFile;
            //GivenName.Text = address.name;
            //CryptoAddress.Text = address.address; //.yeah well done here rofl
            // Etherscan image source
            // Feed box changes
        }
    }
}
