using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using BlietzkriegProject.Models;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;
using BlietzkriegProject.Tools.Navigation;

namespace BlietzkriegProject.ViewModels
{
    internal class WithdrawMoneyViewModel : BaseViewModel
    {
        #region Fields
        private string _sum;
        private string _selectedItems;
        private string _information;
        private List<string> _accountType;

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
            get { return _cancelCommand = new RelayCommand(() => CoreApplication.Exit()); }
        }

        #endregion

        public WithdrawMoneyViewModel()
        {
            AccountType = AccountNames.Accounts;
        }
        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(WithdrawSum) || string.IsNullOrWhiteSpace(AccountSelected)) return false;
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