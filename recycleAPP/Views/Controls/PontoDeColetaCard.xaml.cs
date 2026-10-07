using recycleAPP.Services;

namespace recycleAPP.Views.Controls;

public partial class PontoDeColetaCard : ContentView
{
    public PontoDeColetaCard()
    {
        InitializeComponent();
    }

    private async void OnCardTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is not MockUser usuario)
            return;

        var parametros = new Dictionary<string, object>
        {
            { "Usuario", usuario }
        };

        await Shell.Current.GoToAsync(
            nameof(Views.PerfilColetorPage),
            parametros);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is not MockUser usuario)
            return;

        AceitaPlastico.IsVisible =
            usuario.MateriaisAceitos.Contains(
                "Plastico",
                StringComparer.OrdinalIgnoreCase);

        AceitaMetal.IsVisible =
            usuario.MateriaisAceitos.Contains(
                "Metal",
                StringComparer.OrdinalIgnoreCase);

        AceitaVidro.IsVisible =
            usuario.MateriaisAceitos.Contains(
                "Vidro",
                StringComparer.OrdinalIgnoreCase);

        AceitaPapel.IsVisible =
            usuario.MateriaisAceitos.Contains(
                "Papel",
                StringComparer.OrdinalIgnoreCase);
    }
}