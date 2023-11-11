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

        //#region operators
        //public static Point3D operator -(Point3D p1, Point3D p2)
        //{
        //    return new Point3D(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);  
        //}
        //public static Point3D operator +(Point3D p1, Point3D p2)
        //{
        //    return new Point3D(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        //}
        //public static Point3D operator *(Point3D p, float k)
        //{
        //    return new Point3D(k * p.x, k * p.y, k * p.z);
        //}
        //public static Point3D operator *(float k, Point3D p)
        //{
        //    return p * k;
        //}
        //#endregion
    }
}
