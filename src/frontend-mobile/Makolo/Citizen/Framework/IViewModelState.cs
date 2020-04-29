using Citizen.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citizen.Framework
{
    public interface IViewModelState
    {
        ViewStates State{ get; set; }
    }
}
