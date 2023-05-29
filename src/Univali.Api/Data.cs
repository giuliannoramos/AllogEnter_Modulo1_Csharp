using Univali.Api.Entities;
using System.Collections.Generic;
using Univali.Api.Dtos;

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
                if (_instance == null)
                {
                    // Se _instance for nulo, cria uma nova instância de Data e atribui à variável _instance.
                    _instance = new Data();
                }
                return _instance;
                // Retorna a instância existente ou a nova instância criada.
                // return _instance ??= new Data();
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
                    Name = "Linus Torvalds",
                    Cpf = "123456789"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Bill Gates",
                    Cpf = "987654321"
                }
            };
        }

        /// <summary>
        /// Método para adicionar um novo cliente à lista de Customers usando um DTO.
        /// </summary>
        /// <param name="customerDto">DTO do cliente a ser adicionado.</param>
        public void AddCustomer(CustomerDto customerDto)
        {
            Customer customer = new Customer
            {
                Id = GenerateId(),
                Name = customerDto.Name,
                Cpf = customerDto.Cpf
            };

            Customers.Add(customer);
        }

        private int GenerateId()
        {
            return Customers.Max(c => c.Id) + 1;
        }
    }
}