using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Citizen.Styles
{
    /// <summary>
    /// Class helps to reduce repetitive markup and allows to change the appearance of apps more easily.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhysicalTrackingStyles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Styles" /> class.
        /// </summary>
        public PhysicalTrackingStyles()
        {
            InitializeComponent();
        }
    }
}