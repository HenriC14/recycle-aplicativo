namespace recycleAPP.Services;

public enum TipoUsuario
{
    Reciclador,
    Coletor,
    EmpresaParceira
}

public class MaterialPercentual
{
    public string Nome { get; set; } = string.Empty;
    public double Percentual { get; set; }
    public double PesoKg { get; set; }
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

    // Campos especificos do perfil do Coletor (mock, ate ter backend de verdade)
    public string Endereco { get; set; } = string.Empty;
    public int Seguidores { get; set; }
    public int Seguindo { get; set; }
    public double QuilosReciclados { get; set; }
    public string MaterialMaisReciclado { get; set; } = string.Empty;
    public double MaterialMaisRecicladoPercentual { get; set; }
    public double UltimaAtividadePesoKg { get; set; }
    public string UltimaAtividadeMaterial { get; set; } = string.Empty;
    public double UltimaAtividadeKm { get; set; }
    public string UltimaAtividadeQuando { get; set; } = string.Empty;
    public int SaldoPontos { get; set; }
    public List<MaterialPercentual> Composicao { get; set; } = new();

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
        //coletor
        new MockUser
        {
            Nome = "Ferro Velho Conceicao",
            Username = "ferrovelho",
            Cpf = "00000000000",
            Cnpj = "11222333000181",
            Email = "ferrovelho@teste.com",
            Senha = "123456",
            Tipo = TipoUsuario.Coletor,
            Endereco = "Av. Conceicao, 4567, Vila Maria, Sao Paulo, SP",
            Seguidores = 5,
            Seguindo = 10,
            QuilosReciclados = 20,
            MaterialMaisReciclado = "Plastico",
            MaterialMaisRecicladoPercentual = 70,
            UltimaAtividadePesoKg = 20,
            UltimaAtividadeMaterial = "Plastico",
            UltimaAtividadeKm = 1.2,
            UltimaAtividadeQuando = "Ontem",
            Composicao = new List<MaterialPercentual>
            {
                new() { Nome = "Plastico", Percentual = 70, PesoKg = 14 },
                new() { Nome = "Metal",    Percentual = 12, PesoKg = 2.4 },
                new() { Nome = "Vidro",    Percentual = 3,  PesoKg = 0.6 },
                new() { Nome = "Papel",    Percentual = 0,  PesoKg = 0 },
            }
        },
        //reciclador
        new MockUser
    {
        Nome = "Maria Reciclagem",
        Username = "maria",
        Cpf = "11111111111",
        Email = "maria@teste.com",
        Senha = "123456",
        Tipo = TipoUsuario.Reciclador,
        Endereco = "Rua das Flores, 120, Jardim Sao Paulo, Sao Paulo, SP",
        Seguidores = 12,
        Seguindo = 8,
        QuilosReciclados = 35,
        MaterialMaisReciclado = "Papel",
        MaterialMaisRecicladoPercentual = 55,
        UltimaAtividadePesoKg = 3.5,
        UltimaAtividadeMaterial = "Papel",
        UltimaAtividadeKm = 0.8,
        UltimaAtividadeQuando = "Hoje",
        SaldoPontos = 1000,
        Composicao = new List<MaterialPercentual>
        {
            new() { Nome = "Papel",    Percentual = 55, PesoKg = 19.25 },
            new() { Nome = "Plastico", Percentual = 30, PesoKg = 10.5 },
            new() { Nome = "Vidro",    Percentual = 10, PesoKg = 3.5 },
            new() { Nome = "Metal",    Percentual = 5,  PesoKg = 1.75 },


        }
    },
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