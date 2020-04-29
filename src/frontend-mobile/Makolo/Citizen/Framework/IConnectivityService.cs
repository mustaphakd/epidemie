namespace Citizen.Framework
{
    public interface IConnectivityService
    {
        bool IsThereInternet { get; }
    }
}