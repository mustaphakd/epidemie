using Backend.DataTransferObjects;
using Backend.Helpers;
using Backend.Repository;
using Backend.Services.Features.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Services.Features.ContactTracking
{
    /// <summary>
    ///
    /// </summary>
    public class ContactTrackingService: BaseService<ContactTrackingService>
    {
        private CommonCitizenRepository commonCitizenRepository_;

        /// <summary>
        ///
        /// </summary>
        /// <param name="commonCitizenRepository"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public ContactTrackingService(CommonCitizenRepository commonCitizenRepository, IHttpContextAccessor httpContextAccessor, ILogger<ContactTrackingService> logger)
            : base(httpContextAccessor, logger)
        {
            Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Entrance, $"commonCitizenRepository model: {commonCitizenRepository}");
            Check.NotNull(commonCitizenRepository, nameof(commonCitizenRepository));
            commonCitizenRepository_ = commonCitizenRepository;
            Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Exit);
        }

        /// <summary>
        /// Allows Users to track contacts
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        public Task<IEnumerable<SynchronizationToken>> TrackContacts(IEnumerable<PhysicalContact> contacts)
        {
            Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Entrance, $"contacts model: {contacts}; Thread context: {Thread.CurrentThread.ManagedThreadId}");
            //todo: validate each model
            //valiate access right

            return Task.Factory.StartNew<IEnumerable<SynchronizationToken>>(() => {
                Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Body, $"thread execution context: {Thread.CurrentThread.ManagedThreadId}");

                var physicalContactsModel = contacts.Select(contact => new Model.PhysicalContact {
                    Contacts = contact.Contacts,
                    DateTime = contact.DateTime,
                    UserId = contact.UserId,
                    Latitude = Convert.ToDouble(contact.Latitude), Longitude = Convert.ToDouble(contact.Longitude) }).ToList();

                commonCitizenRepository_.AddPhysicalContacts(physicalContactsModel);
                Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Body, $"Added physical contacts.");
                var groupedInitiators = contacts.Select(contact => new {UserId = contact.UserId, DateTime = contact.DateTime }).GroupBy(contact => Convert.ToInt32(contact.UserId)).ToList();
                var synchronizationTokens = new List<SynchronizationToken>();
                Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Body, $"groupedInitiators: {groupedInitiators}");

                foreach(var group in groupedInitiators)
                {
                    var key = group.Key;
                    var trackedChanges = commonCitizenRepository_.GetSynchronizedPhysicalContacts(key, group.Select(grp => grp.DateTime).ToArray());

                    foreach ( var dictKey in trackedChanges.Keys)
                    {
                        if (! trackedChanges.TryGetValue(dictKey, out int syncKey)) { continue; }

                        synchronizationTokens.Add(new SynchronizationToken(key.ToString(), syncKey.ToString(), dictKey));
                    }
                }

                Check.CallerLog<ContactTrackingService>(Logger, LoggerExecutionPositions.Exit, $"synchronizationTokens: {synchronizationTokens}");

                return synchronizationTokens;
            });
        }


    }
}
