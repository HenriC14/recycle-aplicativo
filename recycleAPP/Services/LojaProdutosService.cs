using recycleAPP.Models;

namespace recycleAPP.Services;

public static class LojaProdutosService
{
    public static List<ProdutoLoja> ObterProdutos() => new()
    {
        new ProdutoLoja { Nome = "Cafeteira Britania 18 Xicaras", Emoji = "\u2615", CustoPontos = 5000 },
        new ProdutoLoja { Nome = "Aspirador de Po Mondial Turbo", Emoji = "\U0001F9F9", CustoPontos = 5500 },
        new ProdutoLoja { Nome = "Fone de Ouvido Sem Fio", Emoji = "\U0001F3A7", CustoPontos = 1200 },
        new ProdutoLoja { Nome = "Microondas Eletrolux", Emoji = "\U0001F4E6", CustoPontos = 7000 },
        new ProdutoLoja { Nome = "Fone Sem Fio Orbit Max Cinza", Emoji = "\U0001F3A7", CustoPontos = 3000 },
        new ProdutoLoja { Nome = "Sanduicheira Press Grill", Emoji = "\U0001F35E", CustoPontos = 4000 },
        new ProdutoLoja { Nome = "Mouse Logitech Com Fio", Emoji = "\U0001F5B1", CustoPontos = 500 },
        new ProdutoLoja { Nome = "Fritadeira Eletrica Air Fryer", Emoji = "\U0001F35F", CustoPontos = 5000 },
        new ProdutoLoja { Nome = "Vale Presente Google Play", Emoji = "\U0001F3AE", CustoPontos = 2500 },
        new ProdutoLoja { Nome = "Teclado Mecanico RGB", Emoji = "\u2328", CustoPontos = 3500 },
        new ProdutoLoja { Nome = "Caixa de Som Bluetooth", Emoji = "\U0001F50A", CustoPontos = 2800 },
        new ProdutoLoja { Nome = "Garrafa Termica Inox 1L", Emoji = "\U0001F9F4", CustoPontos = 900 },
    };
}