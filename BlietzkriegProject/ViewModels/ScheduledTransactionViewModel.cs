using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Models;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    public class ScheduledTransactionViewModel:BaseViewModel
    {
        private List<string> _accountType;
        private string _selectedItems;
        
        private string _sum;
        private string _cardNumber;
        private string _period;

        private RelayCommand _addEditCommand;
        private string _addEdit;
       

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
                _addEditCommand.RaiseCanExecuteChanged();
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
        public string AddEdit
        {
            get { return _addEdit; }
            set
            {
                _addEdit = value;
                OnPropertyChanged();
            }
        }
        public string CardNumberM
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                _addEditCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        private bool CanExecuteMakeSum()
        {
            return CanExecuteCardNumber() && AmountM.All(char.IsDigit)&& Period.All(char.IsDigit);
        }
        private bool CanExecuteCardNumber()
        {
            return CardNumberM.All(char.IsDigit) && CardNumberM.Length == 16;
        }

        public RelayCommand AddEditCommand
        {
            get
            {
                return _addEditCommand ??
                       (_addEditCommand = new RelayCommand(AddEditTransactionImplementation, () => CanExecuteCommand()));
            }
        }

        public ScheduledTransactionViewModel()
        {
            _addEdit="Add";
            AccountType = AccountNames.Accounts;
        }
        

        private async void AddEditTransactionImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO  add or edit transaction

            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO add or edit transaction", "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Transactions);
        }
    }
}