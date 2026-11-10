
using recycleAPP.Models;
using recycleAPP.Services;
using static recycleAPP.Services.MockUserService;

namespace recycleAPP.Views;

public partial class SolicitacaoReciclagemPage
    : ContentPage, IQueryAttributable
{
    private MockUser? _coletor;
    private MockUser? _reciclador;

    private readonly HashSet<string> _materiaisSelecionados =
        new(StringComparer.OrdinalIgnoreCase);

    public SolicitacaoReciclagemPage()
    {
        InitializeComponent();
    }

    // Recebe o coletor selecionado no perfil
    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("Coletor", out var coletor) &&
            coletor is MockUser usuarioColetor)
        {
            _coletor = usuarioColetor;
        }
    }

    // Carrega os dados dos usuários
    protected override void OnAppearing()
    {
        base.OnAppearing();

        _reciclador = CurrentSession.UsuarioLogado;

        if (_coletor is not null)
        {
            ColetorNomeLabel.Text = _coletor.Nome;

            ColetorImagem.Source =
                string.IsNullOrWhiteSpace(_coletor.ImagemDePerfil)
                ? null
                : _coletor.ImagemDePerfil;
        }

        if (_reciclador is not null)
        {
            RecicladorNomeLabel.Text = _reciclador.Nome;

            RecicladorImagem.Source =
                string.IsNullOrWhiteSpace(_reciclador.ImagemDePerfil)
                ? null
                : _reciclador.ImagemDePerfil;
        }
    }

    // ==========================================
    // SELEÇÃO DOS MATERIAIS
    // ==========================================

    private void OnMaterialClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is null)
            return;

        string material =
            button.CommandParameter.ToString()!;

        if (!_materiaisSelecionados.Add(material))
        {
            _materiaisSelecionados.Remove(material);
        }

        bool selecionado =
            _materiaisSelecionados.Contains(material);

        button.BackgroundColor =
            Color.FromArgb(
                selecionado ? "#278B50" : "#777777"
            );

        button.BorderColor =
            selecionado ? Colors.White : Colors.Transparent;

        button.BorderWidth =
            selecionado ? 2 : 0;
    }

    // ==========================================
    // SELEÇÃO DO VOLUME
    // ==========================================

    private void OnVolumeLabelTapped(
        object sender,
        TappedEventArgs e)
    {
        var opcao = (
            e.Parameter ??
            (sender as Label)?
                .GestureRecognizers
                .OfType<TapGestureRecognizer>()
                .FirstOrDefault()?
                .CommandParameter
        )?.ToString();

        switch (opcao)
        {
            case "UmaMao":
                VolumeUmaMao.IsChecked =
                    !VolumeUmaMao.IsChecked;
                break;

            case "DuasMaos":
                VolumeDuasMaos.IsChecked =
                    !VolumeDuasMaos.IsChecked;
                break;

            case "Ajuda":
                VolumeAjuda.IsChecked =
                    !VolumeAjuda.IsChecked;
                break;

            case "Veiculo":
                VolumeVeiculo.IsChecked =
                    !VolumeVeiculo.IsChecked;
                break;
        }
    }

    // ==========================================
    // SALVAR SOLICITAÇÃO
    // ==========================================

    private async void OnSalvarClicked(
        object sender,
        EventArgs e)
    {
        // Verifica o coletor
        if (_coletor is null)
        {
            await DisplayAlert(
                "Solicitação",
                "Não foi possível identificar o coletor.",
                "OK"
            );

            return;
        }

        // Verifica o reciclador
        if (_reciclador is null ||
            _reciclador.Tipo != TipoUsuario.Reciclador)
        {
            await DisplayAlert(
                "Solicitação",
                "Entre como reciclador para solicitar uma coleta.",
                "OK"
            );

            return;
        }

        // Verifica os materiais
        if (_materiaisSelecionados.Count == 0)
        {
            await DisplayAlert(
                "Informações incompletas",
                "Selecione pelo menos um material.",
                "OK"
            );

            return;
        }

        // Recupera o peso selecionado
        var peso = PesoOptions.Children
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked)?
            .Value?.ToString();

        // Recupera o estado selecionado
        var estado = EstadoOptions.Children
            .OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked)?
            .Value?.ToString();

        // Recupera os volumes selecionados
        var volume = new List<string>();

        if (VolumeUmaMao.IsChecked)
            volume.Add("Consigo levar em uma mão sozinho");

        if (VolumeDuasMaos.IsChecked)
            volume.Add("Consigo levar em duas mãos");

        if (VolumeAjuda.IsChecked)
            volume.Add("Consigo levar com ajuda");

        if (VolumeVeiculo.IsChecked)
            volume.Add("Preciso de um veículo");

        // Validação das respostas
        if (string.IsNullOrWhiteSpace(peso) ||
            string.IsNullOrWhiteSpace(estado) ||
            volume.Count == 0)
        {
            await DisplayAlert(
                "Informações incompletas",
                "Preencha o peso, o volume e o estado do material.",
                "OK"
            );

            return;
        }

        // Cria a solicitação
        var solicitacao = new SolicitacaoReciclagem
        {
            ColetorUsername = _coletor.Username,
            ColetorNome = _coletor.Nome,
            ColetorImagem = _coletor.ImagemDePerfil,

            RecicladorUsername = _reciclador.Username,
            RecicladorNome = _reciclador.Nome,
            RecicladorImagem = _reciclador.ImagemDePerfil,

            Materiais = _materiaisSelecionados
                .OrderBy(m => m)
                .ToList(),

            PesoEstimado = peso,

            Volume = volume,

            EstadoMaterial = estado
        };

        // Registra no MockUserService
        try
        {
            await MockUserService.RegistrarSolicitacaoAsync(
                solicitacao
            );

            await DisplayAlert(
                "Solicitação salva",
                "Sua solicitação de reciclagem foi registrada com sucesso!",
                "OK"
            );

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Erro ao salvar",
                $"Não foi possível salvar a solicitação: {ex.Message}",
                "OK"
            );
        }
    }
}
