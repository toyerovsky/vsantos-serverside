using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Interfaces;

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
    }
}
