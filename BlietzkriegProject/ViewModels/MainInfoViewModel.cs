using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BlietzkriegProject.Models;
using BlietzkriegProject.Templates;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;
using BlietzkriegProject.Tools.Navigation;

namespace BlietzkriegProject.ViewModels
{
    internal class MainInfoViewModel : BaseViewModel
    {
        #region Fields
        private List<string> _accountType;
        private string userName;
        private string _selectedItem;


        #endregion

        #region Commands
        private RelayCommand _naviCommand;
        private RelayCommand _backCommand;
        private ObservableCollection<Transaction> _transactions;

        #endregion

        #region Commands
        public RelayCommand NaviCommand
        {
            get { return _naviCommand ?? (_naviCommand = new RelayCommand(() => AccountChangeImplementation())); }
        }

        public RelayCommand CancelCommand
        {
            get { return _backCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Dashboard)); ; }
        }

        #endregion


        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == _selectedItem) return;
                _selectedItem = value;
                OnPropertyChanged();
                AccountChangeImplementation();
                


            }
        }
        public List<string> AccountType
        {
            get => _accountType;
            set => _accountType = value;
        }
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => _transactions = value;
        }
        public MainInfoViewModel()
        {
            AccountType = AccountNames.Accounts;
            Transactions = new ObservableCollection<Transaction>();
            Transactions.Add(item: new Transaction("From","To",2,DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andr546444444444444444444444444444444444444444444444444y", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
            Transactions.Add(item: new Transaction("Andry", "Dania", 2, DateTime.Now, Guid.NewGuid()));
        }
        private async void AccountChangeImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO Put money on account
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Operation is successful //TODO sum put on account="+ _selectedItem, "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.MainInfo);
        }


    }
}