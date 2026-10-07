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
        new ProdutoLoja { Nome = "Cafeteira Britania 18 Xicaras", Empresa = "Britania", ImagemUrl = "cafeteira_britania.png", CustoPontos = 5000, Descricao = "A Cafeteira Elétrica Britânia CP18 foi feita para quem busca praticidade e um café sempre quentinho no dia a dia. Com capacidade para passar até 18 xícaras por vez, ela conta com jarra de vidro resistente, placa aquecedora para manter a bebida na temperatura ideal e filtro permanente lavável, que dispensa o uso do filtro de papel. Além disso, seu sistema corta-pingos permite servir o café mesmo enquanto ele está sendo preparado, sem sujeira nem complicação." },
        new ProdutoLoja { Nome = "Aspirador de Po Mondial Turbo", Empresa = "Mondial", ImagemUrl = "aspirador_de_po.png", CustoPontos = 5500, Descricao = "O Aspirador de Pó Mondial Turbo é perfeito para manter seu ambiente limpo e arejado. Com tecnologia avançada, ele oferece uma limpeza eficiente e rápida, ideal para uso doméstico." },
        new ProdutoLoja { Nome = "Fone de Ouvido Sem Fio", Empresa = "JBL", ImagemUrl = "fone_sem_fio.png", CustoPontos = 1200, Descricao = "Fone de ouvido sem fio com tecnologia de cancelamento de som e bateria de longa duração." },
        new ProdutoLoja { Nome = "Microondas Eletrolux", Empresa = "Eletrolux", ImagemUrl = "microondas_eletrolux.png", CustoPontos = 7000, Descricao = "Micro-ondas com capacidade para preparar refeições rapidamente e com tecnologia de aquecimento uniforme." },
        new ProdutoLoja { Nome = "Fone Sem Fio Orbit Max Cinza", Empresa = "JBL", ImagemUrl = "fone_sem_fio.png", CustoPontos = 3000, Descricao = "Fone sem fio com qualidade de som excepcional e design elegante." },
        new ProdutoLoja { Nome = "Sanduicheira Press Grill", Empresa = "Britania", ImagemUrl = "sanduicheira.png", CustoPontos = 4000, Descricao = "Sanduicheira com pressão para preparar lanches deliciosos e saudáveis." },
        new ProdutoLoja { Nome = "Mouse Logitech Com Fio", Empresa = "Logitech", ImagemUrl = "mouse.png", CustoPontos = 500, Descricao = "Mouse ergonômico com sensor óptico de alta precisão e design moderno." },
        new ProdutoLoja { Nome = "Fritadeira Eletrica Air Fryer", Empresa = "Mondial", ImagemUrl = "air_fryer.png", CustoPontos = 5000, Descricao = "Fritadeira elétrica com tecnologia air frying para preparar refeições deliciosas e saudáveis." },
        new ProdutoLoja { Nome = "Vale Presente Google Play", Empresa = "Google", ImagemUrl = PlaceholderUrl("giftcard"), CustoPontos = 2500, Descricao = "Vale presente para compra de conteúdos no Google Play Store." },
        new ProdutoLoja { Nome = "Teclado Mecanico RGB", Empresa = "Logitech", ImagemUrl = PlaceholderUrl("teclado"), CustoPontos = 3500, Descricao = "Teclado mecânico com iluminação RGB e switches de alta qualidade." },
        new ProdutoLoja { Nome = "Caixa de Som Bluetooth", Empresa = "JBL", ImagemUrl = PlaceholderUrl("caixasom"), CustoPontos = 2800, Descricao = "Caixa de som com conectividade Bluetooth e qualidade de som excepcional." },
        new ProdutoLoja { Nome = "Garrafa Termica Inox 1L", Empresa = "Eletrolux", ImagemUrl = PlaceholderUrl("garrafa"), CustoPontos = 900, Descricao = "Garrafa térmica em aço inox com capacidade para 1 litro e isolamento térmico." },
    };
}