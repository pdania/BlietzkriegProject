using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;
using BlietzkriegProject.Tools.Navigation;

namespace BlietzkriegProject.ViewModels
{
    internal class LoginWindowViewModel:BaseViewModel
    {
        #region Fields
        private string _cardNumber;
        private string _password;
        private string _information;

        #region Commands
        private RelayCommand<object> _signInCommand;
        private RelayCommand<object> _closeCommand;
        #endregion
        #endregion

        #region Properties
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                OnPropertyChanged();
            }
        }
        public string Information
        {
            get { return _information; }
            set
            {
                _information = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        public RelayCommand<object> SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(
                           SignInInplementation));
            }
        }

        public RelayCommand<object> CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand<object>(o => CoreApplication.Exit()));
            }
        }

        #endregion
        #endregion

        private bool CanExecuteCommand()
        {
            return CanExecuteCardNumber() && CanExecutePin();
        }

        private bool CanExecuteCardNumber()
        {
            return CardNumber.All(char.IsDigit) && CardNumber.Length == 16;
        }

        private bool CanExecutePin()
        {
            return Password.All(char.IsDigit) && CardNumber.Length == 4;
        }

        private async void SignInInplementation(object obj)
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(2000).Wait();
                //TODO Connection to Google Authentificator
            });
            LoaderManeger.Instance.HideLoader();
            var dialog  = new MessageDialog($"CardNumber successful for user //TODO name of gotten user");
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Google);
        }
    }
}