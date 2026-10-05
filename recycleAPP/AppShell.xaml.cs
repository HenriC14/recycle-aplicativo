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
            Routing.RegisterRoute(nameof(PerfilColetorPage), typeof(PerfilColetorPage));
            Routing.RegisterRoute(nameof(PerfilRecicladorPage), typeof(PerfilRecicladorPage));
            Routing.RegisterRoute(nameof(LojaPontosPage), typeof(LojaPontosPage));
            Routing.RegisterRoute(nameof(DetalhesProdutoPage), typeof(DetalhesProdutoPage));
            Routing.RegisterRoute(nameof(InicioPage), typeof(InicioPage));
            Routing.RegisterRoute(nameof(NovaReciclagemPage), typeof(NovaReciclagemPage));
        }
    }
}
