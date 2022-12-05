using MessengerWeb.Server.Services;
using MessengerWeb.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiRequestsService _apiRequestsService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _dbContext;

        public HomeController(ILogger<HomeController> logger, 
                              ApiRequestsService apiRequestsService, 
                              IConfiguration configuration,
                              ApplicationContext context)
        {
            _logger = logger;
            _apiRequestsService = apiRequestsService;
            _configuration = configuration;
            _dbContext = context;
        }

        [HttpPost("liveness/{engineId?}")]
        public async Task<IActionResult> PostFrameGetLiveness(string engineId, [FromForm(Name = "data")] IFormFile file)
        {
            if (file is null)
                return StatusCode(400, "No photo, formFile is null");

            using (Stream stream = file.OpenReadStream())
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var bytes = ms.ToArray();

                var fileHashResponse = await _apiRequestsService.GetExternalApiFileHash(bytes);
                var livenessTask = await _apiRequestsService.ProcessTask(_configuration["ApiGates:GetLivenessTask"], 
                                                                            fileHashResponse.Hash, 
                                                                            engineId);
                var livenessTaskResult = await _apiRequestsService.GetTaskResult(livenessTask.TaskId, Operation.Liveness);
                if (livenessTaskResult is LivenessTaskResult)
                {
                    var livenessResult = (LivenessTaskResult)livenessTaskResult;
                    return Ok(livenessResult.Result.Score.ToString());
                }
            }
            return StatusCode(409, "failed");
        }


        [HttpPost("match/{engineId?}")]
        public async Task<IActionResult> PostFrameGetMatch(string engineId, [FromForm(Name = "data")] IFormFile file)
        {
            if (file is null)
                return StatusCode(400, "No photo, formFile is null");

            using (Stream stream = file.OpenReadStream())
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var bytes = ms.ToArray();

                var fileHashResponse = await _apiRequestsService.GetExternalApiFileHash(bytes);
                var matchTask = await _apiRequestsService.ProcessTask(_configuration["ApiGates:GetBestMatchTask"], 
                                                                        fileHashResponse.Hash,
                                                                        engineId);
                var matchTaskResult = await _apiRequestsService.GetTaskResult(matchTask.TaskId, Operation.Match);
                if (matchTaskResult is CommonTaskResult)
                {
                    var matchResult = (CommonTaskResult)matchTaskResult;
                    if (matchResult.Result?.FaceId is not null)
                    {
                        var personMatched = _dbContext.Persons.FirstOrDefault(p => p.UUID == matchResult.Result.FaceId);
                        return Ok(personMatched);
                    }
                }
            }
            return StatusCode(409, "not_identified");
        }

        [HttpPost("register/{engineId?}")]
        public async Task<IActionResult> Post(string engineId, [FromForm(Name = "data")] IFormFile file)
        {
            if (file is null)
                return StatusCode(400, "No photo, formFile is null");

            using (Stream stream = file.OpenReadStream())
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var bytes = ms.ToArray();

                var fileHashResponse = await _apiRequestsService.GetExternalApiFileHash(bytes);
                var registerTaskResponse = await _apiRequestsService.ProcessTask(_configuration["ApiGates:GetRegisterTask"], 
                                                                                    fileHashResponse.Hash,
                                                                                    engineId);
                var registerTaskResult = await _apiRequestsService.GetTaskResult(registerTaskResponse.TaskId, Operation.Register);
                if (registerTaskResult is CommonTaskResult)
                {
                    var registerResult = (CommonTaskResult)registerTaskResult;
                    
                    if (registerResult.Result.FaceId is not null)
                    {
                        if(registerResult.Result.FaceId == String.Empty)
                        {
                            return StatusCode(409, "no_face");
                        }
                        if (_dbContext.Persons.Any(p => p.UUID == registerResult.Result.FaceId))
                        {
                            return StatusCode(409, "exists");
                        }
                        else
                        {
                            return Ok(registerResult.Result.FaceId);
                        }
                    }
                }
            }
            return StatusCode(409, "failed");
        }

        [HttpPost("save_person")]
        public async Task<IActionResult> Post(Person person)
        {
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
