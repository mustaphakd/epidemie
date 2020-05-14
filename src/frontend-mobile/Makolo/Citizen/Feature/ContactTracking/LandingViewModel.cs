using Backend.Core.Types;
using Citizen.Framework;
using Citizen.Models.Personal;
//using Syncfusion.Data.Extensions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using Citizen.Extensions;
using System.Reflection;
using Citizen.Core;
using Akavache;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Citizen.Feature.ContactTracking
{
    public class LandingViewModel : BaseViewModel
    {

        private ObservableCollection<SynchedPhysicalContacts> synchedPhysicalContacts;
        private string physicallyNearKey = "YouCanInCloseContactWith";
        private string translatedNearKeyText;
        private TranslateExtension translator;
        public LandingViewModel()
        {
            this.synchedPhysicalContacts = new ObservableCollection<SynchedPhysicalContacts>();
            this.translator = new TranslateExtension();
            this.translator.Text = physicallyNearKey;
            this.translatedNearKeyText = translator.ProvideValue(null).ToString();
            ConstructContactsTypes();
            CreateDummy();
        }

        public IEnumerable<SynchedPhysicalContacts> OrderedByDateContacts
        {
            get
            {
                return synchedPhysicalContacts.OrderByDescending(contact => contact.DateTime.Ticks).ToArray();
            }
        }


        public event EventHandler OnDataReady;


        public override async  Task InitializeAsync()
        {
            IEnumerable<SynchedPhysicalContacts> contacts = null ;

            try
            {
                //pull from cache
                contacts = await BlobCache.UserAccount.GetObject<IEnumerable<SynchedPhysicalContacts>>(CacheKeys.User_phyContacts);
            }
            catch (KeyNotFoundException ex)
            {
                //.LogInformation($"AuthenticationService::LoginAsync() Failed to retrieve authmodel from cache.");
            }

            if (contacts == null || contacts.Count() < 1)
            {

                await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => {
                    this.OnDataReady(this, new EventArgs());
                });

                return; 
            }

            foreach(var item in contacts)
            {
                PhysicalContactsTrackings.Add(item);
            }

            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => {
                this.OnDataReady(this, new EventArgs());
            });
        }

        public override Task UninitializeAsync()
        {

            try
            {
                //save to cache
                var result =  BlobCache.UserAccount.InsertObject<IEnumerable<SynchedPhysicalContacts>>(CacheKeys.User_phyContacts, PhysicalContactsTrackings);
            }
            catch (KeyNotFoundException ex)
            {
                //.LogInformation($"AuthenticationService::LoginAsync() Failed to retrieve authmodel from cache.");
            }

            return base.UninitializeAsync();
        }

        private void CreateDummy()
        {
            PhysicalContactsTrackings.Add(new SynchedPhysicalContacts
            {
                Contacts = Backend.Core.Types.PhysicalContacts.Family_GrandFather,
                DateTime = System.DateTime.Now.AddHours(5),
                StepStatus = Syncfusion.XForms.ProgressBar.StepStatus.Completed
            });


            PhysicalContactsTrackings.Add(new SynchedPhysicalContacts
            {
                Contacts = Backend.Core.Types.PhysicalContacts.Family_GrandFather,
                DateTime = System.DateTime.Now.AddDays(10),
                StepStatus = Syncfusion.XForms.ProgressBar.StepStatus.NotStarted
            });


            PhysicalContactsTrackings.Add(new SynchedPhysicalContacts
            {
                Contacts = Backend.Core.Types.PhysicalContacts.Family_GrandFather,
                DateTime = System.DateTime.Now,
                StepStatus = Syncfusion.XForms.ProgressBar.StepStatus.InProgress
            });
        }

        internal FormattedString GetSecondaryText(SynchedPhysicalContacts item)
        {
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { FontAttributes = FontAttributes.Bold, Text = item.Date });
            formatted.Spans.Add(new Span { Text="\n\r" });
            formatted.Spans.Add(new Span { Text = item.Time });
            return formatted;
        }

        internal FormattedString GetPrimaryText(SynchedPhysicalContacts item)
        {
            var translatedEnumValue = GetTranslatedEnum(item.Contacts);
            var formatted = new FormattedString();
            formatted.Spans.Add(new Span { Text = translatedNearKeyText });
            formatted.Spans.Add(new Span { Text = "\n\r" });
            formatted.Spans.Add(new Span { FontAttributes = FontAttributes.Bold, Text = " " + translatedEnumValue });
            return formatted;
        }

        private object GetTranslatedEnum(PhysicalContacts contacts)
        {
            var enumType = typeof(PhysicalContacts);
            var enumValue = Enum.GetName(enumType, contacts);
            var translatedValue = ContactsKeyTranslation[enumValue];
            return translatedValue;
        }

        public Dictionary<string, string> ContactsKeyTranslation { get; } = new Dictionary<string, string>();
        private void ConstructContactsTypes()
        {
            //ContactsKeyTranslation 
            var enumType = typeof(PhysicalContacts);
            var members = Enum.GetNames(enumType);
            var translator = new TranslateExtension();

            foreach (var member in members)
            {
                var key = member;
                var memberInfos = enumType.GetMember(member);
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);

                var valueAttribute =
                      enumValueMemberInfo.GetCustomAttribute(typeof(EnumLangualKeyAttribute), false);
                var languageKey = ((EnumLangualKeyAttribute)valueAttribute).LanguageKey;

                translator.Text = languageKey;
                var enumPickerValue = translator.ProvideValue(null).ToString();

                ContactsKeyTranslation.Add(key, enumPickerValue);
            }
        }

        public async Task<SynchedPhysicalContacts> AddContact(PhysicalContacts contact, DateTime date, string Longitude, string Latitude)
        {
            var newContact = new SynchedPhysicalContacts { Contacts = contact, DateTime = date, Latitude = Latitude, Longiture = Longitude, StepStatus = Syncfusion.XForms.ProgressBar.StepStatus.InProgress };
            PhysicalContactsTrackings.Add(newContact);


            await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => {
                this.OnDataReady(this, new EventArgs());
            });

            return newContact;
        }

        public ObservableCollection<SynchedPhysicalContacts> PhysicalContactsTrackings
        {
            get
            {
                return this.synchedPhysicalContacts;
            }

            set
            {
                this.synchedPhysicalContacts = value;
                this.RaisePropertyChanged();
            }
        }

    }
}
