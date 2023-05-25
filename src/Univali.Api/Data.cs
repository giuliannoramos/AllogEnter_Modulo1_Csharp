using Univali.Api.Entities;
using System.Collections.Generic;

namespace Univali.Api
{
    public class Data
    {
        // Propriedade que armazena a lista de clientes
        public List<Customer> Customers { get; set; }

        // Variável estática que armazena a única instância da classe Data
        private static Data? _instance;

        // Propriedade estática Instance que representa a única instância da classe Data
        // acessível globalmente usando o padrão Singleton.
        public static Data Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Se _instance for nulo, cria uma nova instância de Data e atribui à variável _instance.
                    _instance = new Data();
                }
                // Retorna a instância existente ou a nova instância criada.
                return _instance;
            }
        }

        // Construtor privado para evitar que a classe seja instanciada externamente.
        private Data()
        {
            // Inicializa a lista de Customers com alguns exemplos de clientes.
            Customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Linus",
                    Cpf = "123456789"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Bill",
                    Cpf = "987654321"
                }
            };
        }
    }
}
