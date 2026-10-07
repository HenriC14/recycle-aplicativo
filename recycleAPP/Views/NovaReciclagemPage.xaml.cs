using System.Collections.ObjectModel;
using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class NovaReciclagemPage : ContentPage
{
    // Serviço que fornece os dados dos coletores
    private readonly MockUserService _mockService = new();

    // Guarda TODOS os pontos de coleta
    private List<MockUser> _todosOsPontos = new();

    // Guarda os materiais atualmente selecionados nos filtros
    private readonly HashSet<string> _filtrosSelecionados =
        new(StringComparer.OrdinalIgnoreCase);

    // Lista que está ligada à CollectionView
    public ObservableCollection<MockUser> PontosFiltrados { get; set; }
        = new();

    public NovaReciclagemPage()
    {
        InitializeComponent();

        // Define o BindingContext para que o XAML
        // consiga acessar PontosFiltrados
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Carrega os pontos apenas se ainda não foram carregados
        if (_todosOsPontos.Count == 0)
        {
            await CarregarPontosAsync();
        }
    }

    private async Task CarregarPontosAsync()
    {
        // Busca todos os coletores do Mock
        _todosOsPontos = await _mockService.GetPontosColetaAsync();

        // Inicialmente, mostra todos
        PontosFiltrados.Clear();

        foreach (var ponto in _todosOsPontos)
        {
            PontosFiltrados.Add(ponto);
        }
    }

    private void OnFiltroClicked(object sender, EventArgs e)
    {
        if (sender is not Button button)
            return;

        // Obtém o material definido no CommandParameter
        string material = button.CommandParameter?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(material))
            return;

        // Se o filtro já estiver selecionado,
        // remove.
        //
        // Se não estiver selecionado,
        // adiciona.
        if (_filtrosSelecionados.Contains(material))
        {
            _filtrosSelecionados.Remove(material);
        }
        else
        {
            _filtrosSelecionados.Add(material);
        }

        // Aplica os filtros
        AplicarFiltro();

        // Atualiza visualmente os botões
        AtualizarBotoes();
    }

    private void AplicarFiltro()
    {
        // Se nenhum filtro estiver selecionado,
        // mostra todos os coletores novamente.
        if (_filtrosSelecionados.Count == 0)
        {
            PontosFiltrados.Clear();

            foreach (var ponto in _todosOsPontos)
            {
                PontosFiltrados.Add(ponto);
            }

            return;
        }

        // Filtragem EXATA
        var resultado = _todosOsPontos
            .Where(ponto =>
            {
                // Transforma os materiais aceitos pelo coletor
                // em HashSet para poder comparar os conjuntos.
                var materiaisDoColetor =
                    ponto.MateriaisAceitos
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // SetEquals significa:
                //
                // "Os materiais do coletor são exatamente
                // iguais aos materiais selecionados?"
                //
                // Não pode faltar nenhum.
                // Não pode sobrar nenhum.
                return materiaisDoColetor.SetEquals(
                    _filtrosSelecionados);
            })
            .ToList();

        // Atualiza a CollectionView
        PontosFiltrados.Clear();

        foreach (var ponto in resultado)
        {
            PontosFiltrados.Add(ponto);
        }
    }

    private void AtualizarBotoes()
    {
        AtualizarBotao(
            PlasticoButton,
            "Plastico",
            "#696969");

        AtualizarBotao(
            MetalButton,
            "Metal",
            "#696969");

        AtualizarBotao(
            VidroButton,
            "Vidro",
            "#696969");

        AtualizarBotao(
            PapelButton,
            "Papel",
            "#696969");
    }

    private void AtualizarBotao(
        Button button,
        string material,
        string corOriginal)
    {
        bool selecionado =
            _filtrosSelecionados.Contains(material);

        if (selecionado)
        {
            // Quando selecionado, deixa o botão verde
            // para indicar que o filtro está ativo.
            button.BackgroundColor = Color.FromArgb("#3FBF63");
            button.TextColor = Colors.White;
        }
        else
        {
            // Quando não selecionado, volta para
            // a cor original.
            button.BackgroundColor = Color.FromArgb(corOriginal);
            button.TextColor = Colors.White;
        }
    }
}