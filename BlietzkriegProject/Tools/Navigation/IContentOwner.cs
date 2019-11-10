using Windows.UI.Xaml.Controls;

namespace BlietzkriegProject.Tools.Navigation
{
    internal interface IContentOwner
    {
        ContentControl ContentControl { get; }
    }
}
