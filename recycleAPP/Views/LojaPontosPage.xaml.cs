using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class LojaPontosPage : ContentPage
{
    public LojaPontosPage()
    {
        InitializeComponent();
        ProdutosCollectionView.ItemsSource = LojaProdutosService.ObterProdutos();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var usuario = CurrentSession.UsuarioLogado;
        PontosLabel.Text = usuario is not null
            ? $"Pontos: {usuario.SaldoPontos}"
            : "Pontos: 0";
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SelecionarTipoPage));
    }
}