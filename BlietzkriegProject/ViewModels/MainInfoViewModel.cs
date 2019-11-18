using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Client;
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
            SelectedItem = StationManager.CurrentUser.Accounts.First();
            LoaderManeger.Instance.ShowLoader();

            Transactions = new ObservableCollection<Transaction>();
            GetTransactions();
            LoaderManeger.Instance.HideLoader();
        }

        private async void GetTransactions()
        {
            var allTransactions = await RestClient.GetTransfers();
            var specificTransactions = (from transaction in allTransactions
                where transaction.From == SelectedItem.CardNumber || transaction.To == SelectedItem.CardNumber
                select transaction);
            foreach (var specificTransaction in specificTransactions)
                Transactions.Add(specificTransaction);
        }
        private void AccountChangeImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            GetTransactions();
            LoaderManeger.Instance.HideLoader();
        }


    }
}