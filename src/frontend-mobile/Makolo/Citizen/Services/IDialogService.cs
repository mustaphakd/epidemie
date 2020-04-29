using System.Threading.Tasks;

namespace Citizen.Services
{
    public interface IDialogService
    {
        Task<bool> ConfirmDeletion(string contentTextAppend, bool singleItem);
        Task AlertInfo(string titleKey, string messageKey, params System.Object[] args);
    }
}
