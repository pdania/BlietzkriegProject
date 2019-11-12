namespace BlietzkriegProject.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Main,
        Google
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
