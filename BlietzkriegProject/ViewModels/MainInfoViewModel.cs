using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private User userName;
        private Account _selectedItem;
        private ObservableCollection<Transaction> _transactions;
        private Account _accountInfo;
        private static readonly MainInfoViewModel instance = new MainInfoViewModel();


        #endregion

        #region Commands
        private RelayCommand _naviCommand;
        private RelayCommand _backCommand;

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

        public User User
        {
            get =>userName;
            set =>userName=value;
        }
        public Account AccountInfo
        {
            get =>_accountInfo;
            set
            {
                _accountInfo = value; 
                OnPropertyChanged();
            }
        }

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
            set
            {
                _transactions = value; 
                OnPropertyChanged();
            }
        }

        private MainInfoViewModel()
        {}
        public static MainInfoViewModel GetInstance()
        {
            return instance;
        }

        public void SetDefault()
        {
            LoaderManeger.Instance.ShowLoader();
            User = StationManager.CurrentUser;
            AccountType = StationManager.CurrentUser.Accounts.ToList();
            SelectedItem = StationManager.CurrentUser.Accounts.First();
            LoaderManeger.Instance.HideLoader();
        }

        private async void GetTransactions()
        {
            Transactions = new ObservableCollection<Transaction>();
            ObservableCollection<Transaction> allTransactions;
            try
            {
                allTransactions = await RestClient.GetTransfers();
            }
            catch (System.Net.Http.HttpRequestException)
            {
                var internetError = new MessageDialog("Missing internet connection", "Failure");
                internetError.Commands.Add(new UICommand("Ok", null));
                await internetError.ShowAsync();
                LoaderManeger.Instance.HideLoader();
                return;
            }

            if (allTransactions != null)
            {
                var specificTransactions = (from transaction in allTransactions
                    where transaction.From == SelectedItem.CardNumber || transaction.To == SelectedItem.CardNumber
                    select transaction);
                foreach (var specificTransaction in specificTransactions)
                {
                    if (specificTransaction.To == null)
                    {
                        specificTransaction.To = "ATM";
                        Transactions.Add(specificTransaction);
                        continue;
                    }

                    if (specificTransaction.From == null)
                    {
                        specificTransaction.From = "ATM";
                        Transactions.Add(specificTransaction);
                        continue;
                    }

                    Transactions.Add(specificTransaction);
                }
            }
        }
        private void AccountChangeImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            GetTransactions();
            AccountInfo = SelectedItem;
            LoaderManeger.Instance.HideLoader();
        }


    }
}