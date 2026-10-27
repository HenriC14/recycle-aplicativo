using recycleAPP.Services;
using System.Globalization;

namespace recycleAPP.Views;

public partial class PerfilColetorPage : ContentPage, IQueryAttributable
{
    // Usuário cujo perfil está sendo exibido
    public MockUser? UsuarioExibido { get; set; }

    public PerfilColetorPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Usuario", out var valor) &&
            valor is MockUser usuario)
        {
            UsuarioExibido = usuario;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Se nenhum usuário foi passado pela navegação,
        // usa o usuário logado como alternativa.
        UsuarioExibido ??= CurrentSession.UsuarioLogado;

        var usuario = UsuarioExibido;

        bool logadoComoReciclador =
            CurrentSession.UsuarioLogado?.Tipo == TipoUsuario.Reciclador;

        BarraInferior.IsVisible = logadoComoReciclador;
        NovaReciclagemBorder.IsVisible = logadoComoReciclador;

        if (usuario is null)
        {
            NomeLabel.Text = "Nenhum usuario encontrado";
            SeguidoresLabel.Text = "0";
            EnderecoLabel.Text = "";
            HorarioLabel.Text = "";
            StatusAbertoLabel.Text = "";
            MateriaisFlexLayout.Children.Clear();

            return;
        }

        bool ehPerfilProprio =
            CurrentSession.UsuarioLogado is not null &&
            CurrentSession.UsuarioLogado.Username.Equals(
                usuario.Username,
                StringComparison.OrdinalIgnoreCase);

        AcaoPerfilButton.Text =
            ehPerfilProprio ? "Editar perfil" : "Seguir";

        NomeLabel.Text = usuario.Nome;

        SeguidoresLabel.Text =
            usuario.Seguidores.ToString();

        EnderecoLabel.Text =
            usuario.Endereco;

        HorarioLabel.Text =
            usuario.HorarioFuncionamento;

        StatusAbertoLabel.Text =
            usuario.PontoAberto
                ? "Aberto"
                : "Fechado";

        StatusAbertoLabel.TextColor =
            usuario.PontoAberto
                ? Color.FromArgb("#1F8A3B")
                : Color.FromArgb("#D3302F");

        MontarMateriaisAceitos(
            usuario.MateriaisAceitos);

        // =========================================================
        // SOLICITAÇÃO DO RECICLADOR PARA O COLETOR
        // =========================================================
        //
        // Só verifica a solicitação quando:
        // 1. O perfil exibido pertence a um coletor;
        // 2. O usuário atualmente logado também é um coletor.
        //
        // Isso garante que um coletor não veja solicitações
        // destinadas a outro coletor.
        //
        if (usuario.Tipo == TipoUsuario.Coletor &&
            CurrentSession.UsuarioLogado?.Tipo == TipoUsuario.Coletor)
        {
            await MostrarSolicitacaoPendente(usuario);
        }
    }

    private async Task MostrarSolicitacaoPendente(MockUser coletor)
    {
        var solicitacao =
            SolicitacaoMockService.ObterSolicitacaoParaColetor(coletor);

        // Não existe solicitação para este coletor.
        if (solicitacao is null)
            return;

        bool ignorar = await DisplayAlert(
            "Nova solicitação",
            $"{solicitacao.RecicladorNome} enviou uma solicitação para você.",
            "Ignorar",
            "Fechar");

        // Por enquanto, tanto "Ignorar" quanto "Fechar"
        // encerram a solicitação temporária.
        SolicitacaoMockService.IgnorarSolicitacao(solicitacao);
    }

    private void MontarMateriaisAceitos(List<string> materiais)
    {
        MateriaisFlexLayout.Children.Clear();

        foreach (var material in materiais)
        {
            var icone =
                MaterialIconHelper.ObterIcone(material);

            if (string.IsNullOrEmpty(icone))
                continue;

            var item = new VerticalStackLayout
            {
                Spacing = 4,
                Margin = new Thickness(0, 0, 14, 10),
                HorizontalOptions = LayoutOptions.Center,

                Children =
                {
                    new Image
                    {
                        Source = icone,
                        WidthRequest = 48,
                        HeightRequest = 48,
                        Aspect = Aspect.AspectFill,

                        Clip =
                            new Microsoft.Maui.Controls.Shapes
                                .RoundRectangleGeometry
                            {
                                CornerRadius = 10,
                                Rect = new Rect(0, 0, 48, 48)
                            }
                    },

                    new Label
                    {
                        Text = material,
                        TextColor = Colors.Black,
                        FontSize = 12,
                        HorizontalOptions =
                            LayoutOptions.Center
                    }
                }
            };

            MateriaisFlexLayout.Children.Add(item);
        }
    }

    private async void OnNovaReciclagemTapped(
        object sender,
        EventArgs e)
    {
        var usuarioLogado = CurrentSession.UsuarioLogado;

        // O botão "+" só funciona para um reciclador.
        if (usuarioLogado is null ||
            usuarioLogado.Tipo != TipoUsuario.Reciclador)
        {
            return;
        }

        var coletor = UsuarioExibido;

        // Garante que o perfil aberto pertence realmente
        // a um coletor.
        if (coletor is null ||
            coletor.Tipo != TipoUsuario.Coletor)
        {
            return;
        }

        // Cria a solicitação temporária.
        SolicitacaoMockService.CriarSolicitacao(
            usuarioLogado,
            coletor);

        await DisplayAlert(
            "Solicitação enviada",
            $"Uma solicitação foi enviada para {coletor.Nome}.",
            "OK");
    }

    private async void OnAcaoPerfilClicked(
        object sender,
        EventArgs e)
    {
        if (AcaoPerfilButton.Text == "Editar perfil")
        {
            await DisplayAlert(
                "Editar perfil",
                "Tela de edicao de perfil ainda nao implementada.",
                "OK");
        }
        else
        {
            await DisplayAlert(
                "Seguir",
                "Funcionalidade de seguir ainda nao implementada.",
                "OK");
        }
    }

    private async void OnLogoutClicked(
        object sender,
        EventArgs e)
    {
        // Remove o usuário atual da sessão.
        CurrentSession.UsuarioLogado = null;

        // Volta para a tela de login.
        await Shell.Current.GoToAsync("//loginPage");
    }
}