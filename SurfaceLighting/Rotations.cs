using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class Rotations
    {
        public int gamma { get; private set; } = 60;

        Matrix4x4 MmodelX = new Matrix4x4();
        Matrix4x4 MmodelY = new Matrix4x4();
        Matrix4x4 MmodelZ = new Matrix4x4();
        Matrix4x4 Mview = new Matrix4x4();
        Matrix4x4 Mproj = new Matrix4x4();
        Matrix4x4 Mx = new Matrix4x4();
        Matrix4x4 My = new Matrix4x4();
        Matrix4x4 Mz = new Matrix4x4();

        Vector3 cameraPosition = new Vector3(0, -2, -2);
        Vector3 cameraTarget = new Vector3(1f/2, 1f/2, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        public Rotations()
        {
            calculateM();
        }

        private void calculateM()
        {
            calculateMmodelX();
            calculateMmodelY();
            calculateMmodelZ();
            calculateMview();
            calculateMproj();
            Mx = MmodelX * Mview * Mproj;
            My = MmodelY * Mview * Mproj;
            Mz = MmodelZ * Mview * Mproj;
        }

        private void calculateMmodelX()
        {
            MmodelX = Matrix4x4.CreateTranslation(-1f / 2, -1f / 2, 0)
                * Matrix4x4.CreateRotationX(DegreesToRadians(gamma))
                * Matrix4x4.CreateTranslation(1f / 2, 1f / 2, 0);
        }

        private void calculateMmodelY()
        {
            MmodelY = Matrix4x4.CreateTranslation(-1f / 2, -1f / 2, 0)
                * Matrix4x4.CreateRotationY(DegreesToRadians(gamma))
                * Matrix4x4.CreateTranslation(1f / 2, 1f / 2, 0);
        }

        private void calculateMmodelZ()
        {
            MmodelZ = Matrix4x4.CreateTranslation(-1f / 2, -1f / 2, 0)
                * Matrix4x4.CreateRotationZ(DegreesToRadians(gamma))
                * Matrix4x4.CreateTranslation(1f / 2, 1f / 2, 0);
        }

        private void calculateMview()
        {
            Mview = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUp);
        }

        private void calculateMproj()
        {
            Mproj = Matrix4x4.CreatePerspectiveFieldOfView(DegreesToRadians(80), 1, 1, 10);
        }

        private float DegreesToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        private Vector3 scaleTransformedVetrices(Vector3 v, int size)
        {
            float x = (v.X + 1) * (float)size / 2;
            float y = (v.Y + 1) * (float)size / 2;
            float z = (v.Z + 1) / 2;

            return new Vector3(x, y, z);
        }

        public void drawLineTransform(Point3D p1, Point3D p2, Pen pen, Graphics g, int size)
        {
            Vector3 p1V = new Vector3(p1.x, p1.y, p1.z);
            Vector3 p2V = new Vector3(p2.x, p2.y, p2.z);
            Vector3 p1T, p2T;

            p1T = Vector3.Transform(p1V, Mx);
            p2T = Vector3.Transform(p2V, Mx);

            p1T = scaleTransformedVetrices(p1T, size);
            p2T = scaleTransformedVetrices(p2T, size);

            g.DrawLine(pen, p1T.X, p1T.Y, p2T.X, p2T.Y);


            p1T = Vector3.Transform(p1V, My);
            p2T = Vector3.Transform(p2V, My);

            p1T = scaleTransformedVetrices(p1T, size);
            p2T = scaleTransformedVetrices(p2T, size);

            g.DrawLine(pen, p1T.X, p1T.Y, p2T.X, p2T.Y);


            p1T = Vector3.Transform(p1V, Mz);
            p2T = Vector3.Transform(p2V, Mz);

            p1T = scaleTransformedVetrices(p1T, size);
            p2T = scaleTransformedVetrices(p2T, size);

            g.DrawLine(pen, p1T.X, p1T.Y, p2T.X, p2T.Y);
        }

        public void rotate()
        {
            gamma += 5;
            calculateM();
        }

        #region fillPolygon

        Comparer<Node> nodeComparer = Comparer<Node>.Create((a, b) => a.xMin.CompareTo(b.xMin));

        private class Node
        {
            public int yMax;
            public float xMin;
            public float step;
        }

        public Triangle3D[] getRotatedTriangle(Triangle3D t, int size)
        {
            Vector3 p1V = new Vector3(t.vertices[0].x, t.vertices[0].y, t.vertices[0].z);
            Vector3 p2V = new Vector3(t.vertices[1].x, t.vertices[1].y, t.vertices[1].z);
            Vector3 p3V = new Vector3(t.vertices[2].x, t.vertices[2].y, t.vertices[2].z);
            Vector3 p1T, p2T, p3T;

            p1T = Vector3.Transform(p1V, Mx);
            p2T = Vector3.Transform(p2V, Mx);
            p3T = Vector3.Transform(p3V, Mx);

            p1T = scaleTransformedVetrices(p1T, size);
            p2T = scaleTransformedVetrices(p2T, size);
            p3T = scaleTransformedVetrices(p3T, size);

            Triangle3D tx = new Triangle3D(new Point3D(p1T.X, p1T.Y, p1T.Z),
                new Point3D(p2T.X, p2T.Y, p2T.Z), new Point3D(p3T.X, p3T.Y, p3T.Z));


            p1T = Vector3.Transform(p1V, My);
            p2T = Vector3.Transform(p2V, My);
            p3T = Vector3.Transform(p3V, My);

            p1T = scaleTransformedVetrices(p1T, size);
            p2T = scaleTransformedVetrices(p2T, size);
            p3T = scaleTransformedVetrices(p3T, size);

            Triangle3D ty = new Triangle3D(new Point3D(p1T.X, p1T.Y, p1T.Z),
                new Point3D(p2T.X, p2T.Y, p2T.Z), new Point3D(p3T.X, p3T.Y, p3T.Z));


            p1T = Vector3.Transform(p1V, Mz);
            p2T = Vector3.Transform(p2V, Mz);
            p3T = Vector3.Transform(p3V, Mz);

            p1T = scaleTransformedVetrices(p1T, size);
            p2T = scaleTransformedVetrices(p2T, size);
            p3T = scaleTransformedVetrices(p3T, size);

            Triangle3D tz = new Triangle3D(new Point3D(p1T.X, p1T.Y, p1T.Z),
                new Point3D(p2T.X, p2T.Y, p2T.Z), new Point3D(p3T.X, p3T.Y, p3T.Z));

            return new Triangle3D[] { tx, ty, tz };
        }

        public void fillPolygons(Triangle3D t, int size, DirectBitmap db,Graphics g)
        {
            Triangle3D[] triangles = getRotatedTriangle(t, size);
            foreach (Triangle3D triangle in triangles)
            {
                fillPolygon(triangle, db, t.Color);
            }
        }

        public void fillPolygon(IPolygon3D polygon, DirectBitmap db, Color I)
        {
            List<Node>[] edgeTable = getEdgeTable(/*(Triangle3D)*/ polygon, out int y, out int edgeTableCount);
            List<Node> activeEdgeTable = new();
            int activeEdgesTableCount = 0;

            while (activeEdgesTableCount > 0 || edgeTableCount > 0)//(activeEdgeTable.Any() || edgeTable.Any(p => p != null)) 
            {
                if (edgeTable[y] != null)
                {
                    foreach (var edge in edgeTable[y])
                    {
                        activeEdgeTable.insertSorted(edge, nodeComparer);
                        activeEdgesTableCount++;
                    }
                    edgeTable[y] = null;
                    edgeTableCount--;
                }

                for (int i = 0; i < activeEdgeTable.Count - 1; i += 2)
                {
                    for (int j = (int)Math.Floor(activeEdgeTable[i].xMin); j <= Math.Ceiling(activeEdgeTable[i + 1].xMin); j++)
                    {
                        //Color I = ((Triangle3D)polygon).Color; //Color.Red;
                        db.SetPixel(j, y, I);
                    }

                }

                for (int i = 0; i < activeEdgeTable.Count; i++)
                {
                    var edge = activeEdgeTable[i];
                    if (y == edge.yMax)
                    {
                        activeEdgeTable.Remove(edge);
                        activeEdgesTableCount--;
                        i--;
                    }
                    else
                        activeEdgeTable[i].xMin += edge.step;
                }
                y++;
            }

        }

        private (int yMin, int yMax) findEdgeY(IPolygon3D polygon)
        {
            float yMin = int.MaxValue, yMax = int.MinValue;
            foreach (var vertex in polygon.vertices)
            {
                if (vertex.y > yMax)
                    yMax = vertex.y;
                if (vertex.y < yMin)
                    yMin = vertex.y;
            }

            return ((int)Math.Floor(yMin), (int)Math.Ceiling(yMax));
        }

        private List<Node>[] getEdgeTable(IPolygon3D polygon, out int yPolygonMin, out int cupsCount)
        {
            (yPolygonMin, int yPolygonMax) = findEdgeY(polygon);
            List<Node>[] edgeTable = new List<Node>[yPolygonMax + 1];
            cupsCount = 0;

            for (int i = 0; i < polygon.vertices.Length; i++)
            {
                int yMin = (int)Math.Ceiling(polygon.vertices[i].y);
                int yMax = (int)Math.Ceiling(polygon.vertices[(i + 1) % polygon.vertices.Length].y);
                float xMin = (int)Math.Floor(polygon.vertices[i].x);
                float xMax = (int)Math.Ceiling(polygon.vertices[(i + 1) % polygon.vertices.Length].x);
                float step = (xMax - xMin) / (yMax - yMin);

                if (yMax < yMin)
                    (yMin, yMax) = (yMax, yMin);

                if (edgeTable[yMin] == null)
                {
                    edgeTable[yMin] = new List<Node>();
                    cupsCount++;
                }

                if (yMin != yMax) // not horizontal edge
                    edgeTable[yMin].Add(new Node()
                    {
                        yMax = yMax,
                        xMin = xMin,
                        step = step
                    });
            }

            return edgeTable;
        }

        #endregion
    }
}
