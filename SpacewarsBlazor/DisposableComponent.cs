using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacewarsBlazor
{
    public class DisposableComponent : ComponentBase, IDisposable
    {
        public virtual void Dispose()
        {
            
        }
    }
}
