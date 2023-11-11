﻿using System;
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
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
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
            float scaledX = point.X * (Width - 1);
            float scaledY = (1 - point.Y) * (Height - 1);
            return new PointF(scaledX, scaledY);
        }

        public Point3D scalePoint(Point3D point)
        {
            float scaledX = point.x * (Width - 1);
            float scaledY = (1 - point.y) * (Height - 1);
            return new Point3D(scaledX, scaledY, point.z);
        }

        public float reScaleX(float x)
        {
            return x / (Width - 1);
        }

        public float reScaleY(float y)
        {
            return 1 - y / (Height - 1);
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