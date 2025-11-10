using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TecnomBoxes.Services;

namespace TecnomBoxes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        private readonly IWorkshopsService _workshopsService;
        public WorkshopController(IWorkshopsService workshopsService)
        {
            _workshopsService = workshopsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveWorkshops()
        {
            var activeWorkshops = await _workshopsService.GetActiveWorkshopsAsync();
            return Ok(activeWorkshops);
        }
    }
}
