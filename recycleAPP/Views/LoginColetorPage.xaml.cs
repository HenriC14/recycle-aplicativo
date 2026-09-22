using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class LoginColetorPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginColetorPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnEntrarClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var cpf = CpfEntry.Text?.Trim();
        var cnpj = CnpjEntry.Text?.Trim();
        var senha = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(cnpj) || string.IsNullOrWhiteSpace(senha))
        {
            ShowError("Preencha CPF, CNPJ e senha.");
            return;
        }

        var loggedUser = await _authService.TryLoginColetorAsync(cpf, cnpj, senha);

        if (loggedUser is null)
        {
            ShowError("CPF, CNPJ ou senha invalidos.");
            return;
        }

        CurrentSession.UsuarioLogado = loggedUser;
        await Shell.Current.GoToAsync(nameof(WelcomePage));
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Recuperar senha", "Funcionalidade ainda nao implementada.", "OK");
    }

    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CadastroColetorPage));
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }
}