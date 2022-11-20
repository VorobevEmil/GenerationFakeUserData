using CsvHelper;
using GenerationFakeUserData.Server.Services;
using GenerationFakeUserData.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace GenerationFakeUserData.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerationController : ControllerBase
    {
        private readonly GenerationService _service;
        public GenerationController(GenerationService service)
        {
            _service = service;
        }

        [HttpPost("ReceiveFakeUserData")]
        public ActionResult<List<UserInfo>> ReceiveFakeUserData(ConfigureGenerationRequest configure)
        {
            try
            {
                if (configure.Region == default || configure.Region == string.Empty)
                    return BadRequest("Ошибка генерации, ввелите регион");

                return Ok(_service.ReceiveFakeUserData(configure));
            }
            catch (DirectoryNotFoundException)
            {
                return BadRequest("Не найден список для генерации данных");
            }
            catch (Exception)
            {
                return BadRequest("Произошла ошибка во время генерации данных");
            }
        }


        [HttpPost("SaveGenerationUserData")]
        public IActionResult SaveGenerationUserData(List<UserInfo> users)
        {
            if (users == null || users.Count == default)
                return BadRequest("Пожалуйста прикрепите файл");

            try
            {
                return File(_service.SaveGenerationUserData(users), "text/csv");
            }
            catch (Exception)
            {
                return BadRequest("Не удалось обработать файл");
            }
        }
    }
}
