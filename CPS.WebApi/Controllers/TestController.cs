//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using CPS.Entity.Business;
//using CPS.Repository.IRepository;

//namespace CPS.API.Controllers;
//[Route("api/[controller]/[action]")]
//[ApiController]
//public class TestController(IMediaRepository MediaRepository) : ControllerBase
//{
//    private readonly IMediaRepository _MediaRepository = MediaRepository;

//    [HttpPost]
//    public bool AddPicture()
//    {
//        Media picture = new()
//        {
//            Title = "Test",
            
//            CreateUserId = 1,
//            CreateUserName = "Sivic"
//        };

//        return _MediaRepository.Add(picture);
//    }

//}
