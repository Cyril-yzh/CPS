using CPS.Entity.Business;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.AspNetCore.Mvc;
namespace CPS.Entity.Dtos.BusinessDtos
{
    public class VideoDto : BaseEntityDto<Guid>
    {
        /// <summary>
        /// Share的外键  
        /// </summary>
        public Guid? ShareId { get; set; }
        /// <summary>
        /// 前端传入图片
        /// </summary>
        public required IFormFile Vedio { get; set; }
        /// <summary>
        /// 视频封面图片 Url ，可选
        /// </summary>
        public string? Poster { get; set; }
    }
   
   
}