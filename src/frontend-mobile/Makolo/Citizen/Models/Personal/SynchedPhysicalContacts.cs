using Backend.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Syncfusion.XForms.ProgressBar;
using Xamarin.Forms.Internals;

namespace Citizen.Models.Personal
{
    public class SynchedPhysicalContacts : PhysicalContact
    {

        private string status;

        public SynchedPhysicalContacts() : base()
        {
            this.ProgressValue = 90;
            this.StepStatus = StepStatus.InProgress;
        }

        public string Title
        {
            get
            {
                return this.Contacts.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>
        public StepStatus StepStatus { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { 
            get
            {
                return this.DateTime.ToShortDateString();
            }
        }

        public string Time 
        {
            get
            {
                return this.DateTime.ToShortTimeString();
            }
        }
        public int ProgressValue { get; set; }
    }
}
