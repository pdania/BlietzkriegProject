using System;
using BlietzkriegProject.Views;
//using MainView = KMA.ProgrammingInCSharp2019.Practice5.Navigation.Views.MainView;
using LoginView = BlietzkriegProject.Views.LoginView;
using GoogleAuthenticator = BlietzkriegProject.Views.GoogleAuthenticatorView;
using PutMoney = BlietzkriegProject.Views.PutMoney;
namespace BlietzkriegProject.Tools.Navigation
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
                    ViewsDictionary.Add(viewType, new GoogleAuthenticator());
                    break;
                case ViewType.Put:
                    ViewsDictionary.Add(viewType, new PutMoney());
                    break;
                case ViewType.Withdraw:
                    ViewsDictionary.Add(viewType, new WithdrawMoney());
                    break;
                //                case ViewType.Main:
                //                    ViewsDictionary.Add(viewType, new MainView());
                //                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
