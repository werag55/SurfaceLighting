using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    internal class BezeierSurface
    {
        const int k = 4; // k*k - number of control points
        PointF[,] controlPoints = new PointF[k, k];

        public BezeierSurface()
        {
            initControlPoints();
        }

        private void initControlPoints()
        {
            for (int i = 0; i < k; i++)
                for (int j = 0; j < k; j++)
                    controlPoints[i, j] = new PointF((float)i / (float)(k - 1),
                        (float)j / (float)(k - 1));
        }
    }
}
