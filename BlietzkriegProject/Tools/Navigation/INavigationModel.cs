namespace BlietzkriegProject.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Google,
        Put,
        Withdraw,
        MainInfo,
        Dashboard
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
