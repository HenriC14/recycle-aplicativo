namespace recycleAPP.Models;

public class ProdutoLoja
{
    public string Nome { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public int CustoPontos { get; set; }
    public string Empresa { get; set; } = string.Empty;

    public string CustoPontosTexto => $"{CustoPontos} Pontos";
}