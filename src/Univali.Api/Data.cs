using Univali.Api.Entities;

namespace Univali.Api
{
    // Classes sem parâmetro, não possui tipos como parâmetro
    public class Data
    {
        public List<Customer> Customers { get; set; }
        // Propriedade privada que contém a única referência a instância
        private static Data? _instance;

        // Método público e estático que fornece acesso a propriedade que possui a instância
        // Neste caso é uma propriedade porque o método get permite executar uma instrução
        public static Data Instance
        {
            get
            {
                /*
                Assume Lazy instanciação como padrão.
                Não é instânciada ao executar a aplicação, é instânciada
                quando necessária e será somente uma única vez.
                 */

                return _instance ??= new Data();
            }
        }
        // Construtor único, privado e sem parâmetro
        private Data()
        {
            Customers = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Linus Torvalds",
                    Cpf = "00123456789",
                    Addresses = new List<Address>()
                   {
                       new Address
                       {
                            Id = 1,
                            Street = "Rua do Linux",
                            City = "Helsinki"
                       },
                       new Address
                       {
                           Id = 2,
                           Street = "Rua da casa de Linus",
                           City = "Helsinki"
                       }
                    }
                },

                new Customer {
                    Id = 2,
                    Name = "Bill Gates",
                    Cpf = "12345678900",
                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            Id = 1,
                            Street = "Rua da Microssoft",
                            City = "Seattle"
                        }
                    }
                }
            };
        }
    }
}