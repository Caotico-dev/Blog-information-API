using System.ComponentModel.DataAnnotations;

namespace Blog_informetion_API.Models
{
    public class NewsDto
    {
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Autor { get; set; }
        [Required]
        public DateOnly? FechaDePublicacion { get; set; }
        [Required]
        public string? Cuerpo { get; set; }
        [Required]
        public string? Url_images { get; set; } 

       
    }
}
