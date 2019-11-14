using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using BlietzkriegProject.Models;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;
using BlietzkriegProject.Tools.Navigation;

namespace BlietzkriegProject.ViewModels
{
    internal class PutMoneyViewModel:BaseViewModel
    {
        #region Fields
        private List<string> _accountType;
        private string _sum;
        private string _selectedItems;
        private string _information;

        #endregion

        #region Commands
        private RelayCommand _putCommand;
        private RelayCommand _cancelCommand;

        #endregion
        public string PutSum
        {
            get { return _sum; }
            set
            {
                _sum = value;
                _putCommand.RaiseCanExecuteChanged();
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
                _putCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();

            }
        }
        
        

        #region Commands
        public RelayCommand PutCommand
        {
            get { return _putCommand ?? (_putCommand = new RelayCommand(PutMoneyImplementation, () => CanExecuteCommand())); }
        }

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand = new RelayCommand(() => CoreApplication.Exit()); }
        }

        #endregion

        public PutMoneyViewModel()
        {
            AccountType = AccountNames.Accounts;
        }
        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(PutSum) || string.IsNullOrWhiteSpace(AccountSelected)) return false;
            return CanExecutePutSum();
        }
        private bool CanExecutePutSum()
        {
            return PutSum.All(char.IsDigit);
        }
        private async void PutMoneyImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO Put money on account
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO sum put on account="+AccountSelected, "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Put);
        }

    }
}