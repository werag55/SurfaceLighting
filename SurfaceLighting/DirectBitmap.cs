using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SurfaceLighting
{
    //https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
    internal class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[(width+1) * (height+1)]; //[width  * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public DirectBitmap(int size, string filePath)
        {
            Image ig = Image.FromFile(filePath);
            Bitmap temp = new Bitmap(ig, size, size);

            Width = size;
            Height = size;
            Bits = new Int32[(Width + 1) * (Height + 1)]; //[width  * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Color pixelColor = temp.GetPixel(x, y);
                    SetPixel(x, y, pixelColor);
                }
            }
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        #region scaling

        public PointF scalePoint(PointF point)
        {
            return new PointF(scaleX(point.X), scaleY(point.Y));
        }

        public Point3D scalePoint(Point3D point)
        {
            return new Point3D(scaleX(point.x), scaleY(point.y), point.z);
        }

        public float scaleX(float x)
        {
            return (x * (Width - 1)) > 0 ? (x * (Width - 1)) : 0;
        }

        public float scaleY(float y)
        {
            return ((1 - y) * (Height - 1)) > 0 ? ((1 - y) * (Height - 1)) : 0;
        }

        public float reScaleX(float x)
        {
            return (x / (Width - 1)) > 0 ? (x / (Width - 1)) : 0;
        }

        public float reScaleY(float y)
        {
            return (1 - y / (Height - 1)) > 0 ? (1 - y / (Height - 1)) : 0;
        }

        public Triangle3D scaleTriangle(Triangle3D t)
        {
            return new Triangle3D(scalePoint(t.vertices[0]), scalePoint(t.vertices[1]),
                scalePoint(t.vertices[2]));
        }

        #endregion

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}
