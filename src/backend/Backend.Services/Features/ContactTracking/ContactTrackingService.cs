using Backend.Repository;
using Backend.Services.Features.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Services.Features.ContactTracking
{
    public class ContactTrackingService: BaseService<ContactTrackingService>
    {
        public ContactTrackingService(CommonCitizenRepository commonCitizenRepository, ILogger<ContactTrackingService> logger)
            : base(logger)
        {

        }
    }
}
