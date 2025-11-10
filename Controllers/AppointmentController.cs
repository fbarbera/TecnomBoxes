using Microsoft.AspNetCore.Mvc;
using TecnomBoxes.Entities;
using TecnomBoxes.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TecnomBoxes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentsService;
        private readonly IWorkshopsService _workshopsService;
        public AppointmentController(IAppointmentsService appointmentsService, IWorkshopsService workshopsService)
        {
            _appointmentsService = appointmentsService;
            _workshopsService = workshopsService;
        }

        // GET: api/<AppointmentController>
        [HttpGet]
        public IActionResult GetAll()
        {
            var appointmentsList = _appointmentsService.GetAppointments();
            return Ok(appointmentsList);
        }

        // GET api/<AppointmentController>/5
        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var appointment = _appointmentsService.GetAppointmentById(id);
        //    return Ok(appointment);
        //}

        // POST api/<AppointmentController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentDTO appointmentDTO)
        {
            if (appointmentDTO == null)
            {
                return BadRequest("Appointment data is required.");
            }

            if (appointmentDTO.place_id <= 0)
            {
                return BadRequest("Invalid place_id.");
            }

            bool isActive = await _workshopsService.IsActiveWorkshopAsync(appointmentDTO.place_id);
            if (!isActive)
            {
                return BadRequest("The specified workshop is not active.");
            }

            var created = new AppointmentDTO
            {
                place_id = appointmentDTO.place_id,
                appointment_at = appointmentDTO.appointment_at,
                service_type = appointmentDTO.service_type,
                contact = appointmentDTO.contact,
                vehicle = appointmentDTO.vehicle,
                created_at = DateTime.UtcNow
            };
            _appointmentsService.AddAppointment(appointmentDTO);
            return CreatedAtAction(nameof(GetAll), new { id = appointmentDTO.place_id }, appointmentDTO);
        }

        //// PUT api/<AppointmentController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AppointmentController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
