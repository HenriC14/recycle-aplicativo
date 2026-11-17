using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using recycleAPP.Models;
using recycleAPP.Services;
using static recycleAPP.Services.MockUserService;

namespace recycleAPP.Views;

public partial class PerfilColetorPage : ContentPage, IQueryAttributable
{
    // Usuário cujo perfil está sendo exibido
    public MockUser? UsuarioExibido { get; set; }

    public PerfilColetorPage()
    {
        InitializeComponent();
    }

    // ==========================================
    // RECEBE USUÁRIO DA NAVEGAÇÃO
    // ==========================================

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue("Usuario", out var valor) &&
            valor is MockUser usuario)
        {
            UsuarioExibido = usuario;
        }
    }

    // ==========================================
    // CARREGAMENTO DO PERFIL
    // ==========================================

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Se nenhum usuário foi passado pela navegação,
        // utiliza o usuário logado como alternativa.
        UsuarioExibido ??= CurrentSession.UsuarioLogado;

        var usuario = UsuarioExibido;

        bool logadoComoReciclador =
            CurrentSession.UsuarioLogado?.Tipo == TipoUsuario.Reciclador;

        BarraInferior.IsVisible = logadoComoReciclador;
        NovaReciclagemBorder.IsVisible = logadoComoReciclador;

        if (usuario is null)
        {
            NomeLabel.Text = "Nenhum usuario encontrado";
            SeguidoresLabel.Text = "0";
            EnderecoLabel.Text = "";
            HorarioLabel.Text = "";
            StatusAbertoLabel.Text = "";
            MateriaisFlexLayout.Children.Clear();
            ImagemPerfil.Source = null;
            AcaoPerfilButton.Text = "Seguir";

            return;
        }

        bool ehPerfilProprio =
            CurrentSession.UsuarioLogado is not null &&
            CurrentSession.UsuarioLogado.Username.Equals(
                usuario.Username,
                StringComparison.OrdinalIgnoreCase);

        AtualizarEstadoRelacionamento(usuario, ehPerfilProprio);

        NomeLabel.Text = usuario.Nome;

        ImagemPerfil.Source =
            string.IsNullOrWhiteSpace(usuario.ImagemDePerfil)
                ? null
                : usuario.ImagemDePerfil;

        SeguidoresLabel.Text =
            MockUserService.ObterQuantidadeSeguidores(
                usuario.Username).ToString();

        SeguindoLabel.Text =
            MockUserService.ObterQuantidadeSeguindo(
                usuario.Username).ToString();

        EnderecoLabel.Text =
            usuario.Endereco;

        HorarioLabel.Text =
            usuario.HorarioFuncionamento;

        StatusAbertoLabel.Text =
            usuario.PontoAberto
                ? "Aberto"
                : "Fechado";

        StatusAbertoLabel.TextColor =
            usuario.PontoAberto
                ? Color.FromArgb("#1F8A3B")
                : Color.FromArgb("#D3302F");

        MontarMateriaisAceitos(
            usuario.MateriaisAceitos);

        // Verifica as solicitações somente quando
        // o coletor está visualizando o próprio perfil.
        if (ehPerfilProprio &&
            usuario.Tipo == TipoUsuario.Coletor)
        {
            VerificarSolicitacoesPendentes();
        }
    }

    // ==========================================
    // MATERIAIS ACEITOS PELO COLETOR
    // ==========================================

    private void MontarMateriaisAceitos(
        List<string> materiais)
    {
        MateriaisFlexLayout.Children.Clear();

        foreach (var material in materiais)
        {
            var icone =
                MaterialIconHelper.ObterIcone(material);

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

                        Clip =
                            new Microsoft.Maui.Controls.Shapes
                                .RoundRectangleGeometry
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
                        HorizontalOptions =
                            LayoutOptions.Center
                    }
                }
            };

            MateriaisFlexLayout.Children.Add(item);
        }
    }

    // ==========================================
    // VERIFICAR SOLICITAÇÕES DO COLETOR
    // ==========================================

    private void VerificarSolicitacoesPendentes()
    {
        var coletor = CurrentSession.UsuarioLogado;

        if (coletor is null ||
            coletor.Tipo != TipoUsuario.Coletor)
            return;

        // Evita abrir novamente durante o mesmo login.
        if (CurrentSession.NotificacoesColetorVerificadas)
            return;

        CurrentSession.NotificacoesColetorVerificadas = true;

        // Busca somente solicitações destinadas
        // ao coletor que está logado.
        var solicitacoes = MockUserService.Solicitacoes
            .Where(s =>
                s.ColetorUsername.Equals(
                    coletor.Username,
                    StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.DataCriacao)
            .ToList();

        if (solicitacoes.Count == 0)
            return;

        // Limpa notificações anteriores da interface.
        SolicitacoesStack.Children.Clear();

        // Cria um cartão para cada solicitação.
        foreach (var solicitacao in solicitacoes)
        {
            var card = CriarCardSolicitacao(solicitacao);

            SolicitacoesStack.Children.Add(card);
        }

        // Atualiza a quantidade de solicitações.
        NotificacaoQuantidadeLabel.Text =
            solicitacoes.Count == 1
                ? "Você recebeu 1 nova solicitação."
                : $"Você recebeu {solicitacoes.Count} novas solicitações.";

        // Exibe a notificação sobreposta.
        NotificacaoOverlay.IsVisible = true;
    }

    // ==========================================
    // CRIAR CARTÃO DA SOLICITAÇÃO
    // ==========================================

    private Border CriarCardSolicitacao(
        SolicitacaoReciclagem solicitacao)
    {
        var conteudo = new VerticalStackLayout
        {
            Spacing = 10
        };

        // --------------------------------------
        // FOTO E NOME DO RECICLADOR
        // --------------------------------------

        var imagem = new Image
        {
            WidthRequest = 58,
            HeightRequest = 58,
            Aspect = Aspect.AspectFill
        };

        if (!string.IsNullOrWhiteSpace(
            solicitacao.RecicladorImagem))
        {
            imagem.Source = ImageSource.FromFile(
                solicitacao.RecicladorImagem
            );
        }

        var imagemBorder = new Border
        {
            WidthRequest = 58,
            HeightRequest = 58,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 40
            },
            Content = imagem
        };

        var nomeReciclador = new Label
        {
            Text = solicitacao.RecicladorNome,
            TextColor = Colors.Black,
            FontSize = 17,
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center
        };

        var identificacao = new HorizontalStackLayout
        {
            Spacing = 12,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                imagemBorder,
                nomeReciclador
            }
        };

        conteudo.Children.Add(identificacao);

        // --------------------------------------
        // MATERIAIS SELECIONADOS
        // --------------------------------------

        conteudo.Children.Add(
            CriarTitulo("Materiais selecionados")
        );

        var materiaisLayout = new FlexLayout
        {
            Direction = FlexDirection.Row,
            Wrap = FlexWrap.Wrap,
            JustifyContent = FlexJustify.Start
        };

        foreach (var material in solicitacao.Materiais)
        {
            materiaisLayout.Children.Add(
                CriarMaterialChip(material)
            );
        }

        conteudo.Children.Add(materiaisLayout);

        // --------------------------------------
        // RESPOSTA DA PRIMEIRA PERGUNTA
        // --------------------------------------

        conteudo.Children.Add(
            CriarResposta(
                "Peso estimado da coleta",
                solicitacao.PesoEstimado
            )
        );

        // --------------------------------------
        // RESPOSTAS DA SEGUNDA PERGUNTA
        // --------------------------------------

        string volumeTexto =
            solicitacao.Volume.Count > 0
                ? string.Join(
                    Environment.NewLine,
                    solicitacao.Volume.Select(v => "• " + v)
                )
                : "Não informado";

        conteudo.Children.Add(
            CriarResposta(
                "Volume da coleta",
                volumeTexto
            )
        );

        // --------------------------------------
        // RESPOSTA DA TERCEIRA PERGUNTA
        // --------------------------------------

        conteudo.Children.Add(
            CriarResposta(
                "Estado do material",
                solicitacao.EstadoMaterial
            )
        );

        // --------------------------------------
        // BOTÕES ACEITAR E RECUSAR
        // --------------------------------------

        var botaoRecusar = new Button
        {
            Text = "Recusar",
            BackgroundColor = Color.FromArgb("#AAAAAA"),
            TextColor = Colors.White,
            CornerRadius = 12,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            HeightRequest = 44,
            HorizontalOptions = LayoutOptions.Fill
        };

        botaoRecusar.Clicked += OnAcaoSolicitacaoFuturaClicked;

        var botaoAceitar = new Button
        {
            Text = "Aceitar",
            BackgroundColor = Color.FromArgb("#28C85A"),
            TextColor = Colors.White,
            CornerRadius = 12,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            HeightRequest = 44,
            HorizontalOptions = LayoutOptions.Fill
        };

        botaoAceitar.Clicked += OnAcaoSolicitacaoFuturaClicked;

        var botoes = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            ColumnSpacing = 8,
            Children =
            {
                botaoRecusar,
                botaoAceitar
            }
        };

        Grid.SetColumn(botaoRecusar, 0);
        Grid.SetColumn(botaoAceitar, 1);

        conteudo.Children.Add(botoes);

        // --------------------------------------
        // BORDA DO CARTÃO
        // --------------------------------------

        return new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#D0E8D6"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 14
            },
            Padding = 12,
            Content = conteudo
        };
    }

    // ==========================================
    // CRIAR TÍTULO DE UMA SEÇÃO
    // ==========================================

    private Label CriarTitulo(string texto)
    {
        return new Label
        {
            Text = texto,
            TextColor = Colors.Black,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold
        };
    }

    // ==========================================
    // CRIAR RESPOSTA DE UMA PERGUNTA
    // ==========================================

    private VerticalStackLayout CriarResposta(
        string pergunta,
        string resposta)
    {
        var layout = new VerticalStackLayout
        {
            Spacing = 3
        };

        layout.Children.Add(
            CriarTitulo(pergunta)
        );

        layout.Children.Add(
            new Label
            {
                Text = string.IsNullOrWhiteSpace(resposta)
                    ? "Não informado"
                    : resposta,

                TextColor = Color.FromArgb("#444444"),
                FontSize = 13,
                LineBreakMode = LineBreakMode.WordWrap
            }
        );

        return layout;
    }

    // ==========================================
    // CRIAR IDENTIFICAÇÃO VISUAL DO MATERIAL
    // ==========================================

    private Border CriarMaterialChip(string material)
    {
        string nome = material;

        Color fundo;
        Color texto;

        switch (material.ToLowerInvariant())
        {
            case "plastico":
            case "plástico":
                nome = "♻ Plástico";
                fundo = Color.FromArgb("#F9DADA");
                texto = Color.FromArgb("#B52626");
                break;

            case "metal":
                nome = "♻ Metal";
                fundo = Color.FromArgb("#FFF0B8");
                texto = Color.FromArgb("#8A6810");
                break;

            case "vidro":
                nome = "♻ Vidro";
                fundo = Color.FromArgb("#D5F5D9");
                texto = Color.FromArgb("#197B38");
                break;

            case "papel":
                nome = "♻ Papel";
                fundo = Color.FromArgb("#D9EDFF");
                texto = Color.FromArgb("#1976B9");
                break;

            default:
                fundo = Color.FromArgb("#E5E5E5");
                texto = Colors.Black;
                break;
        }

        return new Border
        {
            BackgroundColor = fundo,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 10
            },
            Padding = new Thickness(10, 6),
            Margin = new Thickness(0, 0, 6, 6),

            Content = new Label
            {
                Text = nome,
                TextColor = texto,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold
            }
        };
    }

    // ==========================================
    // BOTÕES PROVISÓRIOS
    // ==========================================

    private async void OnAcaoSolicitacaoFuturaClicked(
        object sender,
        EventArgs e)
    {
        await DisplayAlert(
            "Funcionalidade futura",
            "A opção de aceitar ou recusar solicitações será implementada posteriormente.",
            "OK"
        );
    }

    // ==========================================
    // FECHAR NOTIFICAÇÃO
    // ==========================================

    private void OnFecharNotificacoesClicked(
        object sender,
        EventArgs e)
    {
        NotificacaoOverlay.IsVisible = false;
    }

    // ==========================================
    // ABRIR TELA DE SOLICITAÇÃO DE RECICLAGEM
    // ==========================================

    private async void OnNovaReciclagemTapped(
        object sender,
        EventArgs e)
    {
        if (UsuarioExibido is null)
        {
            await DisplayAlert(
                "Solicitação",
                "Não foi possível identificar este coletor.",
                "OK"
            );

            return;
        }

        var parametros = new Dictionary<string, object>
        {
            { "Coletor", UsuarioExibido }
        };

        await Shell.Current.GoToAsync(
            nameof(SolicitacaoReciclagemPage),
            parametros
        );
    }

    // ==========================================
    // BOTÃO DE AÇÃO DO PERFIL
    // ==========================================


    // ==========================================
    // BOTÃO SEGUIR / NÃO SEGUIR
    // ==========================================

    private async void OnAcaoPerfilClicked(
        object sender,
        EventArgs e)
    {
        var usuario = UsuarioExibido;
        var usuarioLogado = CurrentSession.UsuarioLogado;

        if (usuario is null)
            return;

        bool ehPerfilProprio =
            usuarioLogado is not null &&
            usuarioLogado.Username.Equals(
                usuario.Username,
                StringComparison.OrdinalIgnoreCase);

        if (ehPerfilProprio)
        {
            await DisplayAlert(
                "Editar perfil",
                "Tela de edicao de perfil ainda nao implementada.",
                "OK"
            );

            return;
        }

        if (usuarioLogado is null)
        {
            await DisplayAlert(
                "Entrar na conta",
                "Voce precisa estar logado para seguir outra conta.",
                "OK"
            );

            return;
        }

        bool jaEstaSeguindo = MockUserService.EstaSeguindo(
            usuarioLogado.Username,
            usuario.Username);

        if (jaEstaSeguindo)
        {
            await MockUserService.DeixarDeSeguirUsuarioAsync(
                usuarioLogado.Username,
                usuario.Username);
        }
        else
        {
            await MockUserService.SeguirUsuarioAsync(
                usuarioLogado.Username,
                usuario.Username);
        }

        AtualizarEstadoRelacionamento(
            usuario,
            ehPerfilProprio: false
        );

        SeguidoresLabel.Text =
            MockUserService.ObterQuantidadeSeguidores(
                usuario.Username).ToString();

        SeguindoLabel.Text =
            MockUserService.ObterQuantidadeSeguindo(
                usuario.Username).ToString();
    }

    // ==========================================
    // ATUALIZAR ESTADO DO RELACIONAMENTO
    // ==========================================

    private void AtualizarEstadoRelacionamento(
        MockUser usuario,
        bool ehPerfilProprio)
    {
        if (ehPerfilProprio)
        {
            AcaoPerfilButton.Text = "Editar perfil";

            AcaoPerfilButton.BackgroundColor =
                Color.FromArgb("#3FBF63");

            AcaoPerfilButton.IsEnabled = true;

            return;
        }

        var usuarioLogado = CurrentSession.UsuarioLogado;

        bool estaSeguindo =
            usuarioLogado is not null &&
            MockUserService.EstaSeguindo(
                usuarioLogado.Username,
                usuario.Username);

        if (estaSeguindo)
        {
            AcaoPerfilButton.Text = "Não seguir";

            AcaoPerfilButton.BackgroundColor =
                Color.FromArgb("#AAAAAA");
        }
        else
        {
            AcaoPerfilButton.Text = "Seguir";

            AcaoPerfilButton.BackgroundColor =
                Color.FromArgb("#3FBF63");
        }

        AcaoPerfilButton.IsEnabled =
            usuarioLogado is not null;
    }


    // ==========================================
    // SAIR DA CONTA
    // ==========================================

    private async void OnLogoutClicked(
        object sender,
        EventArgs e)
    {
        CurrentSession.UsuarioLogado = null;

        CurrentSession.NotificacoesColetorVerificadas = false;

        await Shell.Current.GoToAsync(
            nameof(SelecionarTipoPage)
        );
    }
}
