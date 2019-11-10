namespace BlietzkriegProject.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Main
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
