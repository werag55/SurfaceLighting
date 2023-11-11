using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal interface IPolygon3D
    {
        Point3D[] vertices { get; set; }
    }
}
