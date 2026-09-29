namespace recycleAPP.Views;

public partial class SelecionarTipoPage : ContentPage
{
    public SelecionarTipoPage()
    {
        InitializeComponent();
    }

    private async void OnPessoaTapped(object sender, EventArgs e)
    {
        // Pessoa = Reciclador
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private async void OnEmpresaTapped(object sender, EventArgs e)
    {
        // Empresa = Coletor
        await Shell.Current.GoToAsync(nameof(LoginColetorPage));
    }

    private async void OnLojaPontosClicked(object sender, EventArgs e)
    {
        // TEMPORARIO: so pra testar a tela sem depender de login
        await Shell.Current.GoToAsync(nameof(LojaPontosPage));
    }
}