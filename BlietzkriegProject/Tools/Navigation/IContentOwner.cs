using Windows.UI.Xaml.Controls;

namespace UI.Tools.Navigation
{
    internal interface IContentOwner
    {
        ContentControl ContentControl { get; }
    }
}
