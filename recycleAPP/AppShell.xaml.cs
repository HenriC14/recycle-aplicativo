using recycleAPP.Views;

namespace recycleAPP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CadastroPage), typeof(CadastroPage));
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }
    }
}
