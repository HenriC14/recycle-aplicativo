
using recycleAPP.Services;
using System.Globalization;

namespace recycleAPP.Views;

public partial class PerfilRecicladorPage : ContentPage, IQueryAttributable
{
    // Usuário cujo perfil está sendo exibido.
    public MockUser? UsuarioExibido { get; set; }

    public PerfilRecicladorPage()
    {
        InitializeComponent();
    }

    // ==========================================
    // RECEBER USUÁRIO PELA NAVEGAÇÃO
    // ==========================================

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("Usuario", out var valor) &&
            valor is MockUser usuario)
        {
            UsuarioExibido = usuario;
        }
    }

    // ==========================================
    // CARREGAMENTO DO PERFIL
    // ==========================================

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UsuarioExibido ??= CurrentSession.UsuarioLogado;

        var usuario = UsuarioExibido;

        if (usuario is null)
        {
            NomeLabel.Text = "Nenhum usuario encontrado";
            SaldoPontosButton.IsVisible = false;
            AcaoPerfilButton.IsVisible = false;

            return;
        }

        bool ehPerfilProprio =
            CurrentSession.UsuarioLogado is not null &&
            CurrentSession.UsuarioLogado.Username.Equals(
                usuario.Username,
                StringComparison.OrdinalIgnoreCase
            );

        AtualizarEstadoRelacionamento(usuario, ehPerfilProprio);

        // ======================================
        // SALDO DE PONTOS
        // ======================================

        SaldoPontosButton.IsVisible =
            ehPerfilProprio &&
            usuario.Tipo == TipoUsuario.Reciclador;

        if (SaldoPontosButton.IsVisible)
        {
            SaldoPontosButton.Text =
                $"Saldo de pontos: {usuario.SaldoPontos}";
        }

        // ======================================
        // INFORMAÇÕES DO PERFIL
        // ======================================

        NomeLabel.Text = usuario.Nome;

        SeguidoresLabel.Text =
            MockUserService.ObterQuantidadeSeguidores(
                usuario.Username).ToString();

        SeguindoLabel.Text =
            MockUserService.ObterQuantidadeSeguindo(
                usuario.Username).ToString();

        EnderecoLabel.Text = usuario.Endereco;

        ImagemPerfil.Source = usuario.ImagemDePerfil;

        // ======================================
        // ÚLTIMA ATIVIDADE
        // ======================================

        UltimaAtividadeQuandoLabel.Text =
            usuario.UltimaAtividadeQuando;

        UltimaAtividadePesoLabel.Text =
            FormatarKg(usuario.UltimaAtividadePesoKg);

        UltimaAtividadeMaterialLabel.Text =
            usuario.UltimaAtividadeMaterial;

        UltimaAtividadeMaterialIcon.Source =
            MaterialIconHelper.ObterIcone(
                usuario.UltimaAtividadeMaterial
            );

        UltimaAtividadeKmLabel.Text =
            $"{usuario.UltimaAtividadeKm.ToString("0.0", CultureInfo.InvariantCulture)}KM";

        // ======================================
        // QUILOS RECICLADOS
        // ======================================

        QuilosRecicladosLabel.Text =
            FormatarKg(usuario.QuilosReciclados);

        // ======================================
        // MATERIAL MAIS RECICLADO
        // ======================================

        MaterialMaisRecicladoIcon.Source =
            MaterialIconHelper.ObterIcone(
                usuario.MaterialMaisReciclado
            );

        MaterialMaisRecicladoLabel.Text =
            $"{usuario.MaterialMaisReciclado} {usuario.MaterialMaisRecicladoPercentual:0}%";

        // ======================================
        // COMPOSIÇÃO DOS MATERIAIS
        // ======================================

        PctPlasticoLabel.Text =
            FormatarComposicao(usuario, "Plastico");

        PctVidroLabel.Text =
            FormatarComposicao(usuario, "Vidro");

        PctMetalLabel.Text =
            FormatarComposicao(usuario, "Metal");

        PctPapelLabel.Text =
            FormatarComposicao(usuario, "Papel");
    }

    // ==========================================
    // FORMATAR PESO
    // ==========================================

    private static string FormatarKg(double kg) =>
        kg >= 1
            ? $"{kg.ToString("0.#", CultureInfo.InvariantCulture)}Kg"
            : $"{(kg * 1000):0}g";

    // ==========================================
    // FORMATAR COMPOSIÇÃO DOS MATERIAIS
    // ==========================================

    private static string FormatarComposicao(
        MockUser usuario,
        string nomeMaterial)
    {
        var item = usuario.Composicao.FirstOrDefault(m =>
            m.Nome.Equals(
                nomeMaterial,
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (item is null)
            return $"{nomeMaterial}: 0% (0Kg)";

        var pesoFormatado =
            item.PesoKg.ToString(
                "0.#",
                CultureInfo.InvariantCulture
            );

        return $"{item.Percentual:0}% ({pesoFormatado}Kg)";
    }

    // ==========================================
    // BOTÃO DE SALDO DE PONTOS
    // ==========================================

    private async void OnSaldoPontosClicked(
        object sender,
        EventArgs e)
    {
        await DisplayAlert(
            "Saldo de pontos",
            "O historico do saldo de pontos sera implementado futuramente.",
            "OK"
        );
    }

    // ==========================================
    // BOTÃO SEGUIR / NÃO SEGUIR
    // ==========================================

    private async void OnAcaoPerfilClicked(
        object sender,
        EventArgs e)
    {
        var usuario = UsuarioExibido;
        var usuarioLogado = CurrentSession.UsuarioLogado;

        if (usuario is null)
            return;

        bool ehPerfilProprio =
            usuarioLogado is not null &&
            usuarioLogado.Username.Equals(
                usuario.Username,
                StringComparison.OrdinalIgnoreCase);

        // Se for o próprio perfil, mantém a edição.
        if (ehPerfilProprio)
        {
            await DisplayAlert(
                "Editar perfil",
                "Tela de edicao de perfil ainda nao implementada.",
                "OK"
            );

            return;
        }

        if (usuarioLogado is null)
        {
            await DisplayAlert(
                "Entrar na conta",
                "Voce precisa estar logado para seguir outra conta.",
                "OK"
            );

            return;
        }

        bool jaEstaSeguindo = MockUserService.EstaSeguindo(
            usuarioLogado.Username,
            usuario.Username);

        if (jaEstaSeguindo)
        {
            await MockUserService.DeixarDeSeguirUsuarioAsync(
                usuarioLogado.Username,
                usuario.Username);
        }
        else
        {
            await MockUserService.SeguirUsuarioAsync(
                usuarioLogado.Username,
                usuario.Username);
        }

        // Atualiza botão e contadores imediatamente.
        AtualizarEstadoRelacionamento(
            usuario,
            ehPerfilProprio: false
        );

        SeguidoresLabel.Text =
            MockUserService.ObterQuantidadeSeguidores(
                usuario.Username).ToString();

        SeguindoLabel.Text =
            MockUserService.ObterQuantidadeSeguindo(
                usuario.Username).ToString();
    }

    // ==========================================
    // ATUALIZAR ESTADO DO RELACIONAMENTO
    // ==========================================

    private void AtualizarEstadoRelacionamento(
        MockUser usuario,
        bool ehPerfilProprio)
    {
        if (ehPerfilProprio)
        {
            AcaoPerfilButton.Text = "Editar perfil";

            AcaoPerfilButton.BackgroundColor =
                Color.FromArgb("#3FBF63");

            AcaoPerfilButton.IsEnabled = true;

            return;
        }

        var usuarioLogado = CurrentSession.UsuarioLogado;

        bool estaSeguindo =
            usuarioLogado is not null &&
            MockUserService.EstaSeguindo(
                usuarioLogado.Username,
                usuario.Username);

        if (estaSeguindo)
        {
            AcaoPerfilButton.Text = "Não seguir";

            AcaoPerfilButton.BackgroundColor =
                Color.FromArgb("#AAAAAA");
        }
        else
        {
            AcaoPerfilButton.Text = "Seguir";

            AcaoPerfilButton.BackgroundColor =
                Color.FromArgb("#3FBF63");
        }

        AcaoPerfilButton.IsEnabled =
            usuarioLogado is not null;
    }

    // ==========================================
    // SAIR DA CONTA
    // ==========================================

    private async void OnLogoutClicked(
        object sender,
        EventArgs e)
    {
        CurrentSession.UsuarioLogado = null;

        await Shell.Current.GoToAsync(
            nameof(SelecionarTipoPage)
        );
    }
}
