using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.DataTransferObjects
{
    /// <summary>
    /// Tracks changes across islands
    /// </summary>
    public class SynchronizationToken
    {
        public SynchronizationToken(string clientId, string storeId, DateTime dateTime)
        {
            ClientIdentifier = clientId;
            StoreIdentifier = storeId;
            DateTime = dateTime;
        }

        /// <summary>
        /// Originator
        /// </summary>
        public string ClientIdentifier { get; }

        /// <summary>
        /// Destinator
        /// </summary>
        public string StoreIdentifier { get; }

        /// <summary>
        /// Additonal Unique token inside batch
        /// </summary>
        public DateTime DateTime { get; }
    }
}
