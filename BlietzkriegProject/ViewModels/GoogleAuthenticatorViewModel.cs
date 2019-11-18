using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    internal class GoogleAuthenticatorViewModel:BaseViewModel
    {
        private string _authenticatorCode;

        public string AuthenticatorCode
        {
            get => _authenticatorCode;
            set
            {
                _authenticatorCode = value.Replace(" ", "Space") ;
                OnPropertyChanged();
                _signInCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _signInCommand;
        private RelayCommand _backCommand;

        #region Commands

        public RelayCommand SignInCommand
        {
            get
            {
                return _signInCommand = new RelayCommand(SignInImplementation, () => CanExecuteCommand()); ;
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand(
                           () =>
                           {
                               NavigationManager.Instance.Navigate(ViewType.Login);

                           }));
            }
        }

        #endregion

        private bool CanExecuteCommand()
        {
             if (string.IsNullOrWhiteSpace(AuthenticatorCode)) return false;
             return AuthenticatorCode.All(char.IsDigit) && AuthenticatorCode.Length == 6;
        }

        private async void SignInImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                //TODO Send Get request to DB 
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("Authenticator code successful for user: "+StationManager.CurrentUser.Login, "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Dashboard);
        }
    }
}