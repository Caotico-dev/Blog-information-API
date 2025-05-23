using Blog_informetion_API.DataServices;
using Blog_informetion_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Blog_informetion_API.Controller
{
    [Route("/information")]
    [ApiController]
    public class InformationReceiverController : ControllerBase
    {
        private readonly IBlog_information_SQL _Information_SQL;       
        private readonly ILogger<InformationReceiverController> _logger;

        public InformationReceiverController(IBlog_information_SQL blog_Information_SQL, ILogger<InformationReceiverController> logger)
        {
            this._Information_SQL = blog_Information_SQL;
            this._logger = logger;
        }        

        [Authorize]
        [HttpGet("/information/news")]
        public async Task<List<NewsDto>> GetNews()
        {
            try
            {
                var news = await this._Information_SQL.GetNewsAsync();

                return news;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error al obtener la noticia");
                return new List<NewsDto>();

            }
        }
        [Authorize]
        [HttpGet("/information/newsdate")]
        public async Task<List<NewsDto>> GetNews(DateOnly dateOnly)
        {
            try
            {
                var news = await this._Information_SQL.GetNewsDateAsync(dateOnly);

                return news;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error al obtener la noticia");
                return new List<NewsDto>();

            }
        }
        //[Authorize]
        [HttpGet("/information/news_images={tituloNoticia}")]
        public async Task<IActionResult> GetImages(string tituloNoticia)
        {
            try
            {
                if (!string.IsNullOrEmpty(tituloNoticia))
                {
                    var (imageData, ext) = await this._Information_SQL.GetImageData(tituloNoticia);

                    if (imageData != null)
                    {
                        return File(imageData, $"image/{ext}");
                    }
                }
                return BadRequest("Pon un titulo valido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la imagen");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [Authorize]
        [HttpPost("/information/news")]
        public async Task<IActionResult> PostNews([FromForm] PublishNews newsDto, [Required] IFormFile images)
        {
            try
            {
                if (images != null && images.Length > 0 && newsDto != null)
                {
                    bool succes = await this._Information_SQL.SaveNewsAsync(newsDto, images);
                    if (succes)
                    {
                        var news = await this._Information_SQL.GetNewsAsync();
                        return Created("/information/news", news);
                    }
                    else
                    {
                        return StatusCode(500, "Ocurrió un error al guardar la noticia.");
                    }
                }
                return BadRequest(new { message = "Debe enviar archivos no vacío." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la noticia");
                return StatusCode(500, "Error interno del servidor");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/information/news={titulo},{dateOnly}")]
        public async Task<IActionResult> DeleteNews(string titulo, DateOnly dateOnly)
        {
            try
            {
                if (!string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(dateOnly.ToString()))
                {
                    bool succes = await this._Information_SQL.DeleteNewsAsync(titulo, dateOnly);
                    if (succes)
                    {
                        return NoContent();
                    }
                }
                return BadRequest(new { message = "Solicitud inválida. Por favor, verifica los datos enviados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la noticia");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }

}





