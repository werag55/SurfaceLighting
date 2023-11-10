using System.Drawing;

namespace SurfaceLighting
{
    public partial class mainForm : Form
    {
        DirectBitmap directBitmap;
        Graphics g;
        public mainForm()
        {
            InitializeComponent();
            BezeierSurface bezeierSurface = new BezeierSurface();
        }

        private void visualisationPanel_Paint(object sender, PaintEventArgs e)
        {

            int size = visualisationPanel.Width < visualisationPanel.Height ?
                visualisationPanel.Width : visualisationPanel.Height;
            directBitmap = new DirectBitmap(size, size);
            //g = Graphics.FromImage(directBitmap.Bitmap);

            g = e.Graphics;

            (float, float)[] w = { (0.0f, 0.0f), (0.0f, 1.0f), (1.0f, 0.0f), (1.0f, 1.0f) };

            for (int i = 0; i < w.Length; i++)
            {
                // Okreœl wspó³rzêdne i promieñ okrêgu
                float x = w[i].Item1; // Wspó³rzêdna x [0, 1]
                float y = w[i].Item2; // Wspó³rzêdna y [0, 1]
                float radius = 100f; // Promieñ okrêgu

                // Przeskaluj wspó³rzêdne na podstawie rozmiaru DirectBitmap
                int scaledX = (int)(x * (directBitmap.Width - 1));
                int scaledY = (int)((1 - y) * (directBitmap.Height - 1));

                // Okreœl obszar okrêgu
                Rectangle bounds = new Rectangle(scaledX - (int)radius, scaledY - (int)radius, (int)(2 * radius), (int)(2 * radius));

                // Narysuj okr¹g
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    g.DrawEllipse(pen, bounds);
                }
            }

            triangleGrid tg = new triangleGrid(30);
            for (int i = 0; i < tg.triangles.Count; i++)
            {


                // Przeskaluj wspó³rzêdne na podstawie rozmiaru DirectBitmap
                triangle scaledTriangle = directBitmap.scaleTriangle(tg.triangles[i]);



                // Narysuj okr¹g
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    g.DrawPolygon(pen, scaledTriangle.vertices);
                }
            }

            //e.Graphics.DrawImage(directBitmap.Bitmap, Point.Empty);
        }

        private void visualisationPanel_SizeChanged(object sender, EventArgs e)
        {
            ((Panel)sender).Invalidate();
        }
    }
}