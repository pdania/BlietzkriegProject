using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using UI.Tools.Navigation;
using UI.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace UI.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class LoginView : Page, INavigatable
    { 

        public LoginView()
        {
            this.InitializeComponent();
            DataContext = new LoginWindowViewModel();
        }
    }
}
