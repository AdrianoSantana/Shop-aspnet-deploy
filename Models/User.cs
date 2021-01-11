using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        [MaxLength(20, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]
        [MaxLength(20, ErrorMessage = "Este campo deve ter entre 3 e 60 caracteres")]

        public string Password { get; set; }

        public string Role { get; set; }

    }
}
