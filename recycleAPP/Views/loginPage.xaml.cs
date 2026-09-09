using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }
 
    private async void OnEntrarClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var username = UsernameEntry.Text?.Trim();
        var password = PasswordEntry.Text;
 
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Preencha usuario e senha.");
            return;
        }
 
        var loggedUser = await _authService.TryLoginAsync(username, password);
 
        if (loggedUser is null)
        {
            ShowError("Usuario ou senha invalidos.");
            return;
        }
 
        // Quando as proximas telas existirem, troque por:
        // await Shell.Current.GoToAsync(nameof(PerfilColetorPage));
        await DisplayAlert("Recycle", $"Bem-vindo, {loggedUser.Nome}!", "OK");
    }
 
    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        // TODO: implementar fluxo real de recuperacao de senha (caso de uso "Recuperar senha")
        await DisplayAlert("Recuperar senha", "Funcionalidade ainda nao implementada.", "OK");
    }
 
    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        // TODO: trocar pelo nome real da rota/pagina de cadastro quando ela existir
        // await Shell.Current.GoToAsync(nameof(CadastroPage));
        await DisplayAlert("Cadastro", "Tela de cadastro ainda sera conectada aqui.", "OK");
    }
 
    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }
}