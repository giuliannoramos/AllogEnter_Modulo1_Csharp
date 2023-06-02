using System.ComponentModel.DataAnnotations;

namespace Univali.Api.Dtos;

public class CustomerCreateDto
{   
    [Required(ErrorMessage = "You should fill out a Name")]
    [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "You should fill out a Cpf")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "The cpf should have 11 characters")]
    public string Cpf { get; set; } = string.Empty;
}