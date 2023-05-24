using Univali.Api.Entities;

namespace Univali.Api
{
    //classe sem parametro, nao possui tipos como parametro
    public class Data
    {
        public List<Customer> Customers {get; set;}
        //propriedade privada que contem a unica referencia a instancia              
        private static Data? _instance;

        

        public static Data Instance
        {
            get
            {               
                return _instance?? = new Data();
            }
        }

        private Data()
        {
            new List<Customer>
            {
                new Customer{
                    Id = 1,
                    Name = "Linus",
                    Cpf = "123456789"
                },
                new Customer{
                    Id = 2,
                    Name = "Bill",
                    Cpf = "987654321"
                }
            };
        }

    }
}