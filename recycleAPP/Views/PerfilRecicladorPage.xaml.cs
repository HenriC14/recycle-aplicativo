using recycleAPP.Services;
using System.Globalization;

namespace recycleAPP.Views;

public partial class PerfilRecicladorPage : ContentPage
{
    // Usuario cujo perfil esta sendo mostrado. Por enquanto sempre e o proprio
    // usuario logado, ate existir uma tela de "ver perfil de outra pessoa".
    public MockUser? UsuarioExibido { get; set; }

    public PerfilRecicladorPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UsuarioExibido ??= CurrentSession.UsuarioLogado;
        var usuario = UsuarioExibido;

        if (usuario is null)
        {
            NomeLabel.Text = "Nenhum usuario logado";
            return;
        }

        bool ehPerfilProprio = CurrentSession.UsuarioLogado is not null &&
                               CurrentSession.UsuarioLogado.Username.Equals(usuario.Username, StringComparison.OrdinalIgnoreCase);

        AcaoPerfilButton.Text = ehPerfilProprio ? "Editar perfil" : "Seguir";

        NomeLabel.Text = usuario.Nome;
        SeguidoresLabel.Text = usuario.Seguidores.ToString();
        SeguindoLabel.Text = usuario.Seguindo.ToString();
        EnderecoLabel.Text = usuario.Endereco;

        UltimaAtividadeQuandoLabel.Text = usuario.UltimaAtividadeQuando;
        UltimaAtividadePesoLabel.Text = FormatarKg(usuario.UltimaAtividadePesoKg);
        UltimaAtividadeMaterialLabel.Text = usuario.UltimaAtividadeMaterial;
        UltimaAtividadeMaterialIcon.Source = MaterialIconHelper.ObterIcone(usuario.UltimaAtividadeMaterial);
        UltimaAtividadeKmLabel.Text = $"{usuario.UltimaAtividadeKm.ToString("0.0", CultureInfo.InvariantCulture)}KM";

        QuilosRecicladosLabel.Text = FormatarKg(usuario.QuilosReciclados);
        MaterialMaisRecicladoIcon.Source = MaterialIconHelper.ObterIcone(usuario.MaterialMaisReciclado);
        MaterialMaisRecicladoLabel.Text = $"{usuario.MaterialMaisReciclado} {usuario.MaterialMaisRecicladoPercentual:0}%";

        PctPlasticoLabel.Text = FormatarComposicao(usuario, "Plastico");
        PctVidroLabel.Text = FormatarComposicao(usuario, "Vidro");
        PctMetalLabel.Text = FormatarComposicao(usuario, "Metal");
        PctPapelLabel.Text = FormatarComposicao(usuario, "Papel");
    }

    private static string FormatarKg(double kg) =>
        kg >= 1
            ? $"{kg.ToString("0.#", CultureInfo.InvariantCulture)}Kg"
            : $"{(kg * 1000):0}g";

    private static string FormatarComposicao(MockUser usuario, string nomeMaterial)
    {
        var item = usuario.Composicao.FirstOrDefault(m =>
            m.Nome.Equals(nomeMaterial, StringComparison.OrdinalIgnoreCase));

        if (item is null)
            return $"{nomeMaterial}: 0% (0Kg)";

        var pesoFormatado = item.PesoKg.ToString("0.#", CultureInfo.InvariantCulture);
        return $"{item.Percentual:0}% ({pesoFormatado}Kg)";
    }

    private async void OnAcaoPerfilClicked(object sender, EventArgs e)
    {
        if (AcaoPerfilButton.Text == "Editar perfil")
        {
            await DisplayAlert("Editar perfil", "Tela de edicao de perfil ainda nao implementada.", "OK");
        }
        else
        {
            await DisplayAlert("Seguir", "Funcionalidade de seguir ainda nao implementada.", "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        CurrentSession.UsuarioLogado = null;
        await Shell.Current.GoToAsync(nameof(SelecionarTipoPage));
    }
}