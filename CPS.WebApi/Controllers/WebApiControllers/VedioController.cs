using Microsoft.AspNetCore.Mvc;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Entity.Dtos.BusinessDtos;
using Microsoft.IdentityModel.Tokens;
using CPS.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CPS.API.Controllers.WebApiControllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(IImageService imageService, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        private readonly IImageService _imageService = imageService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpPost]
        public async Task<IActionResult> UploadVideo(VideoDto vedioDto)
        {
            ApiResult<Image> result = new();

            if (vedioDto == null)  // 文件为空
            {
                result.Code = -1;
                result.Data = null;
                result.Message = "传入视频为空";
                return Ok(result);
            }
            try
            {
                // 生成文件名
                string fileExtension = Path.GetExtension(vedioDto.Vedio.FileName);
                string fileName = Path.Combine("videos", $"{Guid.NewGuid()}{fileExtension}");

                // 保存文件路径
                string saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

                // 保存文件
                using FileStream stream = new(saveFilePath, FileMode.Create);
                await vedioDto.Vedio.CopyToAsync(stream);

                Image image = new()
                {
                    Url = fileName,
                    ShareId = vedioDto.ShareId,
                };
                bool res = await _imageService.AddAsync(image);

                result.Code = res ? 0 : -1;
                result.Data = image;
                result.Message = res ? "视频上传成功！" : "视频上传出现异常！";

                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Data = null;
                result.Message = ex.Message;
                return Ok(result);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVideo(Guid id)
        {
            ApiResult<Image> result = new();

            try
            {
                Image? image = await _imageService.FindAsync(id);
                if (image != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, image.Url);
                    // 删除文件
                    System.IO.File.Delete(filePath);
                    // 执行删除图片逻辑
                    bool res = await _imageService.DeleteAsync(id);

                    result.Code = res ? 0 : -1;
                    result.Data = image;
                    result.Message = res ? "视频删除成功！" : "视频删除出现异常！";
                    return Ok(result);
                }
                else
                {
                    result.Code = -1;
                    result.Data = null;
                    result.Message = "找不到指定的视频！";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Data = null;
                result.Message = ex.Message;
                return Ok(result);
            }

        }
    }
}
