
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caacteres")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caacteres")]
        public string Title { get; set; }
    }
}
