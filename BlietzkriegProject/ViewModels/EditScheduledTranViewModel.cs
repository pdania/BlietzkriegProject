using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    public class EditScheduledTranViewModel:BaseViewModel
    {
        private List<Account> _accountType;
        private Account _selectedItems;

        private string _sum;
        private string _cardNumber;
        private string _period;

        private RelayCommand _addEditCommand;
        private RelayCommand _cancelCommand;


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
            if (string.IsNullOrWhiteSpace(Period) || string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(Amount) || string.IsNullOrWhiteSpace(AccountSelected.ShowInCombobox)) return false;
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


        public RelayCommand EditCommand
        {
            get
            {
                return _addEditCommand ??
                       (_addEditCommand = new RelayCommand(EditTransactionImplementation, () => CanExecuteCommand()));
            }
        }
        public RelayCommand CancelCommand
        {
            get { return _cancelCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Transactions)); }
        }

        public EditScheduledTranViewModel()
        {
            ScheduleTranferDto tranferDto = StationManager.CurrentScheduledTransfer;
            _cardNumber = tranferDto.CardNumberTo;
            _sum = tranferDto.Amount.ToString();
            _period = tranferDto.Period.ToString();
            _selectedItems = (from acc in StationManager.CurrentUser.Accounts
                where acc.CardNumber == tranferDto.CardNumberFrom
                select acc).First();
            AccountType = StationManager.CurrentUser.Accounts.ToList();
        }


        private async void EditTransactionImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO  edit scheduled transaction

            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO edit transaction", "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Transactions);
        }
    }
}