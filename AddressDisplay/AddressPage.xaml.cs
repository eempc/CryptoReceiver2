using AddressDisplay.Address;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AddressDisplay {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressPage : ContentPage {
        static ObservableCollection<ListViewUserAddress> userAddresses = new ObservableCollection<ListViewUserAddress>(); // An observable collection is needed in order for the listview to auto update, not a List
        List<string> cryptoList = new List<string>(); // This is for the picker
        int updateIdGlobal; // this variable updated by tappedItem.id global variable, not the best way to do it

        public AddressPage() {
            InitializeComponent();
            InitialiseCreatePopUp();
            InitialiseAddressListView();
            PopulateListView();
        }

        // Setting up the invisible pop up
        private void InitialiseCreatePopUp() {
            Overlay.IsVisible = false;
            cryptoList = Currency.CryptocurrencyList.GetCryptoList();
            CryptoPicker.ItemsSource = cryptoList.OrderBy(s => s).ToList();
        }

        public static void InitialiseAddressListView() {
            userAddresses.Clear();
            // First retrieve the raw UserAddress list from the database
            List<UserAddress> tempList = new List<UserAddress>(AddressDatabase.ReadDatabaseUserAddress());

            // Then cast it to the ListViewUserAddress and add the icon's path because I have yet to discover how to cast between base and derived classes
            foreach (UserAddress userAddress in tempList) {
                ListViewUserAddress addressToBeAdded = new ListViewUserAddress();
                addressToBeAdded.Id = userAddress.Id;
                addressToBeAdded.Name = userAddress.Name;
                addressToBeAdded.Address = userAddress.Address;
                addressToBeAdded.Crypto = userAddress.Crypto;
                addressToBeAdded.CryptoIconPath = Currency.CryptocurrencyList.cryptocurrencies[addressToBeAdded.Crypto].ImageFile; // Was this really the best way to do this?

                userAddresses.Add(addressToBeAdded);
            }
        }

        // Then data binding here of the listview
        private void PopulateListView() => AddressesListView.ItemsSource = userAddresses; // Data binding

        // Add a new user address
        private void AddButton_Clicked(object sender, EventArgs e) {
            CreatePopUp("Create address");
            updateIdGlobal = 0; // This zero means this is a new address to be added
        }

        // Make pop up entry form visible. Populate if it is an edit
        private void CreatePopUp(string heading, string name = "", string address = "", int pickerIndex = -1) {
            Overlay.IsVisible = true;
            PopUpLabel.Text = heading;
            CryptoPicker.SelectedIndex = pickerIndex;
            AddressName.Text = name;
            EnterAddressField.Text = address;
        }

        // Simply paste an ETH address into the field
        private void PasteButton_Clicked(object sender, EventArgs e) => PasteAddress();

        private async void PasteAddress() {
            if (Clipboard.HasText) {
                string clipboardText = await Clipboard.GetTextAsync();
                EnterAddressField.Text = clipboardText;
            }
        }

        // Finish up the pop up and make it disappear
        private void CancelButton_Clicked(object sender, EventArgs e) => ClearPopUp();

        private void ClearPopUp() {
            Overlay.IsVisible = false;
            EnterAddressField.Text = "";
            AddressName.Text = "";
        }

        // Save address, either a new one or an edited one
        private void OkayButton_Clicked(object sender, EventArgs e) => SaveAddress();

        private void SaveAddress() {
            UserAddress address = new UserAddress(); // Create a new object and fill in the fields then send it off to save into the database
            address.Name = AddressName.Text;
            address.Address = EnterAddressField.Text;
            address.Crypto = CryptoPicker.SelectedItem.ToString();
            //address.cryptoIconPath = Currency.CryptocurrencyList.cryptocurrencies[CryptoPicker.SelectedItem.ToString()].imageFile; // This is a bad line
            address.Id = updateIdGlobal; // Update global id, if it is 0 then SavetoDatabase will create a new entry, it is set to 0 by Add Button. Could replace with default argument
            ClearPopUp();
            AddressDatabase.SaveToDatabase(address);
            InitialiseAddressListView(); // Refresh listview
        }

        // Tap an item in the list view and get options, one of which is to edit the address
        private async void AddressesListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListViewUserAddress tappedItem = (ListViewUserAddress)((ListView)sender).SelectedItem;
            string action = await DisplayActionSheet("Action on " + tappedItem.Name, "Cancel", null, "Delete", "Edit", "Copy address");

            if (action == "Delete") {
                AddressDatabase.DeleteFromDatabase(tappedItem.Id);
                userAddresses.Remove(userAddresses.Where(x => x.Id == tappedItem.Id).Single()); // x refers to each item in the collection
            } else if (action == "Copy address") {
                await Clipboard.SetTextAsync(tappedItem.Address);
            } else if (action == "Edit") {
                // Editing an existing address - get the index of the crypto in the picker so that it can be passed into the pop up to auto-populate
                int index = CryptoPicker.ItemsSource.IndexOf(tappedItem.Crypto);
                CreatePopUp("Edit address", tappedItem.Name, tappedItem.Address, index);
                updateIdGlobal = tappedItem.Id;
            }
        }
    }
}