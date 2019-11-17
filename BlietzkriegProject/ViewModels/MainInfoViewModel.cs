using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    internal class MainInfoViewModel : BaseViewModel
    {
        #region Fields
        private List<Account> _accountType;
        private string userName;
        private Account _selectedItem;


        #endregion

        #region Commands
        private RelayCommand _naviCommand;
        private RelayCommand _backCommand;
        private ObservableCollection<Transaction> _transactions;
        private string _limit;
        private string _balance;
        private string _percent;

        #endregion

        #region Commands
        public RelayCommand NaviCommand
        {
            get { return _naviCommand ?? (_naviCommand = new RelayCommand(() => AccountChangeImplementation())); }
        }

        public RelayCommand CancelCommand
        {
            get { return _backCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Dashboard)); ; }
        }

        #endregion


        public Account SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == _selectedItem) return;
                _selectedItem = value;
                OnPropertyChanged();
                AccountChangeImplementation();
            }
        }

        public string Limit
        {
            get { return _limit;}
            set { _limit = value; }
        }
        public string Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
        public string Percent
        {
            get { return _percent; }
            set { _percent = value; }
        }
        public List<Account> AccountType
        {
            get => _accountType;
            set => _accountType = value;
        }
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => _transactions = value;
        }
        public MainInfoViewModel()

        {
            AccountType = StationManager.CurrentUser.Accounts.ToList();
            Transactions = new ObservableCollection<Transaction>();
            Transactions.Add(item: new Transaction("From","To",2,DateTime.Now));
            Transactions.Add(item: new Transaction("Andr546444444444444444444444444444444444444444444444444y", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));

            //TODO add to this strings value
            _limit = "Limit: ";
            _balance = "Balance: ";
            _percent = "Percent: ";
        }
        private async void AccountChangeImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO Put money on account
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO sum put on account="+ _selectedItem, "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.MainInfo);
        }


    }
}