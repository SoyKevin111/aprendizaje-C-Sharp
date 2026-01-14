using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apipeliculas.src.Infraestructure.Dtos;

namespace apipeliculas.src.Domain.interfaces
{
    public interface IImageStoreService
    {
        Task<ImageUploadResultDTO> UploadImageAsync(IFormFile file);
    }
}