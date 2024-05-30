using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
namespace CPS.Entity.Dtos.BusinessDtos
{
    public class ImageDto : BaseEntityDto<Guid>
    {
        /// <summary>
        /// Share的外键  
        /// </summary>
        public Guid? ShareId { get; set; }
        /// <summary>
        /// 前端传入图片
        /// </summary>
        public required IFormFile Image { get; set; }
        /// <summary>
        /// 图片描述文字，非必须
        /// </summary>
        public string? Alt { get; set; }
    }
}