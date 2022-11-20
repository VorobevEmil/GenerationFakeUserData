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
            catch (Exception e)
            {
                return BadRequest("");
            }
        }


        [HttpPost("SaveGenerationUserData")]
        public IActionResult SaveGenerationUserData(List<UserInfo> users)
        {
            if (users == null || users.Count == default)
                return BadRequest("Пожалуйста прикрепите файл");

            byte[] file;
            try
            {
                using (var writer = new StringWriter())
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(users);
                    }
                    var stringFile = writer.ToString();
                    file = Encoding.UTF8.GetBytes(stringFile);
                }
            }
            catch (Exception)
            {
                return BadRequest("Не удалось обработать файл");
            }
            return File(file, "text/csv");
        }
    }
}
