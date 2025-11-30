using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinaryClinic.UI.Models.Appointments;

namespace VeterinaryClinic.UI.Services;

public interface IAppointmentApiClient
{
    Task<IReadOnlyList<AppointmentListItemViewModel>> GetAppointmentsAsync();
    Task CreateAppointmentAsync(AppointmentCreateViewModel model);

}
