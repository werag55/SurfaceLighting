using System.Drawing;
using System.Security.Policy;

namespace SurfaceLighting
{
    public partial class mainForm : Form
    {
        TriangleGrid tg;
        BezeierSurface bezeierSurface;
        LightingVisualisation lightingVisualisation;
        public mainForm()
        {
            InitializeComponent();

            int size = visualisationPanel.Width < visualisationPanel.Height ?
                visualisationPanel.Width : visualisationPanel.Height;
            tg = new TriangleGrid(15, size);
            bezeierSurface = new BezeierSurface(tg, size);
            lightingVisualisation = new LightingVisualisation(tg, size);
        }

        private void visualisationPanel_Paint(object sender, PaintEventArgs e)
        { 

            e.Graphics.DrawImage(bezeierSurface.controlPointsBM.Bitmap, Point.Empty);
            e.Graphics.DrawImage(tg.triangleGridBM.Bitmap, Point.Empty);
            e.Graphics.DrawImage(lightingVisualisation.visualisationBM.Bitmap, Point.Empty);
        }

        private void visualisationPanel_SizeChanged(object sender, EventArgs e)
        {
            ((Panel)sender).Invalidate();
        }
    }
}