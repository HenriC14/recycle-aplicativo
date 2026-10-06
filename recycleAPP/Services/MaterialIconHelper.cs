namespace recycleAPP.Services;

/// <summary>
/// Centraliza o mapeamento entre nome do material e o icone (imagem) correspondente,
/// pra nao duplicar essa logica em cada tela que mostra materiais.
/// </summary>
public static class MaterialIconHelper
{
    public static string ObterIcone(string? material) => material?.Trim().ToLowerInvariant() switch
    {
        "plastico" => "simbolo_plastico.png",
        "metal" => "simbolo_metal.png",
        "vidro" => "simbolo_vidro.png",
        "papel" => "simbolo_papel.png",
        _ => string.Empty
    };
}