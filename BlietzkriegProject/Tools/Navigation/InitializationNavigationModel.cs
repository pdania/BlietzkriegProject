using System;
//using MainView = KMA.ProgrammingInCSharp2019.Practice5.Navigation.Views.MainView;
using LoginView = BlietzkriegProject.Views.LoginView;
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
//                case ViewType.SignUp:
//                    ViewsDictionary.Add(viewType, new SignUpView());
//                    break;
//                case ViewType.Main:
//                    ViewsDictionary.Add(viewType, new MainView());
//                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
