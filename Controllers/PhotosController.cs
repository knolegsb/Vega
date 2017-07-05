using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Vega.Core;
using AutoMapper;
using Microsoft.Extensions.Options;
using Vega.Core.Models;
using Vega.Controllers.Resources;
using Microsoft.AspNetCore.Http;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vega.Controllers
{
    [Route("api/[controller]/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly int MAX_BYTES = 1 * 1024 * 1024;

        private readonly IPhotoService photoService;
        private readonly PhotoSettings photoSettings;
        private readonly IMapper mapper;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IPhotoRepository photoRepository;
        private readonly IHostingEnvironment host;

        public PhotosController(IHostingEnvironment host, IVehicleRepository vehicleRepository, IPhotoRepository photoRepository, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoService photoService)
        {
            this.photoService = photoService;
            this.photoSettings = options.Value;
            this.mapper = mapper;
            this.vehicleRepository = vehicleRepository;
            this.photoRepository = photoRepository;
            this.host = host;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await photoRepository.GetPhotos(vehicleId);
            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await vehicleRepository.GetVehicle(vehicleId, includeRelated: false);
            if (vehicle == null)
                return NotFound();

            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if (!photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            var photo = await photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
