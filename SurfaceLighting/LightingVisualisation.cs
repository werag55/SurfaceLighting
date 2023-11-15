using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

public enum ObjColor
{
    Solid,
    Image
}

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
            initBitmap();
            setImage(imageFilePath);
            setNormalMap(normalMapFilePath);

        }

        public void initBitmap()
        {
            TriangleGrid tg = bezeierSurface.triangleGrid;
            if (g != null)
            {
                visualisationBM.Dispose();
                g.Dispose();
            }
            visualisationBM = new DirectBitmap(bezeierSurface.size, bezeierSurface.size);
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

        private class Node
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
                    for (int j = (int)Math.Floor(activeEdgeTable[i].xMin); j <= Math.Ceiling(activeEdgeTable[i+1].xMin); j++) // < czy <=
                    { 

                        Color? I = getFillingColor(visualisationBM.reScaleX(j), visualisationBM.reScaleY(y), t); //
                        //if (I == null)
                        //    continue;
                        //if (I != null)
                        visualisationBM.SetPixel(j, y, (Color)I);
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

        private List<Node>[] getEdgeTable(IPolygon3D polygon, out int yPolygonMin)
        {
            (yPolygonMin, int yPolygonMax) = findEdgeY(polygon);
            List<Node>[] edgeTable = new List<Node>[yPolygonMax + 1];

            for(int i = 0;  i < polygon.vertices.Length; i++)
            {
                int yMin = (int)Math.Floor(polygon.vertices[i].y);
                int yMax = (int)Math.Ceiling(polygon.vertices[(i + 1) % polygon.vertices.Length].y);
                float xMin = (int)Math.Floor(polygon.vertices[i].x);
                float xMax = (int)Math.Ceiling(polygon.vertices[(i + 1) % polygon.vertices.Length].x);
                float step = (xMax - xMin) / (yMax - yMin);

                if (yMax < yMin)
                {
                    yMin = (int)Math.Floor(polygon.vertices[(i + 1) % polygon.vertices.Length].y);
                    yMax = (int)Math.Ceiling(polygon.vertices[i].y);
                }
                //if (xMax < xMin)
                //    xMin = xMax;

                if (edgeTable[yMin] == null)
                    edgeTable[yMin] = new List<Node>();

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

        #region color finding

        public float eps = (float)1e-6;

        public float kd = 1f, ks = 1f; // coefficients describing the influence of a given component (the diffuse component of the illumination 
                                     // rmodel (Lambert model) and the specular component, respectively) on the result (0 - 1)
        public float[] Il { get; private set; } = { 1, 1, 1 }; // light color scaled to 0-1 (white by default)
        Vector3 V = new Vector3(0, 0, 1);
        public int m = 60; // coefficient describing how much a given triangle is mirrored (1-100)
        Point3D lightSource = new Point3D(0.5f, 0.5f, 5);

        public ObjColor objColor { get; private set; }
        public float[] Io { get; set; } = { 0, 128f / 255, 0 }; // object color scaled to 0-1 (green by default)
        public string imageFilePath { get; private set; } = @"..\..\..\Images\cat.jpg";//@"C:\Users\weron\OneDrive\Pulpit\semestr5\gk\proj2\SurfaceLighting\SurfaceLighting\Images\cat.jpg";//@"Images/cat.jpg";
        private DirectBitmap imageBM;

        public bool showNormalMap { get; private set; } = false;
        public string normalMapFilePath { get; private set; }  = @"..\..\..\NormalMaps\NormalMap.jpg";
        private DirectBitmap normalMapBM;

        #region setters

        public void setKd(float newKd)
        {
            kd = newKd;
            initBitmap();
        }

        public void setKs(float newKs)
        {
            ks = newKs;
            initBitmap();
        }

        public void setIl(Color c)
        {
            Il = new float[] { (float)c.R / 255, (float)c.G / 255, (float)c.B / 255, };
            initBitmap();
        }

        public void setM(int newM)
        {
            m = newM;
            initBitmap();
        }

        public void setN(int newN)
        {
            bezeierSurface.setN(newN);
            initBitmap();
        }

        public void setZ(float newZ)
        {
            bezeierSurface.setZ(newZ);
            initBitmap();
        }

        public void setObjColor(ObjColor objC)
        {
            objColor = objC;
            initBitmap();
        }

        public void setIo(Color c)
        {
            Io = new float[] { (float)c.R / 255, (float)c.G / 255, (float)c.B / 255, };
            initBitmap();
        }

        public void setImage(string filePath)
        {
            if (imageBM != null)
                imageBM.Dispose();
            imageFilePath = filePath;
            imageBM = new DirectBitmap(bezeierSurface.size, imageFilePath);
            initBitmap();
        }

        public void setNormalMapBool(bool show)
        {
            showNormalMap = show;
            initBitmap();
        }

        public void setNormalMap(string filePath = "")
        {
            if (normalMapBM != null)
                normalMapBM.Dispose();
            normalMapFilePath = filePath;
            normalMapBM = new DirectBitmap(bezeierSurface.size, normalMapFilePath);

            initBitmap();
        }

        #endregion

        #region interpolation

        //private float[] barycentricCoordinates(float x, float y, float z, Triangle3D t)
        //{
        //    PointF[] vertices2D = t.vertices2D();
        //    float x1 = vertices2D[0].X;
        //    float y1 = vertices2D[0].Y;
        //    float x2 = vertices2D[1].X;
        //    float y2 = vertices2D[1].Y;
        //    float x3 = vertices2D[2].X;
        //    float y3 = vertices2D[2].Y;

        //    float alpha = ((y2 - y3) * (x - x3) + (x3 - x2) * (y - y3)) /
        //                  ((y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3));
        //    float beta = ((y3 - y1) * (x - x3) + (x1 - x3) * (y - y3)) /
        //                 ((y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3));
        //    float gamma = 1 - alpha - beta;

        //    return new float[] { alpha, beta, gamma };
        //}

        private float[] barycentricCoordinates(float x, float y, float z, Triangle3D t)
        {
            Point3D p = new Point3D(x, y, z);
            float u = triangleArea(new Triangle3D(p, t.vertices[1], t.vertices[2])) / triangleArea(t);
            float v = triangleArea(new Triangle3D(p, t.vertices[0], t.vertices[2])) / triangleArea(t);
            float w = triangleArea(new Triangle3D(p, t.vertices[0], t.vertices[1])) / triangleArea(t);
            return new float[3] { u, v, w };
        }

        private float triangleArea(Triangle3D t)
        {
            Vector3 AB = new Vector3(t.vertices[1].x - t.vertices[0].x,
                t.vertices[1].y - t.vertices[0].y, 0);
            Vector3 AC = new Vector3(t.vertices[2].x - t.vertices[0].x,
                t.vertices[2].y - t.vertices[0].y, 0);

            return 0.5f * Vector3.Cross(AB, AC).Length();
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

        #endregion

        private Vector3 normalMapVector(Color normalMapColor)
        {
            float X = ((float)normalMapColor.R /255) * 2 - 1;
            float Y = ((float)normalMapColor.G / 255) * 2 - 1;
            float Z = ((float)normalMapColor.B / 255);
            return new Vector3(X, Y, Z);
        }

        public Vector3 modifyNormalVector(Vector3 N, Color normalMapColor)
        {
            Vector3 B = Vector3.Cross(N, new Vector3(0,0,1));
            if (Math.Abs(N.X) < eps && Math.Abs(N.Y) < eps 
                && Math.Abs(N.Z - 1) < eps)
                B = new Vector3(0, 1, 0);

            Vector3 T = Vector3.Cross(B, N);

            Vector3 Nt = normalMapVector(normalMapColor);

            Vector3 row1 = new Vector3(T.X, B.X, N.X);
            Vector3 row2 = new Vector3(T.Y, B.Y, N.Y);
            Vector3 row3 = new Vector3(T.Z, B.Z, N.Z);

            float resultX = Vector3.Dot(row1, Nt);
            float resultY = Vector3.Dot(row2, Nt);
            float resultZ = Vector3.Dot(row3, Nt);

            return new Vector3(resultX, resultY, resultZ);
        }

        private float cos(Vector3 U, Vector3 V)
        {
            float ret = Vector3.Dot(U, V);
            return ret < 0 ? 0 : ret;
        }

        private Color? getFillingColor(float x, float y, Triangle3D t)
        {
            float[] barycCoordZ = barycentricCoordinates(x, y, 0, t);
            float z = interpolateZ(barycCoordZ, t);

            //if (barycCoord[0] < -0.000001 || barycCoord[1] < -0.000001 || barycCoord[2] < -0.000001)
            //    return null;


            float[] barycCoordN = barycentricCoordinates(x, y, z, t);
            Vector3 N = Vector3.Normalize(interpolateNormalVector(barycCoordN, t));
            if (showNormalMap)
                N = Vector3.Normalize(modifyNormalVector(N, 
                    normalMapBM.GetPixel((int)normalMapBM.scaleX(x), (int)normalMapBM.scaleY(y))));

            float[] Io;
            if (objColor == ObjColor.Solid)
                Io = this.Io;
            else
            {
                Color pixelColor = imageBM.GetPixel((int)imageBM.scaleX(x), (int)imageBM.scaleY(y));
                Io = new float[] { (float)(pixelColor.R) / 255, (float)(pixelColor.G) / 255, 
                    (float)(pixelColor.B) / 255 };
            }

            Vector3 L = Vector3.Normalize(new Vector3(lightSource.x - x, lightSource.y - y, lightSource.z - z)); // versor to light source
            //Vector3 L = new Vector3(0, 0, 1);
            Vector3 V = new Vector3(0, 0, 1);
            Vector3 R = Vector3.Normalize(2 * Vector3.Dot(N, L) * N - L);

            float[] I = new float[3];
            for (int i = 0; i < 3; i++)
            {
                I[i] = 255 * ((kd * Il[i] * Io[i] * cos(N, L)
                    + ks * Il[i] * Io[i] * (float)Math.Pow(cos(V, R), m)));
                if (I[i] > 255)
                    I[i] = 255;
            }

            return Color.FromArgb((int)I[0], (int)I[1], (int)I[2]);
    }

        #endregion


        #region light animation

        private double angle = 0;
        public void moveLight()
        {
            // Obliczenia pozycji punktu (światła) na elipsie w zakresie od 0 do 1
            float x = (float)(0.5 + 0.4 * Math.Cos(angle)); // 0.5 to przesunięcie do środka obszaru (0.1 to 0.9)
            float y = (float)(0.5 + 0.2 * Math.Sin(angle)); // 0.5 to przesunięcie do środka obszaru (0.3 to 0.7)

            // Aktualizacja pozycji punktu (światła)
            lightSource = new Point3D(x, y, lightSource.z);

            // Inkrementacja kąta dla ruchu po elipsie
            angle += 0.2;
            if (angle >= 2 * Math.PI)
            {
                angle = 0;
            }

            initBitmap();
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
