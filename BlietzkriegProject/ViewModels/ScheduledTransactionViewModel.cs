using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Client;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    public class ScheduledTransactionViewModel:BaseViewModel
    {
        private List<Account> _accountType;
        private Account _selectedItems;
        
        private string _sum;
        private string _cardNumber;
        private string _period;

        private RelayCommand _addEditCommand;
        private RelayCommand _cancelCommand;
        private readonly static ScheduledTransactionViewModel instance = new ScheduledTransactionViewModel();


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
                _addEditCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();

            }
        }

       

        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(Period) || string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(Amount) || AccountSelected == null) return false;
            return CanExecuteMakeSum();
        }
        private bool CanExecuteMakeSum()
        {
//            return true;
            return CanExecuteCardNumber() && Amount.All(char.IsDigit) && Period.All(char.IsDigit);
        }
        private bool CanExecuteCardNumber()
        {
            return CardNumber.All(char.IsDigit) && CardNumber.Length == 16;
        }

        public string Amount
        {
            get { return _sum; }
            set
            {
                _sum = value;
                _addEditCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        public string Period
        {
            get { return _period; }
            set
            {
                _period = value;
                _addEditCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
       
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                _addEditCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        

        public RelayCommand AddEditCommand
        {
            get
            {
                return _addEditCommand ??
                       (_addEditCommand = new RelayCommand(AddEditTransactionImplementation, () => CanExecuteCommand()));
            }
        }
        public RelayCommand CancelCommand
        {
            get { return _cancelCommand = new RelayCommand(() =>
            {
                NavigationManager.Instance.Navigate(ViewType.Transactions);
            }); }
        }

        private ScheduledTransactionViewModel()
        {
            AccountType = StationManager.CurrentUser.Accounts.ToList();
        }

        public void Update()
        {
            AccountType = StationManager.CurrentUser.Accounts.ToList();
            AccountSelected = null;
            CardNumber = null;
            Amount = null;
            Period = null;
        }

        public static ScheduledTransactionViewModel GetInstance()
        {
            return instance;
        }

        private async void AddEditTransactionImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            HttpStatusCode responseCode;
            try
            {
                responseCode = await RestClient.PostScheduled(
                    new ScheduleInput(AccountSelected.CardNumber, CardNumber, Int32.Parse(Amount), Int32.Parse(Period)));
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
                var dialog = new MessageDialog("Scheduled transaction successfuly added", "Success");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                Update();
                TransactionViewModel.GetInstance().Update();
                NavigationManager.Instance.Navigate(ViewType.Transactions);
            }
            else
            {
                var dialog = new MessageDialog("Error occured while trying to add scheduled transaction", "Failure");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                Update();
            }
        }
    }
}