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
    public string Senha { get; set; } = string.Empty;
    public TipoUsuario Tipo { get; set; }
}

/// <summary>
/// Simula autenticacao e dados de usuario enquanto nao existe backend/API.
/// Substituir por chamadas HTTP reais quando a API estiver pronta.
/// </summary>
public static class MockUserService
{
    private static readonly List<MockUser> Usuarios = new()
    {
        new MockUser { Nome = "Ferro Velho Conceicao", Username = "ferrovelho", Senha = "123456", Tipo = TipoUsuario.Coletor },
        new MockUser { Nome = "Maria Reciclagem", Username = "maria", Senha = "123456", Tipo = TipoUsuario.Reciclador },
        new MockUser { Nome = "EcoParceira LTDA", Username = "ecoparceira", Senha = "123456", Tipo = TipoUsuario.EmpresaParceira },
    };

    public static MockUser? TryLogin(string username, string senha)
    {
        return Usuarios.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
            u.Senha == senha);
    }
}
