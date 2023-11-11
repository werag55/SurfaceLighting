using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class TriangleGrid
    {
        public int n { get; set; } // n*n - number of triangles in the grid
        public List<Triangle3D> triangles { get; set; } = new List<Triangle3D>();

        public TriangleGrid(int n)
        {
            this.n = n;
            initTriangleGrid();
        }

        private void initTriangleGrid(int newN = -1)
        {
            if (newN != -1)
                n = newN;

            float step = 1.0f / n;
            for (float  i = 0; i < n; i+=step)
            {
                for (float j = 0; j < n; j+=step)
                {
                    triangles.Add(new Triangle3D(i, j, i + step, j, i + step, j + step));
                    triangles.Add(new Triangle3D(i, j, i, j + step, i + step, j + step));
                }
            }
        }
    }
}
