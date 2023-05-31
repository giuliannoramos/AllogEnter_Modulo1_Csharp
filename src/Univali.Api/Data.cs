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
        /// Propriedade que armazena a lista de enderecos.
        /// </summary>
        public List<Address> Addresses { get; set; }

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

            // Inicializa a lista de Address com alguns exemplos de enderecos.
            Addresses = new List<Address>
            {
                new Address
                {
                    Id = 1,
                    Street = "Rua Teste 1",
                    City = "Cidade Teste 1",
                    State = "Estado Teste 1",
                    CustomerId = 1 // Linus Torvalds
                },
                new Address
                {
                    Id = 2,
                    Street = "Rua Teste 2",
                    City = "Cidade Teste 2",
                    State = "Estado Teste 2",
                    CustomerId = 2 // Bill Gates
                },
                new Address
                {
                    Id = 3,
                   Street = "Rua Teste 3",
                    City = "Cidade Teste 3",
                    State = "Estado Teste 3",
                    CustomerId = 2 // Bill Gates
                }

            };
        }

        public int GenerateCustomerId()
        {
            return Customers.Max(c => c.Id) + 1;
        }

        /// <summary>
        /// Método para adicionar um novo cliente à lista de Customers usando um DTO.
        /// </summary>
        /// <param name="customerDto">DTO do cliente a ser adicionado.</param>
        public void AddCustomer(Customer customer)
        {
            Customer customera = new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Cpf = customer.Cpf
            };

            Customers.Add(customera);
        }

        /// <summary>
        /// Método para adicionar um novo endereço à lista de Addresses usando um DTO.
        /// </summary>
        /// <param name="addressDto">DTO do endereço a ser adicionado.</param>
        public void AddAddress(AddressForCreationDto addressForCreationDto)
        {
            Address address = new Address
            {
                Id = GenerateAddressId(),
                CustomerId = addressForCreationDto.CustomerId,
                Street = addressForCreationDto.Street,
                City = addressForCreationDto.City,
                State = addressForCreationDto.State
            };

            Addresses.Add(address);
        }



        private int GenerateAddressId()
        {
            return Addresses.Max(a => a.Id) + 1;
        }
    }
}