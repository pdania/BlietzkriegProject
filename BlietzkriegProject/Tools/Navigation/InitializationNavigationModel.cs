using System;
//using MainView = KMA.ProgrammingInCSharp2019.Practice5.Navigation.Views.MainView;
using LoginView = BlietzkriegProject.Views.LoginView;
using GoogleAuthenticator = BlietzkriegProject.Views.GoogleAuthenticatorView;
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
//                case ViewType.Main:
//                    ViewsDictionary.Add(viewType, new MainView());
//                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
