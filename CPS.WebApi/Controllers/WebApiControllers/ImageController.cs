using Microsoft.AspNetCore.Mvc;
using CPS.Entity;
using CPS.Entity.Business;
using CPS.Service.IServices;
using CPS.Entity.Dtos.BusinessDtos;
using Microsoft.IdentityModel.Tokens;
using CPS.Service;
using Microsoft.AspNetCore.Http;
//using static System.Net.Mime.MediaTypeNames;
using Image = CPS.Entity.Business.Image;
using System;
using System.IO;

namespace CPS.API.Controllers.WebApiControllers;
/// <summary>
/// 文章API接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ImageController(IImageService imageService, IWebHostEnvironment webHostEnvironment) : ControllerBase
{
    private readonly IImageService _imageService = imageService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    [HttpPost]
    public async Task<IActionResult> UploadImage(ImageDto imageDto)
    {
        ApiResult<Image> result = new();

        if (imageDto.Image == null)  // 文件为空
        {
            result.Code = -1;
            result.Data = null;
            result.Message = "传入图片为空";
            return Ok(result);
        }
        try
        {
            // 生成文件名
            string fileExtension = Path.GetExtension(imageDto.Image.FileName);
            string fileName = Path.Combine("images", $"{Guid.NewGuid()}{fileExtension}");

            // 保存文件路径
            string saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

            // 保存文件
            using FileStream stream = new(saveFilePath, FileMode.Create);
            await imageDto.Image.CopyToAsync(stream);

            // 执行图片处理逻辑
            Image image = new()
            {
                Url = fileName,
                Alt = imageDto.Alt,
                ShareId = imageDto.ShareId,
            };
            bool res = await _imageService.AddAsync(image);

            result.Code = res ? 0 : -1;
            result.Data = image;
            result.Message = res ? "图片上传成功！" : "图片上传出现异常！";

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
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        ApiResult<bool> result = new();

        try
        {
            Image? image = await _imageService.FindAsync(id);
            if (image != null)
            {
                bool res = await _imageService.DeleteImageAsync(image, _webHostEnvironment.WebRootPath);

                result.Code = res ? 0 : -1;
                result.Data = res;
                result.Message = res ? "图片删除成功！" : "图片删除出现异常！";
                return Ok(result);
            }
            else
            {
                result.Code = -1;
                result.Data = false;
                result.Message = "找不到指定的图片！";
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            result.Code = -1;
            result.Data = false;
            result.Message = ex.Message;
            return Ok(result);
        }
    }
}
