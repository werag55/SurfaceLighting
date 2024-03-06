using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class BezeierSurface
    {
        const int k = 4; // k*k - number of control points
        float radius = 0;
        public int size {  get; private set; }
        public Point3D[,] controlPoints { get; private set; } = new Point3D[k, k];
        Vector3[,] controlVectors = new Vector3[k, k];
        public (int i, int j) chosenPoint { get; private set; } = (-1, -1);
        public TriangleGrid triangleGrid { get; set; }
        public DirectBitmap controlPointsBM { get; private set; }
        public Graphics g { get; private set; }

        public BezeierSurface(int size)
        {
            this.size = size;
            radius = (float)size / 30;

            initControlPoints();
            initControlVectors();
            initBitmap();

            triangleGrid = new TriangleGrid(16, size);
            calculateZ();
            calculateNormalVectors();
            triangleGrid.initBitmap();
        }

        private void initControlPoints()
        {
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    controlPoints[i, j] = new Point3D((float)i / (float)(k - 1),
                        (float)j / (float)(k - 1));

            controlPoints[2, 2].z = 1;
        }

        private void initControlVectors()
        {
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    controlVectors[i, j] = new Vector3(controlPoints[i, j].x, controlPoints[i, j].y, controlPoints[i, j].z);
        }

        public void initBitmap()
        {
            if (g != null)
            {
                controlPointsBM.Dispose();
                g.Dispose();
            }
            controlPointsBM = new DirectBitmap(size, size);
            g = Graphics.FromImage(controlPointsBM.Bitmap);

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    int scaledX = (int)(controlPoints[i, j].x * (controlPointsBM.Width - 1));
                    int scaledY = (int)((1 - controlPoints[i, j].y) * (controlPointsBM.Height - 1));
                    Rectangle bounds = new Rectangle(scaledX - (int)radius, scaledY - (int)radius, (int)(2 * radius), (int)(2 * radius));
                    if (i == chosenPoint.i && j == chosenPoint.j)
                    {
                        using (Pen pen = new Pen(Color.Yellow, 2))
                            g.DrawEllipse(pen, bounds);
                    }
                    else
                    {
                        using (Pen pen = new Pen(Color.Red, 2))
                            g.DrawEllipse(pen, bounds);
                    }
                }
            }
        }

        #region calculateZ
        public void calculateZ()
        {
            foreach (var triangle in triangleGrid.triangles)
            {
                foreach (var point in triangle.vertices)
                {
                    point.z = 0;
                    for (int i = 0; i < k; i++)
                        for (int j = 0; j < k; j++)
                            point.z += controlPoints[i, j].z * B(i, k - 1, point.x) * B(j, k - 1, point.y);
                }
            }
        }

        private float B(int i, int n, float t)
        {
            float x = newtonSymbol(n, i);
            float y = (float)Math.Pow(t, i);
            float z = (float)Math.Pow(1 - t, n - i);
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
            return (float)factorial(n) / (float)(factorial(k) * factorial(n - k));
        }

        #endregion

        #region calculateNormalVectors

        public void calculateNormalVectors()
        {
            foreach (var triangle in triangleGrid.triangles)
                for (int i = 0; i < triangle.vertices.Length; i++)
                    triangle.normalVectors[i] = Vector3.Normalize(Vector3.Cross(Pu(triangle.vertices[i]), Pv(triangle.vertices[i]))); //zmiana
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

        #region click on control point

        private float pointsDistanceSquared(float x1, float y1, float x2, float y2)
        {
             return (float)(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        public bool clickedOnControlPoint(Point p, TextBox text)
        {
            foreach (var t in triangleGrid.triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    var v = t.vertices[i];
                    if (pointsDistanceSquared(controlPointsBM.scaleX(v.x), controlPointsBM.scaleY(v.y), p.X, p.Y) <= (float)(Math.Pow(radius / 2, 2)))
                        text.Text = $"x = {v.x} y= {v.y} z={v.z} \n normal = {t.normalVectors[i].ToString()}" +
                            $"xBM = {triangleGrid.triangleGridBM.scaleX(v.x)} yBM = {triangleGrid.triangleGridBM.scaleY(v.y)}";
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if ((i == 0 && j == 0) || (i == k - 1 && j == k - 1) 
                        || (i == 0 && j == k - 1) || (i == k - 1 && j == 0))
                        continue;

                    if (pointsDistanceSquared(controlPointsBM.scaleX(controlPoints[i, j].x), controlPointsBM.scaleY(controlPoints[i, j].y),
                        p.X, p.Y) <= (float)(Math.Pow(radius, 2)))
                    {
                        chosenPoint = (i, j);
                        initBitmap();
                        return true;
                    }
                }
            }
            chosenPoint = (-1, -1);
            initBitmap();
            return false;
        }

        #region setters

        public void setZ(int i, int j, float z)
        {
            controlPoints[i, j].z = z;
            controlVectors[i, j].Z = z;
            calculateZ();
            calculateNormalVectors();
        }

        public void setZ(float z)
        {
            setZ(chosenPoint.i, chosenPoint.j, z);
        }

        public void setN(int newN)
        {
            triangleGrid.setN(newN);
            calculateZ();
            calculateNormalVectors();
        }

        #endregion

        #endregion
    }
}
