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
    public string Cnpj { get; set; } = string.Empty; // usado so por Coletor/EmpresaParceira
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
        new MockUser { Nome = "Ferro Velho Conceicao", Username = "ferrovelho", Cpf = "00000000000", Cnpj = "11222333000181", Email = "ferrovelho@teste.com", Senha = "123456", Tipo = TipoUsuario.Coletor },
        new MockUser { Nome = "Maria Reciclagem", Username = "maria", Cpf = "11111111111", Email = "maria@teste.com", Senha = "123456", Tipo = TipoUsuario.Reciclador },
        new MockUser { Nome = "EcoParceira LTDA", Username = "ecoparceira", Cpf = "22222222222", Cnpj = "99888777000166", Email = "eco@teste.com", Senha = "123456", Tipo = TipoUsuario.EmpresaParceira },
    };

    public Task<MockUser?> TryLoginAsync(string username, string senha)
    {
        var user = Usuarios.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
            u.Senha == senha);

        return Task.FromResult(user);
    }

    public Task<MockUser?> TryLoginColetorAsync(string cpf, string cnpj, string senha)
    {
        var user = Usuarios.FirstOrDefault(u =>
            u.Tipo == TipoUsuario.Coletor &&
            u.Cpf == cpf &&
            u.Cnpj == cnpj &&
            u.Senha == senha);

        return Task.FromResult(user);
    }

    public Task<string?> RegisterAsync(MockUser novoUsuario)
    {
        var erro = ValidarDuplicidade(novoUsuario, checarCnpj: false);
        if (erro is not null)
            return Task.FromResult<string?>(erro);

        novoUsuario.Tipo = TipoUsuario.Reciclador;
        Usuarios.Add(novoUsuario);

        return Task.FromResult<string?>(null);
    }

    public Task<string?> RegisterColetorAsync(MockUser novoColetor)
    {
        var erro = ValidarDuplicidade(novoColetor, checarCnpj: true);
        if (erro is not null)
            return Task.FromResult<string?>(erro);

        novoColetor.Tipo = TipoUsuario.Coletor;
        Usuarios.Add(novoColetor);

        return Task.FromResult<string?>(null);
    }

    private static string? ValidarDuplicidade(MockUser usuario, bool checarCnpj)
    {
        if (Usuarios.Any(u => u.Username.Equals(usuario.Username, StringComparison.OrdinalIgnoreCase)))
            return "Esse nome de usuario ja esta em uso.";

        if (Usuarios.Any(u => u.Cpf == usuario.Cpf))
            return "Esse CPF ja esta cadastrado.";

        if (Usuarios.Any(u => u.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase)))
            return "Esse e-mail ja esta cadastrado.";

        if (checarCnpj && Usuarios.Any(u => u.Cnpj == usuario.Cnpj))
            return "Esse CNPJ ja esta cadastrado.";

        return null;
    }
}