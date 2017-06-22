using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vega.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vega.Controllers
{
    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        [HttpPost("")]
        public IActionResult CreateVehicle([FromBody]Vehicle vehicle)
        {
            return Ok(vehicle);
        }
    }
}
