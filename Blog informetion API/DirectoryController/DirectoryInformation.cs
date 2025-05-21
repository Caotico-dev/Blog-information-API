
namespace Blog_informetion_API.DirectoryController
{

    public interface IDirectoryController
    {
        /// <summary>
        /// save an image to the directoy.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="imagen"></param>
        /// <returns>path file.</returns>
        Task<string> SaveImages(string titulo, IFormFile imagen);
        /// <summary>
        /// gets file images in the path spacified.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A array bytes.</returns>
        Task<byte[]> GetFileImages(string path);
        /// <summary>
        /// Gets extension in a path spacified.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A file extension.</returns>
        Task<string> GetExtension(string path);
        /// <summary>
        /// Delete a file in a path specified.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>returns true if deleted otherwise false.</returns>
        Task<bool> DeleteImages(string path);


    }
    public class DirectoryInformation : IDirectoryController
    {
        /// <summary>
        /// Default path
        /// </summary>
        private const string path = "..\\Blog informetion API\\SupDirectory\\DirectoryLow\\";
        
        public DirectoryInformation(){          
        }
        /// <summary>
        /// create a directory if it does not exist
        /// </summary>
        private void CreateDirectory()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// Enter directory number
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Int</returns>
        private int NumberDirectoy(string path)
        {
            int number = 0;
            var numberDirectory = Directory.GetDirectories(path).Length;
            return number = numberDirectory + 1;
        }

        /// <summary>
        /// Removes a directory if the directory was deleted succesfully returns true otherwise false.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Bool</returns>
        public async Task<bool> DeleteImages(string path)
        {
            if (Directory.Exists(path))
            {
                await Task.Run(() => Directory.Delete(path, recursive: true));

                return true;
            }
            return false;
        }
        /// <summary>
        /// get the image in a byte string if the image not exist return empty byte
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Byte[]</returns>
        public async Task<byte[]> GetFileImages(string path)
        {
            byte[] image = null;

            if (Directory.Exists(path))
            {
                var files = await Task.Run(() => Directory.GetFiles(path));

                var img = files.FirstOrDefault(f => Path.GetExtension(f).Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                Path.GetExtension(f).Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                Path.GetExtension(f).Equals(".png", StringComparison.OrdinalIgnoreCase));


                if (img != null)
                {
                    await using (FileStream strem = new FileStream(img, FileMode.Open, FileAccess.Read, FileShare.Read, 4097, true))
                    {
                        byte[] byteimg = new byte[strem.Length];
                        await strem.ReadAsync(byteimg, 0, (int)strem.Length);
                        return byteimg;
                    }
                }
            }
            return image;

        }
        /// <summary>
        /// Gets extension in a path spacified.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A file extension.</returns>
        public async Task<string> GetExtension(string path)
        {

            if (Directory.Exists(path))
            {
                var files = await Task.Run(() => Directory.GetFiles(path));
                var img = files.FirstOrDefault(f => Path.GetExtension(f).Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                Path.GetExtension(f).Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                Path.GetExtension(f).Equals(".png", StringComparison.OrdinalIgnoreCase));

                var imgext = Path.GetExtension(img);


                if (imgext != null)
                {
                    return imgext.TrimStart('.');
                }
                return "";

            }
            return "";

        }

        /// <summary>
        /// save to directory if saved successfully returns true otherwise false.
        /// </summary>       
        /// <param name="imagen"></param>
        /// <returns>string</returns>
        public async Task<string> SaveImages(string titulo, IFormFile imagen)
        {

            CreateDirectory();

            if (!string.IsNullOrWhiteSpace(titulo) && imagen.Length != 0)
            {
                var numberDirectory = NumberDirectoy(path);
                string file = Path.Combine(path, numberDirectory + ".-" + titulo);
                Directory.CreateDirectory(file);


                var fileimg = Path.Combine(file, imagen.FileName);
                using (var stream = new FileStream(fileimg, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                if (Directory.Exists(file))
                {
                    return file;
                }
                return "";

            }
            return "";

        }

    }
}
