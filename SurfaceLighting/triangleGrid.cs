using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class triangleGrid
    {
        public int n { get; set; } // n*n - number of triangles in the grid
        public List<triangle> triangles { get; set; } = new List<triangle>();

        public triangleGrid(int n)
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
                    triangles.Add(new triangle(i, j, i + step, j, i + step, j + step));
                    triangles.Add(new triangle(i, j, i, j + step, i + step, j + step));
                }
            }
        }
    }
}
