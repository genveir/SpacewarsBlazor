using Geerten.MovementLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Movement
{
    public interface IBody
    {
        /// <summary>
        /// The location of a body
        /// </summary>
        ILocation Location { get; }
    }
}
