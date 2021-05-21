using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Game
{
    public interface IWithTimeout
    {
        long Id { get; }

        bool IsTimedOut { get; }
    }
}
