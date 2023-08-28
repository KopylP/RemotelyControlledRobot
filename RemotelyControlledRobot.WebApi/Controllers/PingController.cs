using System;
using Microsoft.AspNetCore.Mvc;

namespace RemotelyControlledRobot.WebApi.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class PingController : Controller
	{
		[HttpGet]
		public IActionResult Ping() => Ok("200 OK");
	}
}

