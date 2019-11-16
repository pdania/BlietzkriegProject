using UI.Tools;
using UI.Tools.Managers;
using UI.Tools.Navigation;

namespace UI.ViewModels
{
    public class DashboardViewModel:BaseViewModel
    {
        private RelayCommand _loginCommand;
        private RelayCommand _putMoneyCommand;
        private RelayCommand _withdrawCommand;
        private RelayCommand _infoCommand;
        private RelayCommand _transactionCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Login)));
            }
        }

        public RelayCommand PutMoneyCommand
        {
            get { return _putMoneyCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Put)); }
        }

        public RelayCommand WithdrawCommand
        {
            get { return _withdrawCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Withdraw)); }
        }

        public RelayCommand InfoCommand
        {
            get { return _infoCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.MainInfo)); }
        }

        public RelayCommand TransactionCommand
        {
            get
            {

                //return _transactionCommand = new RelayCommand(() => NavigationManager.Instance.Navigate(ViewType.Withdraw));
                return null;
            }
        }
    }
}