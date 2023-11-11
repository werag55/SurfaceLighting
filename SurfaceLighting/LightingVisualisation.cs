using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SurfaceLighting
{ 
    internal class LightingVisualisation
    {
        public DirectBitmap visualisationBM { get; private set; }
        public Graphics g { get; private set; }

        public LightingVisualisation(TriangleGrid tg, int size)
        {
            initBitmap(tg, size);
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

            Parallel.ForEach (tg.triangles, triangle =>
                fillPolygon(visualisationBM.scaleTriangle(triangle)));
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
            List<Node>[] edgeTable = getEdgeTable(polygon, out int y);
            List<Node> activeEdgeTable = new();
            while (activeEdgeTable.Any() || edgeTable.Any(p => p != null)) 
            {
                if (edgeTable[y] != null)
                    foreach (var edge in edgeTable[y])
                        activeEdgeTable.insertSorted(edge, nodeComparer);
                edgeTable[y] = null;

                //wypełnij piksele pomiędzy parami przecięć
                for (int i =  0; i < activeEdgeTable.Count - 1; i+=2)
                {
                    for (int j = (int)(activeEdgeTable[i].xMin); j < (int)(activeEdgeTable[i+1].xMin); j++)
                    {
                        Color I = Color.FromArgb(0, 128, 0); //
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
