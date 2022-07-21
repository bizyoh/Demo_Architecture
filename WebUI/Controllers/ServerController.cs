using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private IServerService serverService;
        //private IOptionsSnapshot<ServerSetting> setting;

        public ServerController(IServerService _serverService)
        {
            serverService = _serverService;
        }

        [HttpGet]
        public IActionResult GetById()
        {
            serverService.ChangeServer();
            return Ok();
        }

    }
}
