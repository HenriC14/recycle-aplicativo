using recycleAPP.Services;

namespace recycleAPP.Views.Controls;

public partial class SettingsMenu : ContentView
{
    public SettingsMenu()
    {
        InitializeComponent();
    }

    private void OnEngrenagemTapped(object sender, EventArgs e)
    {
        DropdownBorder.IsVisible = !DropdownBorder.IsVisible;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        CurrentSession.UsuarioLogado = null;
        await Shell.Current.GoToAsync(nameof(SelecionarTipoPage));
    }

}