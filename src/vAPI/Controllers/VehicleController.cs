using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Interfaces;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class VehicleController : Controller
    {
        private readonly IJoinableRepository<VehicleModel> _vehiclesRepository;
        private readonly IConfiguration _configuration;


        public VehicleController(IJoinableRepository<VehicleModel> vehiclesRepository,
            IConfiguration configuration)
        {
            _vehiclesRepository = vehiclesRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
           // int characterId = HttpContext.User.Identities.First(claim => claim.Name == "CharacterId").;
            //IEnumerable<VehicleModel> vehicles = _vehiclesRepository.Get(vehicle => vehicle.CreatorId ==);
            return Json(vehicles);
        }
    }
}
