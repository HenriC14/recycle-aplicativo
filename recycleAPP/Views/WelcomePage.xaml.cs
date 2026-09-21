using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var usuario = CurrentSession.UsuarioLogado;

        NomeLabel.Text = usuario is not null
            ? $"Ola, {usuario.Nome}!"
            : "Nenhum usuario logado.";

        TipoLabel.Text = usuario is not null
            ? $"Tipo: {usuario.Tipo}"
            : string.Empty;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        CurrentSession.UsuarioLogado = null;
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }
}