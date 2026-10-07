using recycleAPP.Models;

namespace recycleAPP.Views;

[QueryProperty(nameof(Produto), "Produto")]
public partial class DetalhesProdutoPage : ContentPage
{
    public ProdutoLoja? Produto
    {
        set
        {
            if (value is null)
                return;

            NomeLabel.Text = value.Nome;
            EmpresaLabel.Text = value.Empresa;
            CustoLabel.Text = value.CustoPontosTexto;
            ImagemProduto.Source = value.ImagemUrl;
            DescricaoLabel.Text = value.Descricao;
        }
    }

    public DetalhesProdutoPage()
    {
        InitializeComponent();
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnTrocarClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Recycle", "Funcionalidade de troca ainda nao implementada.", "OK");
    }
}