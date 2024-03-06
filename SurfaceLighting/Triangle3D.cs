using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class Triangle3D : IPolygon3D
    {
        public Color Color { get; set; }
        public Point3D[] vertices { get; set; }  = new Point3D[3];
        public Point3D[] scaledVertices { get; set; } = new Point3D[3];
        public Vector3[] normalVectors = new Vector3[3];

        Color[] dostepneKolory = {
            Color.Red, Color.Green, Color.Blue,
            Color.Yellow, Color.Magenta, Color.Cyan,
            Color.White, Color.Gray, Color.DarkGray,
            Color.DarkCyan
        };
        Random random = new Random();

        public Triangle3D(float p1X, float p1Y, float p2X, float p2Y,
            float p3X, float p3Y)
        {

            int indeksKoloru = random.Next(dostepneKolory.Length);
            Color = dostepneKolory[indeksKoloru];

            vertices[0] = new Point3D(p1X, p1Y); 
            vertices[1] = new Point3D(p2X, p2Y);
            vertices[2] = new Point3D(p3X, p3Y);
        }

        public Triangle3D(float p1X, float p1Y, float p2X, float p2Y,
            float p3X, float p3Y, int size)
        {
            int indeksKoloru = random.Next(dostepneKolory.Length);
            Color = dostepneKolory[indeksKoloru];

            scaledVertices[0] = new Point3D(p1X, p1Y);
            vertices[0] = new Point3D(reScaleX(p1X, size), reScaleY(p1Y, size));

            scaledVertices[1] = new Point3D(p2X, p2Y);
            vertices[1] = new Point3D(reScaleX(p2X, size), reScaleY(p2Y, size));

            scaledVertices[2] = new Point3D(p3X, p3Y);
            vertices[2] = new Point3D(reScaleX(p3X, size), reScaleY(p3Y, size));
        }

        public Triangle3D(Point3D p1, Point3D p2, Point3D p3)
        {
            int indeksKoloru = random.Next(dostepneKolory.Length);
            Color = dostepneKolory[indeksKoloru];

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

        public float reScaleX(float x, int Width)
        {
            return (x / (Width - 1)) > 0 ? (x / (Width - 1)) : 0;
        }

        public float reScaleY(float y, int Height)
        {
            return (1 - y / (Height - 1)) > 0 ? (1 - y / (Height - 1)) : 0;
        }

    }
}
