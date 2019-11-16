namespace BlietzkriegProject.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Google,
        Put,
        Withdraw,
        MainInfo,
        Dashboard,
        Transactions
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
