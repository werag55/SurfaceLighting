using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class Point3D
    {
        public float x {get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public Point3D()
        {
            this.x = 0f; this.y = 0f; this.z = 0f;
        }
        public Point3D(float x, float y, float z = 0f)
        {
            this.x = x; this.y = y; this.z = z;
        }
    }
}
