﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Popups;
using UI.Client;
using UI.Templates;
using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    internal class GoogleAuthenticatorViewModel : BaseViewModel
    {
        private string _authenticatorCode;

        public string AuthenticatorCode
        {
            get => _authenticatorCode;
            set
            {
                _authenticatorCode = value;
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
                return _signInCommand = new RelayCommand(SignInImplementation, () => CanExecuteCommand());
                ;
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand(
                           () => NavigationManager.Instance.Navigate(ViewType.Login)));
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
            bool responseCode;
            try
            {
                responseCode = await RestClient.GoogleAuth(new Auth(AuthenticatorCode));
            }
            catch (System.Net.Http.HttpRequestException)
            {
                var internetError = new MessageDialog("Missing internet connection", "Failure");
                internetError.Commands.Add(new UICommand("Ok", null));
                await internetError.ShowAsync();
                LoaderManeger.Instance.HideLoader();
                return;
            }

            LoaderManeger.Instance.HideLoader();
            if (responseCode)
            {
                var dialog = new MessageDialog("Google authenticator code is correct. Welcome!", "Success");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                AuthenticatorCode = null;
                NavigationManager.Instance.Navigate(ViewType.Dashboard);
            }
            else
            {
                var dialog = new MessageDialog("Google authenticator code is incorrect. Try again", "Failure");
                dialog.Commands.Add(new UICommand("Ok", null));
                await dialog.ShowAsync();
                AuthenticatorCode = null;
            }
        }
    }
}