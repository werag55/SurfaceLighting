using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class TriangleGrid
    {
        public int n { get; private set; }// 2*n*n - number of triangles in the grid
        private int size;
        public List<Triangle3D> triangles { get; set; } = new List<Triangle3D>();
        public DirectBitmap triangleGridBM { get; private set; } 
        public Graphics g { get; private set; }
        public Matrix4x4 M = new Matrix4x4();

        public TriangleGrid(int n, int size)
        {
            this.n = n;
            this.size = size;

            M = Matrix4x4.CreateTranslation(-size / 2, -size / 2, 0)
                * Matrix4x4.CreateFromYawPitchRoll(0, 0, 0)
                * Matrix4x4.CreateTranslation(size / 2, size / 2, 0);

            initTriangleGrid();
            initBitmap();
        }

        public void initTriangleGrid()
        {
            triangles.Clear();
            float step = 1.0f / n;
            for (float i = 0; i < 1; i += step)
            {
                for (float j = 0; j < 1; j += step)
                {
                    triangles.Add(new Triangle3D(i, j, i + step, j, i + step, j + step));
                    triangles.Add(new Triangle3D(i, j, i, j + step, i + step, j + step));
                }
            }

            //float step = (float)size / n;
            //for (float i = 0; i < n; i++)
            //{
            //    for (float j = 1; j <= n; j++)
            //    {
            //        triangles.Add(new Triangle3D(i * step, j * step,
            //            (i + 1) * step, j * step, (i + 1) * step, (j - 1) * step, size));
            //        triangles.Add(new Triangle3D(i * step, j * step,
            //            i * step, (j - 1) * step, (i + 1) * step, (j - 1) * step, size));
            //    }
            //}
        }

        public void initBitmap()
        {
            if (g != null)
            {
                triangleGridBM.Dispose();
                g.Dispose();
            }
            triangleGridBM = new DirectBitmap(size, size);
            g = Graphics.FromImage(triangleGridBM.Bitmap);
            for (int i = 0; i < triangles.Count; i++)
            {
                Triangle3D scaledTriangle = triangleGridBM.scaleTriangle(triangles[i]);
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    //g.DrawPolygon(pen, scaledTriangle.vertices2D());
                    //var vertices = scaledTriangle.vertices2D();
                    for (int j = 0; j < scaledTriangle.vertices.Length; j++)
                        //g.DrawLine(pen, vertices[j], vertices[(j + 1) % vertices.Length]);
                        drawLineTransform(scaledTriangle.vertices[j], scaledTriangle.vertices[(j + 1) % scaledTriangle.vertices.Length],
                            pen, g);
                }
            }
        }

        private void drawLineTransform(Point3D p1, Point3D p2, Pen pen, Graphics g)
        {
            Vector3 p1V = new Vector3(p1.x, p1.y, 1000 * p1.z);
            Vector3 p2V = new Vector3(p2.x, p2.y, 1000 * p2.z);
            p1V = Vector3.Transform(p1V, M);
            p2V = Vector3.Transform(p2V, M);
            g.DrawLine(pen, p1V.X, p1V.Y, p2V.X, p2V.Y);
        }

        public void setN(int n)
        {
            this.n = n;
            initTriangleGrid();
            initBitmap();
        }
    }
}
