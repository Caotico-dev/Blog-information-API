using Blog_informetion_API.DirectoryController;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Blog_informetion_API.Models
{
    public interface IBlog_information_SQL
    {
        /// <summary>
        /// Get new news.
        /// </summary>
        /// <returns>List news.</returns>
        Task<List<NewsDto>> GetNewsAsync();

        Task<List<NewsDto>> GetNewsDateAsync(DateOnly dateOnly);

        /// <summary>
        /// Save a news
        /// </summary>
        /// <param name="newsDto"></param>
        /// <param name="images"></param>
        /// <returns>Return true if the news was saved correctly, otherwise false.</returns>
        Task<bool> SaveNewsAsync(PublishNews news, IFormFile images);
        /// <summary>
        /// Delete a news item 
        /// </summary>
        /// <param name="news"></param>
        /// <returns>Return true if the news was deleted correctly, otherwisa false.</returns>
        Task<bool> DeleteNewsAsync(string titulo, DateOnly dateOnly);
        /// <summary>
        /// Gets images in bytes and their extension.
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns>byte[] imageData, string extension.</returns>
        Task<(byte[] imageData, string extension)> GetImageData(string titulo);
    }

    public class Blog_Information_API_SQL : IBlog_information_SQL
    {
        private readonly BlogInformationApiContext _Db;
        IDirectoryController _DirectoryController;

        public Blog_Information_API_SQL(BlogInformationApiContext db, IDirectoryController directoryController)
        {
            this._Db = db;
            this._DirectoryController = directoryController;
        }
        /// <summary>
        /// Delete a news item 
        /// </summary>
        /// <param name="news"></param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteNewsAsync(string titulo, DateOnly dateOnly)
        {

            var News = await this._Db.News.FirstOrDefaultAsync(f => f.Titulo!.ToLower().Trim() == titulo.ToLower().Trim() && f.FechaDePublicacion == dateOnly);

            if (News != null)
            {
                bool deletedirectory = await this._DirectoryController.DeleteImages(News.Url_images!);
                if (deletedirectory)
                {
                    this._Db.News.Remove(News);

                    await _Db.SaveChangesAsync();
                    return true;
                }
            }

            return false;

        }
        /// <summary>
        /// Get new news
        /// </summary>
        /// <returns>List<NewsDto></News></returns>
        public async Task<List<NewsDto>> GetNewsAsync()
        {

            var newscontent = new List<NewsDto>();
            var today = DateOnly.FromDateTime(DateTime.Today);

            var newsList = await this._Db.News.Where(f => f.FechaDePublicacion == today).ToListAsync();


            foreach (var item in newsList)
            {
                var News = new NewsDto()
                {
                    Titulo = item.Titulo,
                    Autor = item.Autor,
                    FechaDePublicacion = item.FechaDePublicacion,
                    Cuerpo = Regex.Replace(item.Cuerpo!, @"\.\s+", ".\n"),
                    Url_images = $"https://localhost:7186/information/news_images={Uri.EscapeDataString(item.Titulo!)}"
                };

                newscontent.Add(News);

            }

            return newscontent;


        }
        /// <summary>
        /// Gets images in bytes and their extension.
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns>byte[] imageData, string extension</returns>
        public async Task<(byte[] imageData, string extension)> GetImageData(string titulo)
        {
            if (!string.IsNullOrEmpty(titulo))
            {

                var images = this._Db.News.FirstOrDefault(t => t.Titulo!.ToLower().Trim() == titulo.ToLower().Trim())?.Url_images;

                if (!string.IsNullOrEmpty(images))
                {

                    var imageData = await this._DirectoryController.GetFileImages(images);
                    var ext = await this._DirectoryController.GetExtension(images);

                    if (imageData != null && !string.IsNullOrEmpty(ext))
                    {
                        return (imageData, ext);
                    }
                }
            }

            return (Array.Empty<byte>(), string.Empty);

        }

        /// <summary>
        /// Save a news
        /// </summary>
        /// <param name="newsDto"></param>
        /// <param name="images"></param>
        /// <returns>Return true if the news was saved correctly, otherwise false</returns>
        public async Task<bool> SaveNewsAsync(PublishNews newsDto, IFormFile images)
        {

            if (newsDto != null)
            {
                var news = new News
                {
                    Titulo = newsDto.Titulo,
                    Autor = newsDto.Autor,
                    FechaDePublicacion = newsDto.FechaDePublicacion,
                    Cuerpo = newsDto.Cuerpo

                };

                var url = await this._DirectoryController.SaveImages(newsDto.Titulo!, images);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    news.Url_images = url;
                }
                else
                {
                    return false;
                }

                var Confirm = await this._Db.News.AddAsync(news);
                var SaveResult = await _Db.SaveChangesAsync();
                if (SaveResult > 0)
                {
                    return true;
                }
            }
            return false;

        }

        public async Task<List<NewsDto>> GetNewsDateAsync(DateOnly dateOnly)
        {
            var newscontent = new List<NewsDto>();
            var today = dateOnly;

            var newsList = await this._Db.News.Where(f => f.FechaDePublicacion == today).ToListAsync();


            foreach (var item in newsList)
            {
                var News = new NewsDto()
                {
                    Titulo = item.Titulo,
                    Autor = item.Autor,
                    FechaDePublicacion = item.FechaDePublicacion,
                    Cuerpo = Regex.Replace(item.Cuerpo!, @"\.\s+", ".\n"),
                    Url_images = $"https://localhost:7186/information/news_images={Uri.EscapeDataString(item.Titulo!)}"
                };

                newscontent.Add(News);

            }

            return newscontent;
        }
    }
}
