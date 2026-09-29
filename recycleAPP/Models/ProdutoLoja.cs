namespace recycleAPP.Models;

public class ProdutoLoja
{
    public string Nome { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public int CustoPontos { get; set; }

    public string CustoPontosTexto => $"{CustoPontos} Pontos";
}