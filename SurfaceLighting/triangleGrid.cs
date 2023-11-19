using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public TriangleGrid(int n, int size)
        {
            this.n = n;
            this.size = size;
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
                    g.DrawPolygon(pen, scaledTriangle.vertices2D());
                }
            }
        }

        public void setN(int n)
        {
            this.n = n;
            initTriangleGrid();
            initBitmap();
        }
    }
}
