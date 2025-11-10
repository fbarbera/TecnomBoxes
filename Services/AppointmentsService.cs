using Microsoft.Extensions.Caching.Memory;
using TecnomBoxes.Entities;

namespace TecnomBoxes.Services
{
    public interface IAppointmentsService
    {
        void AddAppointment(AppointmentDTO a);
        IEnumerable<AppointmentDTO> GetAppointments();
    }

    public class AppointmentsService : IAppointmentsService
    {
        private readonly List<AppointmentDTO> _appointments = new();
        public void AddAppointment(AppointmentDTO a)
        {
            _appointments.Add(a);
        }
        public IEnumerable<AppointmentDTO> GetAppointments()
        {
            return _appointments.ToArray();
        }
    }
}
