
using recycleAPP.Services;
using System.Globalization;

namespace recycleAPP.Views;

public partial class PerfilRecicladorPage : ContentPage
{
    // Usuario cujo perfil esta sendo mostrado.
    public MockUser? UsuarioExibido { get; set; }

    public PerfilRecicladorPage()
    {
        InitializeComponent();
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
            NomeLabel.Text = "Nenhum usuario logado";

            SaldoPontosButton.IsVisible = false;

            return;
        }

        // Verifica se o perfil exibido pertence
        // ao mesmo usuario que esta logado.
        bool ehPerfilProprio =
            CurrentSession.UsuarioLogado is not null &&
            CurrentSession.UsuarioLogado.Username.Equals(
                usuario.Username,
                StringComparison.OrdinalIgnoreCase
            );

        // Define o texto do botao de acao.
        AcaoPerfilButton.Text =
            ehPerfilProprio
                ? "Editar perfil"
                : "Seguir";

        // ======================================
        // SALDO DE PONTOS
        // ======================================

        // O botao aparece somente no proprio
        // perfil de um usuario do tipo Reciclador.
        SaldoPontosButton.IsVisible =
            ehPerfilProprio &&
            usuario.Tipo == TipoUsuario.Reciclador;

        if (SaldoPontosButton.IsVisible)
        {
            SaldoPontosButton.Text =
                $"Saldo de pontos: {usuario.SaldoPontos}";
        }

        // ======================================
        // INFORMACOES DO PERFIL
        // ======================================

        NomeLabel.Text = usuario.Nome;

        SeguidoresLabel.Text =
            usuario.Seguidores.ToString();

        SeguindoLabel.Text =
            usuario.Seguindo.ToString();

        EnderecoLabel.Text =
            usuario.Endereco;

        ImagemPerfil.Source =
            usuario.ImagemDePerfil;

        // ======================================
        // ULTIMA ATIVIDADE
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
        // COMPOSICAO DOS MATERIAIS
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
    // FORMATAR COMPOSICAO DOS MATERIAIS
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
    // BOTAO DE SALDO DE PONTOS
    // ==========================================

    private async void OnSaldoPontosClicked(
        object sender,
        EventArgs e)
    {
        // FUTURA IMPLEMENTACAO:
        // Aqui sera aberta a tela de historico
        // de movimentacoes do saldo de pontos.

        await DisplayAlert(
            "Saldo de pontos",
            "O historico do saldo de pontos sera implementado futuramente.",
            "OK"
        );
    }

    // ==========================================
    // BOTAO DE ACAO DO PERFIL
    // ==========================================

    private async void OnAcaoPerfilClicked(
        object sender,
        EventArgs e)
    {
        if (AcaoPerfilButton.Text == "Editar perfil")
        {
            await DisplayAlert(
                "Editar perfil",
                "Tela de edicao de perfil ainda nao implementada.",
                "OK"
            );
        }
        else
        {
            await DisplayAlert(
                "Seguir",
                "Funcionalidade de seguir ainda nao implementada.",
                "OK"
            );
        }
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
