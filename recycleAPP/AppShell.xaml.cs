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
            Routing.RegisterRoute(nameof(SelecionarTipoPage), typeof(SelecionarTipoPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(CadastroColetorPage), typeof(CadastroColetorPage));
            Routing.RegisterRoute(nameof(LoginColetorPage), typeof(LoginColetorPage));
        }
    }
}
