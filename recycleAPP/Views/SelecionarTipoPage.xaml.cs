namespace recycleAPP.Views;

public partial class SelecionarTipoPage : ContentPage
{
    public SelecionarTipoPage()
    {
        InitializeComponent();
    }

    private async void OnRecicladorTapped(object sender, EventArgs e)
    {
        // Pessoa = Reciclador -> login/cadastro ja existentes
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private async void OnEmpresaTapped(object sender, EventArgs e)
    {
        // TODO: Empresa = Coletor. Por enquanto usa a mesma LoginPage generica,
        // ate as telas de login/cadastro proprias do Coletor serem criadas.
        // Quando existirem, trocar para: await Shell.Current.GoToAsync(nameof(LoginColetorPage));
        await Shell.Current.GoToAsync(nameof(LoginColetorPage));
    }
}