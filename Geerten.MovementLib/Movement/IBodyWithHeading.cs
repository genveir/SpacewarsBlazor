using Geerten.MovementLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Movement
{
    public interface IBodyWithHeading : IBody
    {
        /// <summary>
        /// The direction in which the nose of the body is pointed
        /// </summary>
        Direction Heading { get; }
    }
}
