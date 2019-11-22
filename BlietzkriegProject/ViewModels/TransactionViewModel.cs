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
            set => _accountType = value;
        }

        public Account AccountSelected
        {
            get { return this._selectedItems; }
            set
            {
                this._selectedItems = value;
                _makeTranCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private bool CanExecuteCommand()
        {
            if (_toAnotherCard)
            {
                if (string.IsNullOrWhiteSpace(AmountM) || string.IsNullOrWhiteSpace(AccountSelected.ShowInCombobox))
                    return false;
                return CanExecuteMakeSum();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(AmountM) || string.IsNullOrWhiteSpace(ToMyAccountSelcted.ShowInCombobox))
                    return false;
                return AmountM.All(char.IsDigit) &&
                       (ToMyAccountSelcted.ShowInCombobox != AccountSelected.ShowInCombobox);
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

        public Account ToMyAccountSelcted
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
                ToMyCardVisibility = Visibility.Collapsed;
                ToAnotherCardVisibility = Visibility.Visible;
            }
            else
            {
                ToMyCardVisibility = Visibility.Visible;
                ToAnotherCardVisibility = Visibility.Collapsed;
            }
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
            if (_toAnotherCard)
            {
                try
                {
                    result = await RestClient.TransferMoney(new TranferInput(AccountSelected.CardNumber,
                        CardNumberM,
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

                //TODO Message Dialog with To account user name
                if (result == HttpStatusCode.OK)
                {
                    var errorDialog =
                        new MessageDialog(
                            "Transaction is successful to " + CardNumberM + " with user //TODO user name and email",
                            "Success");
                    errorDialog.Commands.Add(new UICommand("Ok", null));
                    await errorDialog.ShowAsync();
                }
                else flag = true;
            }
            else
            {
                try
                {
                    result = await RestClient.TransferMoney(new TranferInput(AccountSelected.CardNumber,
                        ToMyAccountSelcted.CardNumber,
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
                            "Transaction is successful to your card " + ToMyAccountSelcted.CardNumber + " type " +
                            ToMyAccountSelcted.Type, "Success");
                    errorDialog.Commands.Add(new UICommand("Ok", null));
                    await errorDialog.ShowAsync();
                }
                else flag = true;
            }

            LoaderManeger.Instance.HideLoader();
            if (flag)
            {
                var dialog = new MessageDialog("Transaction failed", "Failure");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
            }
        }

        #endregion

        #region TransactionHistory

        public ObservableCollection<Transaction> TransactionsHistory
        {
            get => _transactionsHistory;
            set => _transactionsHistory = value;
        }

        #endregion

        #region ScheduledTransaction

        public ObservableCollection<ScheduleTranferDto> ScheduledTran
        {
            get => _scheduledTran;
            set => _scheduledTran = value;
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
                           new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Dashboard)));
            }
        }

        private async void RemoveScheduledTranImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO  Remove scheduled transaction
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO remove transaction", "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Transactions);
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


        public TransactionViewModel()
        {
            MakeTranVisibility = Visibility.Visible;
            ScheduledTranVisibility = Visibility.Collapsed;
            TranHistoryVisibility = Visibility.Collapsed;
            ToAnotherCardVisibility = Visibility.Collapsed;
            transactionList = new List<string>();
            transactionList.Add("Make Transaction");
            transactionList.Add("Scheduled Transaction");
            transactionList.Add("Transaction history");
            AccountType = StationManager.CurrentUser.Accounts.ToList();
            LoaderManeger.Instance.ShowLoader();
            GetTransactions();
            LoaderManeger.Instance.HideLoader();
            ScheduledTran = new ObservableCollection<ScheduleTranferDto>();
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
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