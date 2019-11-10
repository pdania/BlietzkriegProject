using System.ComponentModel;
using System.Windows;
using Windows.UI.Xaml;

namespace BlietzkriegProject.Tools
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool IsControlEnabled { get; set; }
    }
}
