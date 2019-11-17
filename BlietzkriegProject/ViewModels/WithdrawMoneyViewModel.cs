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
    internal class WithdrawMoneyViewModel : BaseViewModel
    {
        #region Fields
        private string _sum;
        private Account _selectedItems;
        private string _information;
        private List<Account> _accountType;

        #endregion

        #region Commands
        private RelayCommand _withdrawCommand;
        private RelayCommand _cancelCommand;

        #endregion
        public string WithdrawSum
        {
            get { return _sum; }
            set
            {
                _sum = value;
                _withdrawCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

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
                _withdrawCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();

            }
        }



        #region Commands
        public RelayCommand WithdrawCommand
        {
            get { return _withdrawCommand ?? (_withdrawCommand = new RelayCommand(WithdrawMoneyImplementation, () => CanExecuteCommand())); }
        }

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Dashboard)); }
        }

        #endregion

        public WithdrawMoneyViewModel()
        {
            AccountType = StationManager.CurrentUser.Accounts;
        }
        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(WithdrawSum) || string.IsNullOrWhiteSpace(AccountSelected.ShowInCombobox)) return false;
            return CanExecuteCardNumber();
        }
        private bool CanExecuteCardNumber()
        {
            return WithdrawSum.All(char.IsDigit);
        }
        private async void WithdrawMoneyImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO Withdraw money on account
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO sum withdraw from account=" + AccountSelected, "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Withdraw);
        }

    }
}