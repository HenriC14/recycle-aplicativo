using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class CadastroPage : ContentPage
{
    private readonly IAuthService _authService;

    public CadastroPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnCadastrarClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var username = UsernameEntry.Text?.Trim();
        var cpf = CpfEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var senha = SenhaEntry.Text;
        var confirmarSenha = ConfirmarSenhaEntry.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(cpf) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(senha) ||
            string.IsNullOrWhiteSpace(confirmarSenha))
        {
            ShowError("Preencha todos os campos.");
            return;
        }

        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
        {
            ShowError("CPF invalido. Digite os 11 numeros, sem pontos ou traco.");
            return;
        }

        if (!email.Contains('@') || !email.Contains('.'))
        {
            ShowError("E-mail invalido.");
            return;
        }

        if (senha != confirmarSenha)
        {
            ShowError("As senhas nao coincidem.");
            return;
        }

        var novoUsuario = new MockUser
        {
            Nome = username,
            Username = username,
            Cpf = cpf,
            Email = email,
            Senha = senha
        };

        var erro = await _authService.RegisterAsync(novoUsuario);

        if (erro is not null)
        {
            ShowError(erro);
            return;
        }

        await DisplayAlert("Recycle", "Cadastro realizado! Faca login pra continuar.", "OK");
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private async void OnVoltarLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }
}