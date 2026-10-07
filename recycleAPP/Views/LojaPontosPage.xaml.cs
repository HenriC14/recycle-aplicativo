using recycleAPP.Services;

namespace recycleAPP.Views;

public partial class LojaPontosPage : ContentPage
{
    public LojaPontosPage()
    {
        InitializeComponent();

        ProdutosCollectionView.ItemsSource =
            LojaProdutosService.ObterProdutos();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var usuario = CurrentSession.UsuarioLogado;

        if (usuario is not null)
        {
            PontosLabel.Text =
                $"Pontos: {usuario.SaldoPontos}";

            ImagemPerfil.Source =
                usuario.ImagemDePerfil;
        }
        else
        {
            PontosLabel.Text = "Pontos: 0";

            ImagemPerfil.Source = null;
        }
    }

    private async void OnVoltarClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(SelecionarTipoPage));
    }
}