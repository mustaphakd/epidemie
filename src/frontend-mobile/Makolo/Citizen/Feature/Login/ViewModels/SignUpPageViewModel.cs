using Backend.Core.Types;
using Backend.DataTransferObjects;
using Citizen.Extensions;
using Citizen.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Citizen.Feature.Login.ViewModels
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        #region Fields

        private string name;

        private string password;

        private string confirmPassword;

        private UserRegistration registrationModel;

        private IAuthenticationService authService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            this.SignUpCommand = new Command(this.SignUpClicked);
            registrationModel = new UserRegistration();
            this.authService = DependencyService.Get<IAuthenticationService>();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public string Name
        {
            get
            {
                return this.registrationModel.FirstName;
            }

            set
            {
                if (this.registrationModel.FirstName == value)
                {
                    return;
                }

                this.registrationModel.FirstName = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.registrationModel.Password;
            }

            set
            {
                if (this.registrationModel.Password == value)
                {
                    return;
                }

                this.registrationModel.Password = value;
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password confirmation from users in the Sign Up page.
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }

            set
            {
                if (this.confirmPassword == value)
                {
                    return;
                }

                this.confirmPassword = value;
                this.RaisePropertyChanged();
            }
        }

        
        public string LastName
        {
            get
            {
                return this.registrationModel.LastName;
            }

            set
            {
                if (this.registrationModel.LastName == value)
                {
                    return;
                }

                this.registrationModel.LastName = value;
                this.RaisePropertyChanged();
            }
        }


        public string Occupation
        {
            get
            {
                return this.registrationModel.Occupation;
            }

            set
            {
                if (this.registrationModel.Occupation == value)
                {
                    return;
                }

                this.registrationModel.Occupation = value;
                this.RaisePropertyChanged();
            }
        }


        public Gender Gender
        {
            get
            {
                return this.registrationModel.Gender;
            }

            set
            {
                if (this.registrationModel.Gender == value)
                {
                    return;
                }

                this.registrationModel.Gender = value;
                this.RaisePropertyChanged();
            }
        }


        public DateTime BirthDate
        {
            get
            {
                return this.registrationModel.DateOfBirth;
            }

            set
            {
                if (this.registrationModel.DateOfBirth == value)
                {
                    return;
                }

                this.registrationModel.DateOfBirth = value;
                this.RaisePropertyChanged();
            }
        }


        public MaritalStatus Marital
        {
            get
            {
                return this.registrationModel.MaritalStatus;
            }

            set
            {
                if (this.registrationModel.MaritalStatus == value)
                {
                    return;
                }

                this.registrationModel.MaritalStatus = value;
                this.RaisePropertyChanged();
            }
        }


        new public string Email
        {
            get
            {
                return this.registrationModel.Email;
            }

            set
            {
                if (this.registrationModel.Email == value)
                {
                    return;
                }

                this.registrationModel.Email = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            // vALIDATE model
            //if error let user know

            //use service to send request.
           var results = await this.authService.RegisterNewUserAsync(registrationModel);

            var translationKey = results.IsSuccess == true ? "RegistratationSuccess" : "RegistrationFailure";
            var translator = new TranslateExtension { Text = translationKey };
            var message = translator.ProvideValue(null).ToString();

            App.DisplayMessage(message);
        }

        #endregion
    }
}