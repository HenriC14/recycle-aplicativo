using recycleAPP.Models;

namespace recycleAPP.Views.Controls;

public partial class ProductCard : ContentView
{
    public static readonly BindableProperty ProdutoProperty =
        BindableProperty.Create(
            nameof(Produto),
            typeof(ProdutoLoja),
            typeof(ProductCard),
            propertyChanged: OnProdutoChanged);

    public ProdutoLoja? Produto
    {
        get => (ProdutoLoja?)GetValue(ProdutoProperty);
        set => SetValue(ProdutoProperty, value);
    }

    public ProductCard()
    {
        InitializeComponent();
    }

    private static void OnProdutoChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProductCard card)
        {
            card.BindingContext = newValue as ProdutoLoja;
        }
    }

    private async void OnCardTapped(object sender, EventArgs e)
    {
        if (Produto is null)
            return;

        await Shell.Current.GoToAsync(nameof(Views.DetalhesProdutoPage), new Dictionary<string, object>
        {
            ["Produto"] = Produto
        });
    }
}