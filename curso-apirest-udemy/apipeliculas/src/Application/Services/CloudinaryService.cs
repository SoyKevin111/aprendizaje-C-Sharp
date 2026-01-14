using apipeliculas.src.Domain.interfaces;
using apipeliculas.src.Infraestructure.Dtos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace apipeliculas.src.Application.Services
{
    public class CloudinaryService : IImageStoreService
    {

        private readonly Cloudinary _cld;
        private readonly ILogger<CloudinaryService> _logger;
        public CloudinaryService(IConfiguration cfg, ILogger<CloudinaryService> logger)
        {
            this._logger = logger;
            var acc = new Account(
                cfg.GetValue<string>("Cloudinary:CloudName"),
                cfg.GetValue<string>("Cloudinary:ApiKey"),
                cfg.GetValue<string>("Cloudinary:ApiSecret")
            );

            _cld = new Cloudinary(acc);
        }

        public async Task<ImageUploadResultDTO> UploadImageAsync(IFormFile file)
        {
            if (_cld == null)
            {
                _logger.LogError("Cloudinary instance is null");
                throw new InvalidOperationException("Cloudinary no está inicializado");
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Archivo invalido");
            }

            //validacion tipos
            var allowedTypes = new List<string> { "image/jpeg", "image/png", "image/webp" };
            string fileType = file.ContentType;
            if (!allowedTypes.Contains(fileType))
            {
                _logger.LogInformation("file-type: {fileType}", fileType);
                throw new ArgumentException("Formato no permitido");
            }

            //using cierra el flujo automaticamente
            using var stream = file.OpenReadStream(); //flujo de bytes y lo sube a cloudinary por chunks
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                //Folder = "",
                Transformation = new Transformation().Width(800).Height(800).Crop("limit") //limita imagen sin recortar.
            };

            var result = await _cld.UploadAsync(uploadParams);
            return new ImageUploadResultDTO
            {
                Url = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }
    }
}