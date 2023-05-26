using Univali.Api.Entities;
using System.Collections.Generic;

namespace Univali.Api
{
    /// <summary>
    /// Classe responsável por armazenar os dados da aplicação.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Propriedade que armazena a lista de clientes.
        /// </summary>
        public List<Customer> Customers { get; set; }

        /// <summary>
        /// Variável estática que armazena a única instância da classe Data.
        /// </summary>
        private static Data? _instance;

        /// <summary>
        /// Propriedade estática Instance que representa a única instância da classe Data,
        /// acessível globalmente usando o padrão Singleton.
        /// </summary>
        public static Data Instance
        {
            get
            {
                // if (_instance == null)
                // {
                //     // Se _instance for nulo, cria uma nova instância de Data e atribui à variável _instance.
                //     _instance = new Data();
                // }
                // Retorna a instância existente ou a nova instância criada.
                return _instance ??= new Data();
            }
        }

        /// <summary>
        /// Construtor privado para evitar que a classe seja instanciada externamente.
        /// </summary>
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