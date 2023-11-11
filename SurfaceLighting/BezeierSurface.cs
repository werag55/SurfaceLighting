using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class BezeierSurface
    {
        const int k = 4; // k*k - number of control points
        Point3D[,] controlPoints = new Point3D[k, k];
        Vector3[,] controlVectors = new Vector3[k, k];

        public DirectBitmap controlPointsBM { get; private set; }
        public Graphics g { get; private set; }

        public BezeierSurface(TriangleGrid tg, int size)
        {
            initControlPoints();
            initControlVectors();
            initBitmap(size);
            calculateZ(tg);
            calculateNormalVectors(tg);
        }

        private void initControlPoints()
        {
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    controlPoints[i, j] = new Point3D((float)i / (float)(k - 1),
                        (float)j / (float)(k - 1));
        }

        private void initControlVectors()
        {
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    controlVectors[i, j] = new Vector3(controlPoints[i, j].x, controlPoints[i, j].y, controlPoints[i, j].z);
        }

        public void initBitmap(int size)
        {
            if (g != null)
            {
                controlPointsBM.Dispose();
                g.Dispose();
            }
            controlPointsBM = new DirectBitmap(size, size);
            g = Graphics.FromImage(controlPointsBM.Bitmap);

            float radius = (float)size / 30;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    int scaledX = (int)(controlPoints[i, j].x * (controlPointsBM.Width - 1));
                    int scaledY = (int)((1 - controlPoints[i, j].y) * (controlPointsBM.Height - 1));
                    Rectangle bounds = new Rectangle(scaledX - (int)radius, scaledY - (int)radius, (int)(2 * radius), (int)(2 * radius));
                    using (Pen pen = new Pen(Color.Red, 2))
                    {
                        g.DrawEllipse(pen, bounds);
                    }
                }
            }
        }

        #region calculateZ
        public void calculateZ(TriangleGrid triangleGrid)
        {
            foreach (var triangle in triangleGrid.triangles)
                foreach (var point in triangle.vertices)
                    for (int i = 0; i < k; i++)
                        for (int j = 0; j < k; j++)
                            point.z += controlPoints[i, j].z * B(i, k - 1, point.x) * B(j, k - 1, point.y);
        }

        private float B(int i, int n, float t)
        {
            return newtonSymbol(n, i) * (float)Math.Pow(t, i) * (float)Math.Pow(1 - t, n - i);
        }

        private int factorial(int n)
        {
            int ret = 1;
            while(n > 1)
            {
                ret *= n;
                n--;
            }
            return ret;
        }

        private float newtonSymbol(int n, int k)
        {
            return factorial(n) / (factorial(k) * factorial(n - k));
        }

        #endregion

        #region calculateNormalVectors

        public void calculateNormalVectors(TriangleGrid triangleGrid)
        {
            foreach (var triangle in triangleGrid.triangles)
                for (int i = 0; i < triangle.vertices.Length; i++)
                    triangle.normalVectors[i] = Vector3.Cross(Pu(triangle.vertices[i]), Pv(triangle.vertices[i]));
        }

        private Vector3 Pu(Point3D p)
        {
            Vector3 v = new Vector3();
            for (int i = 0; i < k - 1; i++)
                for (int j = 0; j < k; j++)
                    v += (controlVectors[i + 1, j] - controlVectors[i, j]) * B(i, k - 2, p.x) * B(j, k - 1, p.y);
            return v *= (k - 1);
        }

        private Vector3 Pv(Point3D p)
        {
            Vector3 v = new Vector3();
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k - 1; j++)
                    v += (controlVectors[i, j + 1] - controlVectors[i, j]) * B(i, k - 1, p.x) * B(j, k - 2, p.y);
            return v *= (k - 1);
        }

        #endregion
    }
}
