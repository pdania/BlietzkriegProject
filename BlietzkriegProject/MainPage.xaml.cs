using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using UI.Tools.Managers;
using UI.Tools.Navigation;
using UI.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace UI
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page, IContentOwner
    {
        public MainPage()
        {
           
            InitializeComponent();
            DataContext = new MainWindowViewModel();
//            ApplicationView.PreferredLaunchViewSize = new Size(800, 400);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
//            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500,320));
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.ScheduledTransaction);
        }

        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }
    }
}
