namespace recycleAPP.Views.Controls;

public partial class BottomNavBar : ContentView
{
    public BottomNavBar()
    {
        InitializeComponent();
    }

    private async void OnInicioTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.InicioPage));
    }

    private async void OnNovaReciclagemTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.NovaReciclagemPage));
    }

    private async void OnLojaTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Views.LojaPontosPage));
    }
}