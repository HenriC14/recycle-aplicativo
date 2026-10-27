using System.Collections.Generic;
using System.Linq;

namespace recycleAPP.Services;

public class SolicitacaoMock
{
    public string RecicladorUsername { get; set; } = string.Empty;
    public string RecicladorNome { get; set; } = string.Empty;

    public string ColetorUsername { get; set; } = string.Empty;
    public string ColetorNome { get; set; } = string.Empty;
}

public static class SolicitacaoMockService
{
    // Temporário: futuramente será substituído pelo banco de dados.
    private static readonly List<SolicitacaoMock> Solicitacoes = new();

    public static void CriarSolicitacao(
        MockUser reciclador,
        MockUser coletor)
    {
        if (reciclador.Tipo != TipoUsuario.Reciclador ||
            coletor.Tipo != TipoUsuario.Coletor)
        {
            return;
        }

        // Evita criar a mesma solicitação várias vezes.
        bool jaExiste = Solicitacoes.Any(s =>
            s.RecicladorUsername.Equals(
                reciclador.Username,
                StringComparison.OrdinalIgnoreCase) &&
            s.ColetorUsername.Equals(
                coletor.Username,
                StringComparison.OrdinalIgnoreCase));

        if (jaExiste)
            return;

        Solicitacoes.Add(new SolicitacaoMock
        {
            RecicladorUsername = reciclador.Username,
            RecicladorNome = reciclador.Nome,

            ColetorUsername = coletor.Username,
            ColetorNome = coletor.Nome
        });
    }

    public static SolicitacaoMock? ObterSolicitacaoParaColetor(
        MockUser coletor)
    {
        return Solicitacoes.FirstOrDefault(s =>
            s.ColetorUsername.Equals(
                coletor.Username,
                StringComparison.OrdinalIgnoreCase));
    }

    public static void IgnorarSolicitacao(
        SolicitacaoMock solicitacao)
    {
        Solicitacoes.Remove(solicitacao);
    }
}