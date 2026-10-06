using recycleAPP.Services;
using System.Globalization;

namespace recycleAPP.Views;

public partial class PerfilColetorPage : ContentPage
{
    // Usuario cujo perfil esta sendo mostrado. Por enquanto sempre e o proprio
    // usuario logado, ate existir uma tela de "ver perfil de outra pessoa".
    public MockUser? UsuarioExibido { get; set; }

    public PerfilColetorPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UsuarioExibido ??= CurrentSession.UsuarioLogado;
        var usuario = UsuarioExibido;

        // Navbar de baixo e botao de nova reciclagem so aparecem pra quem esta
        // logado como Reciclador. Coletor tera sua propria navbar depois.
        bool logadoComoReciclador = CurrentSession.UsuarioLogado?.Tipo == TipoUsuario.Reciclador;
        BarraInferior.IsVisible = logadoComoReciclador;
        NovaReciclagemBorder.IsVisible = logadoComoReciclador;

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
        EnderecoLabel.Text = usuario.Endereco;

        HorarioLabel.Text = usuario.HorarioFuncionamento;
        StatusAbertoLabel.Text = usuario.PontoAberto ? "Aberto" : "Fechado";
        StatusAbertoLabel.TextColor = usuario.PontoAberto ? Color.FromArgb("#1F8A3B") : Color.FromArgb("#D3302F");

        MontarMateriaisAceitos(usuario.MateriaisAceitos);
    }

    private void MontarMateriaisAceitos(List<string> materiais)
    {
        MateriaisFlexLayout.Children.Clear();

        foreach (var material in materiais)
        {
            var icone = MaterialIconHelper.ObterIcone(material);
            if (string.IsNullOrEmpty(icone))
                continue;

            var item = new VerticalStackLayout
            {
                Spacing = 4,
                Margin = new Thickness(0, 0, 14, 10),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image
                    {
                        Source = icone,
                        WidthRequest = 48,
                        HeightRequest = 48,
                        Aspect = Aspect.AspectFill,
                        Clip = new Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry
                        {
                            CornerRadius = 10,
                            Rect = new Rect(0, 0, 48, 48)
                        }
                    },
                    new Label
                    {
                        Text = material,
                        TextColor = Colors.Black,
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.Center
                    }
                }
            };

            MateriaisFlexLayout.Children.Add(item);
        }
    }

    private async void OnNovaReciclagemTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NovaReciclagemPage));
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