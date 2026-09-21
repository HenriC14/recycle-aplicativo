namespace recycleAPP.Services;

public enum TipoUsuario
{
    Reciclador,
    Coletor,
    EmpresaParceira
}

public class MockUser
{
    public string Nome { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public TipoUsuario Tipo { get; set; }
}

/// <summary>
/// Implementacao FALSA de IAuthService, so pra desenvolver o front-end sem backend.
/// Quando o banco/API estiver pronto, crie outra classe (ex: ApiAuthService)
/// implementando IAuthService, e troque o registro em MauiProgram.cs.
/// Essa classe pode ser deletada nesse momento.
/// </summary>
public class MockUserService : IAuthService
{
    private static readonly List<MockUser> Usuarios = new()
    {
        new MockUser { Nome = "Ferro Velho Conceicao", Username = "ferrovelho", Cpf = "00000000000", Email = "ferrovelho@teste.com", Senha = "123456", Tipo = TipoUsuario.Coletor },
        new MockUser { Nome = "Maria Reciclagem", Username = "maria", Cpf = "11111111111", Email = "maria@teste.com", Senha = "123456", Tipo = TipoUsuario.Reciclador },
        new MockUser { Nome = "EcoParceira LTDA", Username = "ecoparceira", Cpf = "22222222222", Email = "eco@teste.com", Senha = "123456", Tipo = TipoUsuario.EmpresaParceira },
    };

    public Task<MockUser?> TryLoginAsync(string username, string senha)
    {
        var user = Usuarios.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
            u.Senha == senha);

        return Task.FromResult(user);
    }

    public Task<string?> RegisterAsync(MockUser novoUsuario)
    {
        if (Usuarios.Any(u => u.Username.Equals(novoUsuario.Username, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult<string?>("Esse nome de usuario ja esta em uso.");

        if (Usuarios.Any(u => u.Cpf == novoUsuario.Cpf))
            return Task.FromResult<string?>("Esse CPF ja esta cadastrado.");

        if (Usuarios.Any(u => u.Email.Equals(novoUsuario.Email, StringComparison.OrdinalIgnoreCase)))
            return Task.FromResult<string?>("Esse e-mail ja esta cadastrado.");

        // Por padrao, cadastro publico vira Reciclador.
        // Coletor e EmpresaParceira podem ter fluxo de cadastro proprio depois.
        novoUsuario.Tipo = TipoUsuario.Reciclador;
        Usuarios.Add(novoUsuario);

        return Task.FromResult<string?>(null);
    }
}