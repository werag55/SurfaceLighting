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

                for (int i =  0; i < activeEdgeTable.Count - 1; i+=2)
                {
                    for (int j = (int)Math.Floor(activeEdgeTable[i].xMin); j <= Math.Ceiling(activeEdgeTable[i+1].xMin); j++)
                    { 
                        Color I = getFillingColor(visualisationBM.reScaleX(j), visualisationBM.reScaleY(y), t /*(Triangle3D)polygon*/);
                        visualisationBM.SetPixel(j, y, I);
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

        private (int yMin, int yMax) findEdgeY(Triangle3D t)
        {
            float yMin = int.MaxValue, yMax = int.MinValue;
            foreach (var vertex in t.vertices) //scaledVertices)
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

            for(int i = 0;  i < polygon.vertices.Length; i++)
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

        private List<Node>[] getEdgeTable(Triangle3D t, out int yPolygonMin, out int cupsCount)
        {
            (yPolygonMin, int yPolygonMax) = findEdgeY(t);
            List<Node>[] edgeTable = new List<Node>[yPolygonMax + 1];
            cupsCount = 0;

            for (int i = 0; i < t.vertices.Length; i++)
            {
                int yMin = (int)Math.Ceiling(t.vertices[i].y); //scaledVertices[i].y); 
                int yMax = (int)Math.Ceiling(t.vertices[(i + 1) % t.vertices.Length].y); //scaledVertices[(i + 1) % t.vertices.Length].y); 
                float xMin = t.vertices[i].x; //scaledVertices[i].x; 
                float xMax = t.vertices[(i + 1) % t.vertices.Length].x;  //scaledVertices[(i + 1) % t.vertices.Length].x;
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

        #region color finding

        public float eps = (float)1e-6;

        public float kd = 1f, ks = 1f; // coefficients describing the influence of a given component (the diffuse component of the illumination 
                                       // rmodel (Lambert model) and the specular component, respectively) on the result (0 - 1)

        public bool showLight { get; private set; } = true;
        public float[] Il { get; private set; } = { 1, 1, 1 }; // light color scaled to 0-1 (white by default)
        public Point3D lightSource { get; private set; } = new Point3D(0.5f, 0.5f, 2);
        public bool showReflectors { get; private set; } = false;

        public float[,] Ilr = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }; // reflectors light color scaled to 0-1 (red, green, blue)
        public Point3D[] reflectors { get; private set; } = { new Point3D(0f, 0f, 2), new Point3D(1f, 0f, 2), new Point3D(0.5f, 1f, 2) };
        public Vector3[] D { get; private set; } = { new Vector3(), new Vector3(), new Vector3() };

        Vector3 V = new Vector3(0, 0, 1);
        public int m = 50; // coefficient describing how much a given triangle is mirrored (1-100)
        

        public ObjColor objColor { get; private set; }
        public float[] Io { get; set; } = { 0f / 255, 128f / 255, 0f / 255 }; // object color scaled to 0-1 (green by default)
        public string imageFilePath { get; private set; } = @"..\..\..\Images\cat.jpg";
        private DirectBitmap imageBM;

        public bool showNormalMap { get; private set; } = false;
        public string normalMapFilePath { get; private set; }  = @"..\..\..\NormalMaps\face.png"; 
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

        public void setLightZ(float z)
        {
            lightSource.z = z;
            initBitmap();
        }

        public void setLightBool(bool light)
        {
            showLight = light;
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

        private float[] barycentricCoordinates(float x, float y, Triangle3D t)
        {
            Point3D p = new Point3D(x, y, 0);
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

        private Color getFillingColor(float x, float y, Triangle3D t)
        {
            float[] barycCoord = barycentricCoordinates(x, y, t);
            float z = interpolateZ(barycCoord, t);

            Vector3 N = Vector3.Normalize(interpolateNormalVector(barycCoord, t));
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
            Vector3 R = Vector3.Normalize(2 * Vector3.Dot(N, L) * N - L);

            float[] I = new float[3];
            for (int i = 0; i < 3; i++)
            {
                if (showLight)
                    I[i] += ((kd * Il[i] * Io[i] * cos(N, L)
                        + ks * Il[i] * Io[i] * (float)Math.Pow(cos(V, R), m)));
                I[i] *= 255;
                if (I[i] > 255)
                    I[i] = 255;
            }

            return Color.FromArgb((int)I[0], (int)I[1], (int)I[2]);
    }

        #endregion


        #region light animation

        private double angle = 0;
        //public void moveLight() // animation of light movement along an ellipse
        //{
        //    float x = (float)(0.5 + 0.4 * Math.Cos(angle)); 
        //    float y = (float)(0.5 + 0.2 * Math.Sin(angle)); 

        //    lightSource = new Point3D(x, y, lightSource.z);

        //    angle += 0.2;
        //    if (angle >= 2 * Math.PI)
        //        angle = 0;

        //    initBitmap();
        //}

        private double revolutions = 2; // number of full spiral revolutions
        private double maxRadius = 0.4; // maximum radius of the spiral
        public void moveLight() // animation of light movement along a spiral with constant z
        {
            double radius = angle / (2 * Math.PI * revolutions) * maxRadius; 
            float x = (float)(0.5 + radius * Math.Cos(angle)); 
            float y = (float)(0.5 + radius * Math.Sin(angle)); 

            lightSource = new Point3D(x, y, lightSource.z);

            angle += 0.1;

            if (angle >= 2 * Math.PI * revolutions)
                angle = 0; 

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
