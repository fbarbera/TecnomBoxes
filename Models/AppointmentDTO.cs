using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;
using TecnomBoxes.Models;

namespace TecnomBoxes.Entities
{
    public class AppointmentDTO
    {
        public int place_id { get; set; }
        public DateTime appointment_at { get; set; }
        public string service_type { get; set; }
        public ContactDTO contact { get; set; }
        public VehicleDTO vehicle { get; set; }
        public DateTime created_at { get; set; }

    }
}
