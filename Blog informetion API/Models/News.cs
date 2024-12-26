using System;
using System.Collections.Generic;

namespace Blog_informetion_API.Models;

public partial class News
{
    public int Id { get; set; }

    public string? Titulo { get; set; }

    public string? Autor { get; set; }

    public DateOnly? FechaDePublicacion { get; set; }   
    public string? Cuerpo { get; set; }

    public string? Url_images { get; set; }
   
}
