using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Web.Http;
using UI.Client;
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
        private List<Account> _accountType;
        private string _sum;
        private Account _selectedItems;
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
            AccountType = StationManager.CurrentUser.Accounts.ToList();
        }
        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(PutSum) || string.IsNullOrWhiteSpace(AccountSelected.ShowInCombobox)) return false;
            return CanExecutePutSum();
        }
        private bool CanExecutePutSum()
        {
            return PutSum.All(char.IsDigit);
        }
        private async void PutMoneyImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            MessageDialog errorDialog;
            HttpStatusCode responseCode;
            try
            {
                responseCode = await RestClient.PutMoney(new MoneyTo(AccountSelected.CardNumber, Int32.Parse(PutSum)));
            }
            catch (System.Net.Http.HttpRequestException)
            {
                var internetError = new MessageDialog("Missing internet connection", "Failure");
                internetError.Commands.Add(new UICommand("Ok", null));
                await internetError.ShowAsync();
                LoaderManeger.Instance.HideLoader();
                return;
            }
            if (responseCode == HttpStatusCode.OK)
            {
                var dialog = new MessageDialog("Operation is successful for account "+AccountSelected.CardNumber+", type "+AccountSelected.Type, "Success");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                foreach (var currentUserAccount in StationManager.CurrentUser.Accounts)
                {
                    if (currentUserAccount.CardNumber.Equals(AccountSelected.CardNumber))
                        currentUserAccount.Balance += Int32.Parse(PutSum);
                }
                AccountType = StationManager.CurrentUser.Accounts.ToList();
                AccountSelected = null;
                PutSum = null;
            }
            else
            {
                errorDialog = new MessageDialog("Error occured while trying to put money", "Failed");
                errorDialog.Commands.Add(new UICommand("Ok", null));
                await errorDialog.ShowAsync();
            }
            LoaderManeger.Instance.HideLoader();
        }
    }
}