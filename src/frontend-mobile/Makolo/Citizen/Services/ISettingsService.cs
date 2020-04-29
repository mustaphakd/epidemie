using Citizen.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Citizen.Services
{
    public interface ISettingsService
    {
        SettingsModel GetSettings();
        Task SetSettings(SettingsModel model);
    }
}
