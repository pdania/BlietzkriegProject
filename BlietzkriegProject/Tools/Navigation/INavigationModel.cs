namespace UI.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Google,
        Put,
        Withdraw,
        MainInfo,
        Dashboard,
        Transactions,
        ScheduledTransaction,
        EditScheduledTran
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
