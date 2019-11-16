using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;
using BlietzkriegProject.Tools;
using BlietzkriegProject.Tools.Managers;
using BlietzkriegProject.Tools.Navigation;

namespace BlietzkriegProject.ViewModels
{
    public class LoginWindowViewModel:BaseViewModel
    {
        #region Fields
        private string _cardNumber;
        private string _password;
        private string _information;

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
        public string Information
        {
            get { return _information; }
            set => _information = value;
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

        #region Commands
        public RelayCommand GoogleCommand
        {
            get { return _googleCommand ?? (_googleCommand =  new RelayCommand(SignInImplementation, () => CanExecuteCommand())); }
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
            return CanExecuteCardNumber() && CanExecutePin(); ;
        }

        private bool CanExecuteCardNumber()
        {
            return CardNumber.All(char.IsDigit) && CardNumber.Length == 16;
        }

        private bool CanExecutePin()
        {
            return Password.All(char.IsDigit) && Password.Length == 4;
        }

        public LoginWindowViewModel()
        {
            Information = "Input card number and PIN";
        }

        private async void SignInImplementation()
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() =>
            {
                //var user = RestClient.AuthPost(CardNumber, Password);
            });
            LoaderManeger.Instance.HideLoader();
            var dialog = new MessageDialog("CardNumber successful for user //TODO name of gotten user", "Success");
            dialog.Commands.Add(new UICommand("Ok", null));
            await dialog.ShowAsync();
            NavigationManager.Instance.Navigate(ViewType.Google);
        }
    }
}