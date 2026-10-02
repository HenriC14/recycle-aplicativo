using recycleAPP.Models;

namespace recycleAPP.Services;

/// <summary>
/// Catalogo mockado da loja de pontos, ate existir cadastro real de produtos
/// pela EmpresaParceira (ja previsto no diagrama de casos de uso).
/// ImagemUrl hoje aponta pra um servico de placeholder (picsum.photos) so
/// pra teste visual; quando houver banco, troca por uma URL real salva ali.
/// </summary>
public static class LojaProdutosService
{
    private static string PlaceholderUrl(string seed) =>
        $"https://picsum.photos/seed/{seed}/300/300";

    public static List<ProdutoLoja> ObterProdutos() => new()
    {
        new ProdutoLoja { Nome = "Cafeteira Britania 18 Xicaras", Empresa = "Britania", ImagemUrl = "dotnet_bot.png", CustoPontos = 5000 },
        new ProdutoLoja { Nome = "Aspirador de Po Mondial Turbo", Empresa = "Mondial", ImagemUrl = PlaceholderUrl("aspirador"), CustoPontos = 5500 },
        new ProdutoLoja { Nome = "Fone de Ouvido Sem Fio", Empresa = "JBL", ImagemUrl = PlaceholderUrl("fone1"), CustoPontos = 1200 },
        new ProdutoLoja { Nome = "Microondas Eletrolux", Empresa = "Eletrolux", ImagemUrl = PlaceholderUrl("microondas"), CustoPontos = 7000 },
        new ProdutoLoja { Nome = "Fone Sem Fio Orbit Max Cinza", Empresa = "JBL", ImagemUrl = PlaceholderUrl("fone2"), CustoPontos = 3000 },
        new ProdutoLoja { Nome = "Sanduicheira Press Grill", Empresa = "Britania", ImagemUrl = PlaceholderUrl("sanduicheira"), CustoPontos = 4000 },
        new ProdutoLoja { Nome = "Mouse Logitech Com Fio", Empresa = "Logitech", ImagemUrl = PlaceholderUrl("mouse"), CustoPontos = 500 },
        new ProdutoLoja { Nome = "Fritadeira Eletrica Air Fryer", Empresa = "Mondial", ImagemUrl = PlaceholderUrl("airfryer"), CustoPontos = 5000 },
        new ProdutoLoja { Nome = "Vale Presente Google Play", Empresa = "Google", ImagemUrl = PlaceholderUrl("giftcard"), CustoPontos = 2500 },
        new ProdutoLoja { Nome = "Teclado Mecanico RGB", Empresa = "Logitech", ImagemUrl = PlaceholderUrl("teclado"), CustoPontos = 3500 },
        new ProdutoLoja { Nome = "Caixa de Som Bluetooth", Empresa = "JBL", ImagemUrl = PlaceholderUrl("caixasom"), CustoPontos = 2800 },
        new ProdutoLoja { Nome = "Garrafa Termica Inox 1L", Empresa = "Eletrolux", ImagemUrl = PlaceholderUrl("garrafa"), CustoPontos = 900 },
    };
}