using Citizen.Models.Personal;
using OperationResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Operations
{
    public interface IOperationsService
    {
        OperationResult<IEnumerable<SynchedPhysicalContacts>> UpdatePhysicalContacts(IEnumerable<ActionableOperations<SynchedPhysicalContacts>> operations);
        OperationResult<IEnumerable<Profile>> UpdateProfile(IEnumerable<ActionableOperations<Profile>> operations);
    }

    public enum OperationKinds
    {
        Update,
        Create,
        Delete,
        Increase,
        Decrease
    }

    public class ActionableOperations<T>
    {
        public ActionableOperations(T item, OperationKinds operationKind)
        {
            this.Item = item;
            this.OperationKind = operationKind;
        }

        public T Item { get; private set; }
        public OperationKinds OperationKind { get; private set; }
    }
}
