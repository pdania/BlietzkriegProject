using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UI.Client;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;
using UI.Views;

namespace UI.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
        private bool _toAnotherCard;

//        private List<string> _transction;
        private List<string> transactionList;


        private RelayCommand _backCommand;
//
//        private Visibility vis = Visibility.Visible;
//        private Visibility inVis = Visibility.Collapsed;

        private Visibility _scheduled;
        private Visibility _make;
        private Visibility _history;

        private string _selectedItem;
        private List<Account> _accountType;
        private Account _selectedItems;
        private RelayCommand _makeTranCommand;
        private string _sum;
        private string _cardNumber;
        private ObservableCollection<Transaction> _transactionsHistory;
        private ObservableCollection<ScheduleTranferDto> _scheduledTran;
        private ScheduleTranferDto _selectedTran;
        private RelayCommand _addCommand;
        private RelayCommand _editCommand;
        private RelayCommand _removeCommand;
        private RelayCommand _cancelCommand;
        private Visibility _myCard;
        private Visibility _toAnother;
        private Account _toAccountSelected;
        private readonly static TransactionViewModel instance = new TransactionViewModel();

        //        public RelayCommand CancelCommand
        //        {
        //            get { return _backCommand = new RelayCommand(ChangeWindow(_window)); }
        //        }

        #region Visability

        public Visibility ScheduledTranVisibility
        {
            get => _scheduled;
            set
            {
                _scheduled = value;
                OnPropertyChanged();
            }
        }


        public Visibility MakeTranVisibility
        {
            get => _make;
            set
            {
                _make = value;
                OnPropertyChanged();
            }
        }

        public Visibility TranHistoryVisibility
        {
            get => _history;
            set
            {
                _history = value;
                OnPropertyChanged();
            }
        }


        public List<string> TransactionList
        {
            get => transactionList;
            set => transactionList = value;
        }

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == _selectedItem) return;
                _selectedItem = value;
                ChangeWindow(transactionList.IndexOf(_selectedItem));
                OnPropertyChanged();
            }
        }

        #endregion

        #region MakeTransaction

        public List<Account> AccountType
        {
            get => _accountType;
            set
            {
                _accountType = value;
                OnPropertyChanged();
            }
        }

        public Account AccountSelected
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                _makeTranCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private bool CanExecuteCommand()
        {
            if (_toAnotherCard)
            {
                if (string.IsNullOrWhiteSpace(AmountM) || string.IsNullOrWhiteSpace(CardNumberM) || AccountSelected == null)
                    return false;
                return CanExecuteMakeSum();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(AmountM) || AccountSelected == null || ToMyAccountSelected == null )
                    return false;
                return AmountM.All(char.IsDigit) &&
                       (ToMyAccountSelected.ShowInCombobox != AccountSelected.ShowInCombobox);
            }
        }

        public string AmountM
        {
            get { return _sum; }
            set
            {
                _sum = value;
                _makeTranCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }


        public string CardNumberM
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                _makeTranCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private bool CanExecuteMakeSum()
        {
            return CanExecuteCardNumber() && AmountM.All(char.IsDigit);
        }

        private bool CanExecuteCardNumber()
        {
            return CardNumberM.All(char.IsDigit) && CardNumberM.Length == 16;
        }

        #region ToMyToAnotherAccount

        public bool ToAccount
        {
            get => _toAnotherCard;
            set
            {
                _toAnotherCard = value;
                ChangeToAccount(_toAnotherCard);
                _makeTranCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public Account ToMyAccountSelected
        {
            get { return this._toAccountSelected; }
            set
            {
                this._toAccountSelected = value;
                _makeTranCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public Visibility ToMyCardVisibility
        {
            get => _myCard;
            set
            {
                _myCard = value;
                OnPropertyChanged();
            }
        }

        public Visibility ToAnotherCardVisibility
        {
            get => _toAnother;
            set
            {
                _toAnother = value;
                OnPropertyChanged();
            }
        }

        private void ChangeToAccount(bool toAnotherCard)
        {
            if (toAnotherCard)
            {
                CardNumberM = null;
                ToMyCardVisibility = Visibility.Collapsed;
                ToAnotherCardVisibility = Visibility.Visible;
            }
            else
            {
                ToMyAccountSelected = null;
                ToMyCardVisibility = Visibility.Visible;
                ToAnotherCardVisibility = Visibility.Collapsed;
            }

            _makeTranCommand.RaiseCanExecuteChanged();
        }

        #endregion

        public RelayCommand MakeTranCommand
        {
            get
            {
                return _makeTranCommand ??
                       (_makeTranCommand = new RelayCommand(MakeTransactionImplementation, () => CanExecuteCommand()));
            }
        }


        private async void MakeTransactionImplementation()
        {
            bool flag = false;
            LoaderManeger.Instance.ShowLoader();
            HttpStatusCode result;
            Receiver receiver = null;
            if (_toAnotherCard)
            {
                try
                {
                    receiver = await RestClient.GetAccountInfo(CardNumberM);
                    
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    var internetError = new MessageDialog("Missing internet connection", "Failure");
                    internetError.Commands.Add(new UICommand("Ok", null));
                    await internetError.ShowAsync();
                    LoaderManeger.Instance.HideLoader();
                    return;
                }

                if (receiver == null)
                {
                    var errorDialog =
                        new MessageDialog(
                            "Transaction failed to " + CardNumberM + "\n Reason: such user don't exist",
                            "Failure");
                    errorDialog.Commands.Add(new UICommand("Ok", null));
                    await errorDialog.ShowAsync();
                    ClearFields();
                }
                else 
                {
                    var chooseDialog =
                        new MessageDialog(
                            "Are you sure you want to send "+AmountM+"hrn to " + CardNumberM + " with user "+receiver+"?",
                            "Accept operation");
                    chooseDialog.Commands.Add(new UICommand("Yes", null));
                    chooseDialog.Commands.Add(new UICommand("Cancel", null));
                    var choose = await chooseDialog.ShowAsync();
                    if (choose.Label == "Yes")
                    {
                        result = await RestClient.TransferMoney(new TranferInput(AccountSelected.CardNumber,
                            CardNumberM,
                            Int32.Parse(AmountM)));
                        if (result == HttpStatusCode.OK)
                        {
                            var errorDialog =
                                new MessageDialog(
                                    "Transaction is successful to " + CardNumberM +
                                    " with user "+receiver,
                                    "Success");
                            errorDialog.Commands.Add(new UICommand("Ok", null));
                            await errorDialog.ShowAsync();
                            SyncBalance();
                            ClearFields();
                        }
                        else flag = true;
                    }
                }
            }
            else
            {
                try
                {
                    result = await RestClient.TransferMoney(new TranferInput(AccountSelected.CardNumber,
                        ToMyAccountSelected.CardNumber,
                        Int32.Parse(AmountM)));
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    var internetError = new MessageDialog("Missing internet connection", "Failure");
                    internetError.Commands.Add(new UICommand("Ok", null));
                    await internetError.ShowAsync();
                    LoaderManeger.Instance.HideLoader();
                    return;
                }

                if (result == HttpStatusCode.OK)
                {
                    var errorDialog =
                        new MessageDialog(
                            "Transaction is successful to your card " + ToMyAccountSelected.CardNumber + " type " +
                            ToMyAccountSelected.Type, "Success");
                    errorDialog.Commands.Add(new UICommand("Ok", null));
                    await errorDialog.ShowAsync();
                    SyncBalance(true);
                }
                else flag = true;
            }

            if (flag)
            {
                var dialog = new MessageDialog("Transaction failed", "Failure");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                ClearFields();
            }
            LoaderManeger.Instance.HideLoader();
        }

        private void ClearFields()
        {
            AccountSelected = null;
            AmountM = null;
            AccountType = StationManager.CurrentUser.Accounts.ToList();
            CardNumberM = null;
        }
        #endregion

        #region TransactionHistory

        private void SyncBalance(bool isMine = false)
        {
            foreach (var currentUserAccount in StationManager.CurrentUser.Accounts)
            {
                if (currentUserAccount.CardNumber.Equals(AccountSelected.CardNumber))
                    currentUserAccount.Balance -= Int32.Parse(AmountM);
                if (isMine)
                    if (currentUserAccount.CardNumber.Equals(ToMyAccountSelected.CardNumber))
                        currentUserAccount.Balance += Int32.Parse(AmountM);
            }
        }

        public ObservableCollection<Transaction> TransactionsHistory
        {
            get => _transactionsHistory;
            set
            {
                _transactionsHistory = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ScheduledTransaction

        public ObservableCollection<ScheduleTranferDto> ScheduledTran
        {
            get => _scheduledTran;
            set
            {
                _scheduledTran = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                       (_addCommand = new RelayCommand(() =>
                           NavigationManager.Instance.Navigate(ViewType.ScheduledTransaction)));
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ??
                       (_editCommand =
                           new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.EditScheduledTran),
                               () => CanExecute()));
            }
        }

        private bool CanExecute()
        {
            return _selectedTran != null;
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                       (_removeCommand = new RelayCommand(RemoveScheduledTranImplementation, () => CanExecute()));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           new RelayCommand(() =>
                           {
                               ClearFields();
                               NavigationManager.Instance.Navigate(ViewType.Dashboard);
                           }));
            }
        }

        private async void RemoveScheduledTranImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            HttpStatusCode responseCode;
            try
            {
                responseCode = await RestClient.DeleteScheduled(SelectedTransaction.TranferId);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                var internetError = new MessageDialog("Missing internet connection", "Failure");
                internetError.Commands.Add(new UICommand("Ok", null));
                await internetError.ShowAsync();
                LoaderManeger.Instance.HideLoader();
                return;
            }
            LoaderManeger.Instance.HideLoader();
            if (responseCode == HttpStatusCode.OK)
            {
                var dialog = new MessageDialog("Scheduled transaction successfuly removed", "Success");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                Update();
            }
            else
            {
                var dialog = new MessageDialog("Error occured while trying to add scheduled transaction", "Failure");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
            }
        }

        public ScheduleTranferDto SelectedTransaction
        {
            get { return this._selectedTran; }
            set
            {
                if (value == _selectedTran) return;
                this._selectedTran = value;

                _selectedTran = value;
                StationManager.CurrentScheduledTransfer = _selectedTran;
                EditCommand.RaiseCanExecuteChanged();
                RemoveCommand.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        #endregion


        private TransactionViewModel()
        {
            LoaderManeger.Instance.ShowLoader();
            MakeTranVisibility = Visibility.Visible;
            ScheduledTranVisibility = Visibility.Collapsed;
            TranHistoryVisibility = Visibility.Collapsed;
            ToAnotherCardVisibility = Visibility.Collapsed;
            ToMyCardVisibility = Visibility.Visible;
            transactionList = new List<string>();
            transactionList.Add("Make Transaction");
            transactionList.Add("Scheduled Transaction");
            transactionList.Add("Transaction history");
            SelectedItem = transactionList.First();
            AccountType = StationManager.CurrentUser.Accounts.ToList();
            GetTransactions();
            GetScheduledTransactions();
            LoaderManeger.Instance.HideLoader();
        }

        public static TransactionViewModel GetInstance()
        {
            return instance;
        }

        public void Update()
        {
            LoaderManeger.Instance.ShowLoader();
            GetScheduledTransactions();
            LoaderManeger.Instance.HideLoader();
        } 
        private async void GetScheduledTransactions()
        {
            ScheduledTran = new ObservableCollection<ScheduleTranferDto>();
            ObservableCollection<ScheduleTranferDto> allTransactions;
            try
            {
                allTransactions = await RestClient.GetScheduledTransfers();
            }
            catch (System.Net.Http.HttpRequestException)
            {
                var internetError = new MessageDialog("Missing internet connection", "Failure");
                internetError.Commands.Add(new UICommand("Ok", null));
                await internetError.ShowAsync();
                LoaderManeger.Instance.HideLoader();
                return;
            }

            foreach (var specificTransaction in allTransactions)
            {
                ScheduledTran.Add(specificTransaction);
            }
        }

        private async void GetTransactions()
        {
            TransactionsHistory = new ObservableCollection<Transaction>();
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

            foreach (var specificTransaction in allTransactions)
            {
                if (specificTransaction.To == null)
                {
                    specificTransaction.To = "ATM";
                    TransactionsHistory.Add(specificTransaction);
                    continue;
                }

                if (specificTransaction.From == null)
                {
                    specificTransaction.From = "ATM";
                    TransactionsHistory.Add(specificTransaction);
                    continue;
                }

                TransactionsHistory.Add(specificTransaction);
            }
        }

        private void ChangeWindow(int n)
        {
            MakeTranVisibility = Visibility.Collapsed;
            ScheduledTranVisibility = Visibility.Collapsed;
            TranHistoryVisibility = Visibility.Collapsed;
            switch (n)
            {
                case 0:
                    MakeTranVisibility = Visibility.Visible;
                    break;
                case 1:
                    LoaderManeger.Instance.ShowLoader();
                    GetScheduledTransactions();
                    LoaderManeger.Instance.HideLoader();
                    ScheduledTranVisibility = Visibility.Visible;
                    break;
                case 2:
                    LoaderManeger.Instance.ShowLoader();
                    GetTransactions();
                    LoaderManeger.Instance.HideLoader();
                    TranHistoryVisibility = Visibility.Visible;
                    break;
                default:
                    MakeTranVisibility = Visibility.Collapsed;
                    ScheduledTranVisibility = Visibility.Collapsed;
                    TranHistoryVisibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}