using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vega.Core.Models;
using Vega.Controllers.Resources;
using AutoMapper;
using Vega.Persistence;
using Vega.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vega.Controllers
{
    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        //private readonly VegaDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly IVehicleRepository repository;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            //this.context = context;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateVehicle([FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if (true)
            //{
            //    ModelState.AddModelError("...", "error");
            //    return BadRequest(ModelState);
            //}

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();


            //var model = await context.Models.FindAsync(vehicleResource.ModelId);
            //if (model == null)
            //{
            //    ModelState.AddModelError("ModelId", "Invalid modelId.");
            //    return BadRequest(ModelState);
            //}

            //var vehicle = mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            //vehicle.LastUpdate = DateTime.Now;

            //context.Vehicles.Add(vehicle);
            //await context.SaveChangesAsync();

            //var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);


            //return Ok(vehicle);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await repository.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();
            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id, includeRelated: false);

            if (vehicle == null)
                return NotFound();

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }

        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
            var queryResult = await repository.GetVehicles(filter);

            return mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}
