using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;
using BlietzkriegProject.Tools.Navigation;

namespace BlietzkriegProject.ViewModels
{
    internal class GoogleAuthenticatorViewModel:BaseViewModel
    {
        private string _authenticatorCode;

        public string AuthenticatorCode
        {
            get => _authenticatorCode;
            set
            {
                _authenticatorCode = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand<object> _signInCommand;
        private RelayCommand<object> _backCommand;

        #region Commands

        public RelayCommand<object> SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(
                           SignInInplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand<object>(
                           o =>
                           {
                               NavigationManager.Instance.Navigate(ViewType.Login);

                           }));
            }
        }

        #endregion

        private bool CanExecuteCommand()
        {
            return AuthenticatorCode.All(char.IsDigit) && AuthenticatorCode.Length == 6;
        }

        private async void SignInInplementation(object obj)
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(2000).Wait();
                //TODO Send Get request to DB 
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog($"Authenticator code successful for user //TODO name of gotten user");
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Main);
        }
    }
}