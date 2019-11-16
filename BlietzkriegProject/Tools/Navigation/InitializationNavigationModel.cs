using System;
using UI.Views;

namespace UI.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {
            
        }
   
        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    ViewsDictionary.Add(viewType,new LoginView());
                    break;
                case ViewType.Google:
                    ViewsDictionary.Add(viewType, new GoogleAuthenticatorView());
                    break;
                case ViewType.Put:
                    ViewsDictionary.Add(viewType, new PutMoneyView());
                    break;
                case ViewType.Withdraw:
                    ViewsDictionary.Add(viewType, new WithdrawMoneyView());
                    break;
                case ViewType.MainInfo:
                    ViewsDictionary.Add(viewType, new MainInfoView());
                    break;
                case ViewType.Dashboard:
                    ViewsDictionary.Add(viewType, new DashboardView());
                    break;
                case ViewType.Transactions:
                    ViewsDictionary.Add(viewType, new TransactionsView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
