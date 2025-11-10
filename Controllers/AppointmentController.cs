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

        /// <summary>
        /// Retrieves all appointments.
        /// </summary>
        /// <remarks>This method returns a list of all appointments available in the system. The result is
        /// returned as an HTTP 200 OK response containing the list of appointments.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 OK response with a list of appointments.</returns>
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var appointmentsList = _appointmentsService.GetAppointments();
            return Ok(appointmentsList);
        }

        /// <summary>
        /// Creates a new appointment based on the provided appointment data.
        /// </summary>
        /// <remarks>This method checks if the specified workshop is active before creating the
        /// appointment.  If the workshop is not active, the method returns a bad request response.</remarks>
        /// <param name="appointmentDTO">The data transfer object containing the details of the appointment to be created.  Must not be <see
        /// langword="null"/> and must have a valid <c>place_id</c>.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="BadRequestResult"/> if the input data is invalid or the specified workshop is not active.  Returns
        /// <see cref="CreatedAtActionResult"/> if the appointment is successfully created.</returns>
        [HttpPost("create")]
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
            return Ok(new { message = "Turno creado correctamente" });
        }
    }
}
