using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Web.Http;
using UI.Client;
using UI.Models;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace UI.ViewModels
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
            get { return _cancelCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Dashboard)); }
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
            Account currentAccount = null;
            MessageDialog errorDialog;
            currentAccount = StationManager.GetAccount(AccountSelected);
            if (currentAccount == null)
            {
                errorDialog = new MessageDialog("You don't have such type of account", "Failed");
                errorDialog.Commands.Add(new UICommand("Ok", null));
                await errorDialog.ShowAsync();
                NavigationManager.Instance.Navigate(ViewType.Put);
                return;
            }
            var responseCode = await RestClient.PutMoney(new Money(currentAccount.CardNumber, Int32.Parse(PutSum)));
            if (responseCode == HttpStatusCode.OK)
            {
                var dialog = new MessageDialog("Operation is successful //TODO sum put on account=" + AccountSelected, "Success");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                NavigationManager.Instance.Navigate(ViewType.Put);
            }
            LoaderManeger.Instance.HideLoader();
            errorDialog = new MessageDialog("Error occured while trying to put money", "Failed");
            errorDialog.Commands.Add(new UICommand("Ok", null));
            await errorDialog.ShowAsync();
        }
    }
}