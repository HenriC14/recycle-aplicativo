using System;
using System.Collections.Generic;
using System.Text;

namespace recycleAPP.Models;

public class SolicitacaoReciclagem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    // Dados do coletor
    public string ColetorUsername { get; set; } = string.Empty;
    public string ColetorNome { get; set; } = string.Empty;
    public string ColetorImagem { get; set; } = string.Empty;

    // Dados do reciclador
    public string RecicladorUsername { get; set; } = string.Empty;
    public string RecicladorNome { get; set; } = string.Empty;
    public string RecicladorImagem { get; set; } = string.Empty;

    // Materiais selecionados
    public List<string> Materiais { get; set; } = new();

    // Peso estimado
    public string PesoEstimado { get; set; } = string.Empty;

    // Volume selecionado
    public List<string> Volume { get; set; } = new();

    // Estado do material
    public string EstadoMaterial { get; set; } = string.Empty;
}
