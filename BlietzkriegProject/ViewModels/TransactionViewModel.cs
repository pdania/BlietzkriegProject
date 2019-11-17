using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UI.Models;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;
using UI.Views;

namespace UI.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
//        private bool _window = true;
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
        private List<string> _accountType;
        private string _selectedItems;
        private RelayCommand _makeTranCommand;
        private string _sum;
        private string _cardNumber;
        private ObservableCollection<Transaction> _transactionsHistory;
        private ObservableCollection<ScheduleTranferDto> _scheduledTran;
        private ScheduleTranferDto _selectedTran;

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

        public List<string> AccountType
        {
            get => _accountType;
            set => _accountType = value;
        }

        public string AccountSelected
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
            if (string.IsNullOrWhiteSpace(AmountM) || string.IsNullOrWhiteSpace(AccountSelected)) return false;
            return CanExecuteMakeSum();
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
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO  Make transaction

            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO Make transaction", "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Transactions);
        }

        public ObservableCollection<Transaction> TransactionsHistory
        {
            get => _transactionsHistory;
            set => _transactionsHistory = value;
        }

        public ObservableCollection<ScheduleTranferDto> ScheduledTran
        {
            get => _scheduledTran;
            set => _scheduledTran = value;
        }

        public ScheduleTranferDto SelectedTransaction
        {
            get { return this._selectedTran; }
            set
            {
                this._selectedTran = value;
                if (value == _selectedTran) return;
                _selectedTran = value;
                OnPropertyChanged();
                EditTranImplementation();
            }
        }

        private void EditTranImplementation()
        {

            //TODo open new window

            NavigationManager.Instance.Navigate(ViewType.ScheduledTransaction);

        }
        


        public TransactionViewModel()
        {
            MakeTranVisibility = Visibility.Visible;
            ScheduledTranVisibility = Visibility.Collapsed;
            TranHistoryVisibility = Visibility.Collapsed;
            transactionList = new List<string>();
            transactionList.Add("Make Transaction");
            transactionList.Add("Scheduled Transaction");
            transactionList.Add("Transaction history");
            AccountType = AccountNames.Accounts;

            TransactionsHistory = new ObservableCollection<Transaction>();
            ScheduledTran = new ObservableCollection<ScheduleTranferDto>();
            TransactionsHistory.Add(item: new Transaction("From", "To", 2, DateTime.Now));
            TransactionsHistory.Add(item: new Transaction("Andr546444444444444444444444444444444444444444444444444y", "Dania", 2, DateTime.Now));
            TransactionsHistory.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            TransactionsHistory.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            TransactionsHistory.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now));
            ScheduledTran.Add(new ScheduleTranferDto(1,"45","56",45,DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
            ScheduledTran.Add(new ScheduleTranferDto(1, "45", "56", 45, DateTime.Now, 3));
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