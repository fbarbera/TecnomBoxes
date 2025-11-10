using System.ComponentModel.DataAnnotations;

namespace TecnomBoxes.Models
{
    public class AppointmentCreateDTO
    {
        [Required]
        public int Place_id { get; set; }

        [Required]
        public DateTime? Appointment_at { get; set; }

        [Required]
        public string Service_type { get; set; }

        [Required]
        public int Contact_id { get; set; }
        public int Vehicle_id { get; set; }
    }
}
