using Common;
using Citizen.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Citizen.Operations;
using Citizen.Models.Personal;

namespace Citizen.Services
{
    public interface IDataStoreService
    {
        Profile LoadProfile();
        IEnumerable<SynchedPhysicalContacts> LoadPhysicalContacts();
        OperationResult<IEnumerable<T>> SaveModels<T>(IEnumerable<Tuple<T, OperationKinds>> modelOperations) where T : IModelDefinition;



        void SaveTemporarily<T>(T value) where T : class, IModelDefinition;
        void ClearTemporaryCache<T>(T value = null) where T : class, IModelDefinition;
    }
}
