using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using BlietzkriegProject.Models;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;

namespace BlietzkriegProject.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
        private bool _window = true;
        private List<string> _transction;
        private List<string> transactionList;


        private RelayCommand _backCommand;

        private Visibility vis = Visibility.Visible;
        private Visibility inVis = Visibility.Collapsed;

        private Visibility _scheduled;
        private Visibility _make;
        private Visibility _history;

        private string _selectedItem;
        private List<string> _accountType;
        private string _selectedItems;
        private RelayCommand _putCommand;
        private string _sum;

        //        public RelayCommand CancelCommand
        //        {
        //            get { return _backCommand = new RelayCommand(ChangeWindow(_window)); }
        //        }

        #region Visability

        public Visibility ScheduledTranVisibility
        {
            get => _scheduled;
            set {
               
                _scheduled = value;
                OnPropertyChanged();
            }
        }


        public Visibility MakeTranVisibility
        {
            get => _make;
            set {
               
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
        #endregion

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
        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(PutSum) || string.IsNullOrWhiteSpace(AccountSelected)) return false;
            return CanExecutePutSum();
        }

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
        private bool CanExecutePutSum()
        {
            return PutSum.All(char.IsDigit);
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