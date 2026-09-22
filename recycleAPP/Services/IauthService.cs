namespace recycleAPP.Services;

/// <summary>
/// Contrato de autenticacao/cadastro. Hoje implementado por MockUserService (dados falsos).
/// No futuro, implemente por ex. ApiAuthService (chama sua API) ou
/// DbAuthService (consulta banco local/SQLite), sem precisar mudar as paginas.
/// </summary>
public interface IAuthService
{
    /// <summary>Login do Reciclador (Pessoa), por username + senha.</summary>
    Task<MockUser?> TryLoginAsync(string username, string senha);

    /// <summary>Login do Coletor (Empresa), por CPF do responsavel + CNPJ + senha.</summary>
    Task<MockUser?> TryLoginColetorAsync(string cpf, string cnpj, string senha);

    /// <summary>Cadastra um Reciclador. Retorna null em sucesso, ou mensagem de erro.</summary>
    Task<string?> RegisterAsync(MockUser novoUsuario);

    /// <summary>Cadastra um Coletor (com CNPJ). Retorna null em sucesso, ou mensagem de erro.</summary>
    Task<string?> RegisterColetorAsync(MockUser novoColetor);
}