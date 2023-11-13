using System.Drawing;
using System.Security.Policy;

namespace SurfaceLighting
{
    public partial class mainForm : Form
    {
        LightingVisualisation lightingVisualisation;
        public mainForm()
        {
            InitializeComponent();

            int size = visualisationPanel.Width < visualisationPanel.Height ?
                visualisationPanel.Width : visualisationPanel.Height;
            lightingVisualisation = new LightingVisualisation(size);
        }

        private void visualisationPanel_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawImage(lightingVisualisation.bezeierSurface.controlPointsBM.Bitmap, Point.Empty);
            e.Graphics.DrawImage(lightingVisualisation.bezeierSurface.triangleGrid.triangleGridBM.Bitmap, Point.Empty);
            e.Graphics.DrawImage(lightingVisualisation.visualisationBM.Bitmap, Point.Empty);
        }

        private void visualisationPanel_SizeChanged(object sender, EventArgs e)
        {
            ((Panel)sender).Invalidate();
        }

        private void controlPointsCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void zControlPointTrackBar_Scroll(object sender, EventArgs e)
        {

        }
    }
}