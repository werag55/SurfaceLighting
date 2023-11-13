using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SurfaceLighting
{ 
    internal class LightingVisualisation
    {
        public DirectBitmap visualisationBM { get; private set; }
        public Graphics g { get; private set; }
        public BezeierSurface bezeierSurface { get; private set; }

        public LightingVisualisation(int size)
        {
            bezeierSurface = new BezeierSurface(size);
            initBitmap(bezeierSurface.triangleGrid, size);
        }

        public void initBitmap(TriangleGrid tg, int size)
        {
            if (g != null)
            {
                visualisationBM.Dispose();
                g.Dispose();
            }
            visualisationBM = new DirectBitmap(size, size);
            g = Graphics.FromImage(visualisationBM.Bitmap);

            Parallel.ForEach(tg.triangles, triangle =>
                fillPolygon(triangle));

            ///////////////////////////
            ///
            //foreach (var triangle in tg.triangles)
            //    fillPolygon(triangle);
        }

        #region fillPolygon

        Comparer<Node> nodeComparer = Comparer<Node>.Create((a, b) => a.xMin.CompareTo(b.xMin));
        private struct Node
        {
            public int yMax;
            public float xMin;
            public float step;
        }

        public void fillPolygon(IPolygon3D polygon)
        {
            Triangle3D t = (Triangle3D)polygon;
            polygon = visualisationBM.scaleTriangle((Triangle3D)polygon);

            List<Node>[] edgeTable = getEdgeTable(polygon, out int y);
            List<Node> activeEdgeTable = new();
            while (activeEdgeTable.Any() || edgeTable.Any(p => p != null)) 
            {
                if (edgeTable[y] != null)
                    foreach (var edge in edgeTable[y])
                        activeEdgeTable.insertSorted(edge, nodeComparer);
                edgeTable[y] = null;

                for (int i =  0; i < activeEdgeTable.Count - 1; i+=2)
                {
                    for (int j = (int)(activeEdgeTable[i].xMin); j < (int)(activeEdgeTable[i+1].xMin); j++)
                    {
                        Color I = getFillingColor(visualisationBM.reScaleX(j), visualisationBM.reScaleY(y), t); //
                        visualisationBM.SetPixel(j, y, I);
                    }

                }

                
                for (int i = 0; i < activeEdgeTable.Count; i++)
                {
                    var edge = activeEdgeTable[i];
                    if (y == edge.yMax)
                    {
                        activeEdgeTable.Remove(edge);
                        i--;
                    }
                    else
                        edge.xMin += edge.step;
                }
                y++;
            }

        }

        private (int yMin, int yMax) findEdgeY(IPolygon3D polygon)
        {
            int yMin = int.MaxValue, yMax = int.MinValue;
            foreach (var vertex in polygon.vertices)
            {
                if (vertex.y > yMax)
                    yMax = (int)vertex.y;
                if (vertex.y < yMin)
                    yMin = (int)vertex.y;
            }
            return (yMin, yMax);
        }

        private List<Node>[] getEdgeTable(IPolygon3D polygon, out int yPolygonMin)
        {
            (yPolygonMin, int yPolygonMax) = findEdgeY(polygon);
            List<Node>[] edgeTable = new List<Node>[yPolygonMax + 1];

            for(int i = 0;  i < polygon.vertices.Length; i++)
            {
                int yMin = (int)(polygon.vertices[i].y);
                int yMax = (int)(polygon.vertices[(i + 1) % polygon.vertices.Length].y);
                float xMin = (int)(polygon.vertices[i].x);
                float xMax = (int)(polygon.vertices[(i + 1) % polygon.vertices.Length].x);
                float step = (xMax - xMin) / (yMax - yMin);

                if (yMax < yMin)
                    (yMin, yMax) = (yMax, yMin);
                if (xMax < xMin)
                    xMin = xMax;

                if (edgeTable[yMin] == null)
                    edgeTable[yMin] = new List<Node>();
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

        #region color finding

        public float kd = 0.5f, ks = 1; // coefficients describing the influence of a given component (the diffuse component of the illumination 
                                     // rmodel (Lambert model) and the specular component, respectively on the result (0 - 1)
        public float[] Il = { 1, 1, 1 }; // light color scaled to 0-1 (white by default)
        Vector3 V = new Vector3(0, 0, 1);
        public int m = 50; // coefficient describing how much a given triangle is mirrored (1-100)
        Point3D lightSource = new Point3D(0.5f, 0.5f, 1);

        #region interpolation
        private float[] barycentricCoordinates(float x, float y, Triangle3D t)
        {
            Point3D p = new Point3D(x, y, 0);
            float u = triangleArea(new Triangle3D(p, t.vertices[0], t.vertices[2])) / triangleArea(t);
            float v = triangleArea(new Triangle3D(p, t.vertices[0], t.vertices[1])) / triangleArea(t);
            float w = triangleArea(new Triangle3D(p, t.vertices[2], t.vertices[1])) / triangleArea(t);
            return new float[3] { u, v, w };
        }

        public float interpolateZ(float[] barycentricCoordinates, Triangle3D t)
        {
            float zInterpolated = barycentricCoordinates[0] * t.vertices[0].z +
                             barycentricCoordinates[1] * t.vertices[1].z +
                             barycentricCoordinates[2] * t.vertices[2].z;
            return zInterpolated;
        }

        public Vector3 interpolateNormalVector(float[] barycentricCoordinates, Triangle3D t)
        {
            Vector3 normalInterpolated = barycentricCoordinates[0] * t.normalVectors[0] +
                                          barycentricCoordinates[1] * t.normalVectors[1] +
                                          barycentricCoordinates[2] * t.normalVectors[2];
            return normalInterpolated;
        }

        private float triangleArea(Triangle3D t)
        {
            Vector3 AB = new Vector3(t.vertices[1].x - t.vertices[0].x,
                t.vertices[1].y - t.vertices[0].y, 0);
            Vector3 AC = new Vector3(t.vertices[2].x - t.vertices[0].x,
                t.vertices[2].y - t.vertices[0].y, 0);
            return 0.5f * (Vector3.Cross(AB, AC)).Length();
        }

        #endregion

        private float cos(Vector3 U, Vector3 V)
        {
            float ret = Vector3.Dot(U, V);
            return ret < 0 ? 0 : ret;
        }

        private Color getFillingColor(float x, float y, Triangle3D t)
        {
            float[] barycCoord = barycentricCoordinates(x, y, t);
            float z = interpolateZ(barycCoord, t);
            Vector3 N = Vector3.Normalize(interpolateNormalVector(barycCoord, t));
            float[] Io = { 0, (float)128/255, 0 }; // object color scaled to 0-1 (white by default)
            Vector3 L = Vector3.Normalize(new Vector3(lightSource.x - x, lightSource.y - y, lightSource.z - z)); // versor to light source
            Vector3 R = 2 * Vector3.Dot(N, L) * N - L;

            float[] I = new float[3];
            for (int i = 0; i < 3; i++)
            {
                I[i] = 255 * (kd * Il[i] * Io[i] * cos(N, L)
                    + ks * Il[i] * Io[i] * (float)Math.Pow(cos(V, R), m));
                if (I[i] > 255)
                    I[i] = 255;
            }

            return Color.FromArgb((int)I[0], (int)I[1], (int)I[2]);
    }

        #endregion

    }

    public static class ListExtensions
    {
        public static void insertSorted<T>(this List<T> list, T newItem, IComparer<T> comparer)
        {
            int index = list.BinarySearch(newItem, comparer);
            if (index < 0)
            {
                index = ~index;
            }
            list.Insert(index, newItem);
        }
    }
}
