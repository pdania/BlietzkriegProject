namespace BlietzkriegProject.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Main,
        Google,
        Put
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
