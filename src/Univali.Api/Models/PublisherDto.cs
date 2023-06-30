namespace Univali.Api.Models
{
   public class PublisherDto
   {
       public int PublisherId { get; set; }
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string Cpf { get; set; } = string.Empty;
   }
}