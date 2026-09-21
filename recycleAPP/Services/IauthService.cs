namespace recycleAPP.Services;


/// Contrato de autenticacao/cadastro. Hoje implementado por MockAuthService (dados falsos).
/// No futuro, implemente por ex. ApiAuthService (chama sua API) ou
/// DbAuthService (consulta banco local/SQLite), sem precisar mudar as paginas.

public interface IAuthService
{
    Task<MockUser?> TryLoginAsync(string username, string senha);


    /// Tenta cadastrar um novo usuario. Retorna null em caso de sucesso,
    /// ou uma mensagem de erro (ex: "Nome de usuario ja existe").

    Task<string?> RegisterAsync(MockUser novoUsuario);
}