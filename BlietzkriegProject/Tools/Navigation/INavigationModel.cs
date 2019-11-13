﻿namespace BlietzkriegProject.Tools.Navigation
{
    internal enum ViewType
    {
        Login,
        Main,
        Google,
        Put,
        Withdraw
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
