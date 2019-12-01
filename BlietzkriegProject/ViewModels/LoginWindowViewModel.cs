using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using UI.Client;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    public class LoginWindowViewModel : BaseViewModel
    {
        #region Fields

        private string _cardNumber;
        private string _password;

        #endregion

        #region Commands

        private RelayCommand _googleCommand;
        private RelayCommand _closeCommand;

        #endregion

        #region Properties

        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                _googleCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                _googleCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public int Tries { get; set; }

        #region Commands

        public RelayCommand GoogleCommand
        {
            get
            {
                return _googleCommand ??
                       (_googleCommand = new RelayCommand(SignInImplementation, () => CanExecuteCommand()));
            }
        }

        public RelayCommand CloseCommand
        {
            get { return _closeCommand = new RelayCommand(() => CoreApplication.Exit()); }
        }

        #endregion

        #endregion

        private bool CanExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(Password)) return false;
            return CanExecuteCardNumber() && CanExecutePin();
            ;
        }

        private bool CanExecuteCardNumber()
        {
            return CardNumber.All(char.IsDigit) && CardNumber.Length == 16;
        }

        private bool CanExecutePin()
        {
            return Password.All(char.IsDigit) && Password.Length == 4;
        }
        private async void SignInImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            bool flag = true;
            User user;
            try
            {
                user = await RestClient.AuthPost(new Card(CardNumber, Password));
            }
            catch(System.Net.Http.HttpRequestException)
            {
                var internetError = new MessageDialog("Missing internet connection", "Failure");
                internetError.Commands.Add(new UICommand("Ok", null));
                await internetError.ShowAsync();
                LoaderManeger.Instance.HideLoader();
                return;
            }

            if (user != null)
                StationManager.CurrentUser = user;
            else
                flag = false;

            if (flag)
            {
                var dialog = new MessageDialog("CardNumber and PIN correct. Login successful. User: "+StationManager.CurrentUser.Login,
                    "Success");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                CardNumber = null;
                Password = null;
                NavigationManager.Instance.Navigate(ViewType.Google);
            }
            else
            {
                var errorDialog = new MessageDialog("Login failed", "Failure");
                errorDialog.Commands.Add(new UICommand("Ok", null));
                await errorDialog.ShowAsync();
                CardNumber = null;
                Password = null;
            }
            LoaderManeger.Instance.HideLoader();
        }
    }
}