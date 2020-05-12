using Backend.DataTransferObjects;
using Backend.Services.Features.ContactTracking;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.GraphQL
{
    public class Mutation
    {
        private readonly ContactTrackingService _service;

        public Mutation(ContactTrackingService service)
        {
            _service = service
                ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Mutate user list of Physical Contacts
        /// </summary>
        /// <param name="contacts">Realized contacts</param>
        /// <param name="service">people contact service</param>
        /// <returns>Synchronization tokens</returns>
        public async Task<IEnumerable<SynchronizationToken>> AddPhysicalContacts(
            IEnumerable<PhysicalContact> contacts,
            [Service] ContactTrackingService service)
        {
            var synchronizedTokens = await service.TrackContacts(contacts);
            return synchronizedTokens;
        }

        /// <summary>
        /// Creates a review for a given Star Wars episode.
        /// </summary>
        /// <param name="episode">The episode to review.</param>
        /// <param name="review">The review.</param>
        /// <param name="eventSender">The event sending service.</param>
        /// <returns>The created review.</returns>
        /*public async Task<Review> CreateReview(
            Episode episode, Review review,
            [Service]IEventSender eventSender)
        {
            _repository.AddReview(episode, review);
            await eventSender.SendAsync(new OnReviewMessage(episode, review));
            return review;
        }*/
    }
}
