using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class triangle
    {
        public PointF[] vertices = new PointF[3];
        public float[] normals = new float[3];

        public triangle(float p1X, float p1Y, float p2X, float p2Y,
            float p3X, float p3Y)
        {
            vertices[0] = new PointF(p1X, p1Y);
            vertices[1] = new PointF(p2X, p2Y);
            vertices[2] = new PointF(p3X, p3Y);
        }

        public triangle(PointF p1, PointF p2, PointF p3)
        { 
            vertices[0] = p1;
            vertices[1] = p2;
            vertices[2] = p3;
        }
    }
}
