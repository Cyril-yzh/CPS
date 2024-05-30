using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CPS.Entity.Business;
using CPS.Repository.BusRepository;

namespace CPS.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IPictureRepository _pictureRepository;

    public TestController(IPictureRepository pictureRepository)
    {
        this._pictureRepository = pictureRepository;
    }

    [HttpPost]
    public bool AddPicture()
    {
        Picture picture = new()
        {
            Title = "Test",

            CreateUserId = 1,
            CreateUserName = "Sivic"
        };

        return _pictureRepository.Add(picture);
    }

}
