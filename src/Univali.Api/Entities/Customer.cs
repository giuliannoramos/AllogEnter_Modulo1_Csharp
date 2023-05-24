namespace Univali.Api.Entities;
public class Customer
{
    public int Id {get; set;}

    // Uma opção melhor que string? para possivel valor nulo, ela declara string vazia
    public string Name {get; set;} = string.Empty;
    public String Cpf {get; set;} = string.Empty;
}