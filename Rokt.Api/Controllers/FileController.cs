using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rokt.Application.Interfaces;
using System;
using System.Collections.Generic;

namespace Rokt.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IDataService _dataService;

        public FileController(ILogger<FileController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        [HttpPost]
        public IActionResult Post()
        {
            var result = _dataService.ExtraData(@"C:\RAVEEN\EXAMS\Rokt\Files\backend-technical-test\sample3.txt", DateTime.Parse("2004-10-02T04:35:22Z"), DateTime.Parse("2004-10-03T03:32:09Z"));

            var response = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            return Ok(response);
        }
    }
}
