using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Service.Base;
using CPS.Repository;
using Microsoft.AspNetCore.Hosting;
using CPS.Entity.Dtos.BusinessDtos;

namespace CPS.Service
{
    public class ImageService(IBaseRepository<Image> repository) : BaseService<Image>(repository), IImageService
    {
        public async Task<bool> DeleteImageAsync(Image image, string webRootPath)
        {
            string filePath = Path.Combine(webRootPath, image.Url);
            System.IO.File.Delete(filePath);
            return await DeleteAsync(image.Id);
        }
    }
}
