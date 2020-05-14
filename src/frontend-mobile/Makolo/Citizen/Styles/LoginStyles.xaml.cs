using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Citizen.Styles
{
    /// <summary>
    /// Class helps to reduce repetitive markup, and allows an apps appearance to be more easily changed.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginStyles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginStyles" /> class.
        /// </summary>
        public LoginStyles()
        {
            InitializeComponent();
        }
    }
}