using Windows.UI.Xaml.Controls;
using UI.Tools.Navigation;
using UI.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardView : Page, INavigatable
    {
        public DashboardView()
        {
            this.InitializeComponent();
            DataContext = new DashboardViewModel();
        }
    }
}
