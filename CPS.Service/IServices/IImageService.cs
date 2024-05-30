using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS.Service.IServices
{
    public interface IImageService : IBaseService<Image>
    {
        public Task<bool> DeleteImageAsync(Image image, string webRootPath);
    }
}
