using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class CadastroColetorPage : ContentPage
{
    private readonly IAuthService _authService;

    public CadastroColetorPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnCadastrarClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var username = UsernameEntry.Text?.Trim();
        var cpf = CpfEntry.Text?.Trim();
        var cnpj = CnpjEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var senha = SenhaEntry.Text;
        var confirmarSenha = ConfirmarSenhaEntry.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(cpf) ||
            string.IsNullOrWhiteSpace(cnpj) ||
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

        if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
        {
            ShowError("CNPJ invalido. Digite os 14 numeros, sem pontos, barra ou traco.");
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

        var novoColetor = new MockUser
        {
            Nome = username,
            Username = username,
            Cpf = cpf,
            Cnpj = cnpj,
            Email = email,
            Senha = senha
        };

        var erro = await _authService.RegisterColetorAsync(novoColetor);

        if (erro is not null)
        {
            ShowError(erro);
            return;
        }

        await DisplayAlert("Recycle", "Cadastro realizado! Faca login pra continuar.", "OK");
        await Shell.Current.GoToAsync(nameof(LoginColetorPage));
    }

    private async void OnVoltarLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginColetorPage));
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }
}