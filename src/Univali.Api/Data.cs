using Univali.Api.Entities;

namespace Univali.Api
{
    // Classes sem parâmetro, não possui tipos como parâmetro
    public class Data
    {
        public List<Customer> Customers { get; set; }

        public Data()
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
                            Id = 3,
                            Street = "Rua da Microssoft",
                            City = "Seattle"
                        }
                    }
                }
            };
        }
    }
}