using Citizen.Extensions;
using Citizen.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Citizen.Feature.Login.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private string password;
        private IAuthenticationService authService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.authService = DependencyService.Get<IAuthenticationService>();
        }

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
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

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// https://docs.microsoft.com/en-us/xamarin/essentials/geolocation?tabs=android
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            var outcome = await authService.LoginAsync(new Backend.DataTransferObjects.UserLogin { Email = this.Email, Password = this.Password, Latitude = location?.Latitude.ToString(), Longitude = location?.Longitude.ToString() });

            if (outcome.IsSuccess == false)
            {
                var translationKey = "LoginFailure";
                var translator = new TranslateExtension { Text = translationKey };
                var message = translator.ProvideValue(null).ToString();

                //App.DisplayMessage(message);
                //return;
            }

            ////todo
            //await App.SetAsRoot(Core.Routes.HomeMenu); //.ConfigureAwait(false);
            await App.ReplaceRootContent(Core.Routes.HomeMenu);
            
                //await App.AppendToSection(Core.Routes.HomeMenu);
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
        }


        #endregion
    }
}