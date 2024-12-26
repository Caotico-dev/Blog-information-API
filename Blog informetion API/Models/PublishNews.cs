using System.ComponentModel.DataAnnotations;

namespace Blog_informetion_API.Models
{
    public class PublishNews
    {
        [Required]
        [MaxLength(200)]
        public string? Titulo { get; set; }
        [Required]
        public string? Autor { get; set; }
        [Required]
        public DateOnly? FechaDePublicacion { get; set; }
        [Required]
        public string? Cuerpo { get; set; }
    }
}
