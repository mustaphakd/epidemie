using Citizen.Framework;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConnectivityService))]

namespace Citizen.Framework
{
    internal class ConnectivityService : IConnectivityService
    {
        public bool IsThereInternet => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
