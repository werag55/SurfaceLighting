using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class Triangle3D
    {
        public Point3D[] vertices = new Point3D[3];
        public float[] normals = new float[3];

        public Triangle3D(float p1X, float p1Y, float p2X, float p2Y,
            float p3X, float p3Y)
        {
            vertices[0] = new Point3D(p1X, p1Y); 
            vertices[1] = new Point3D(p2X, p2Y);
            vertices[2] = new Point3D(p3X, p3Y);
        }

        public Triangle3D(Point3D p1, Point3D p2, Point3D p3)
        { 
            vertices[0] = p1;
            vertices[1] = p2;
            vertices[2] = p3;
        }

        public PointF[] vertices2D()
        {
            PointF[] vertices2D = new PointF[3];
            vertices2D[0] = new PointF(vertices[0].x, vertices[0].y);
            vertices2D[1] = new PointF(vertices[1].x, vertices[1].y);
            vertices2D[2] = new PointF(vertices[2].x, vertices[2].y);
            return vertices2D;
        }
    }
}
