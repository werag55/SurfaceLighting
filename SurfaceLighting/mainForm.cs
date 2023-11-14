using System.Drawing;
using System.Security.Policy;

namespace SurfaceLighting
{
    public partial class mainForm : Form
    {
        LightingVisualisation lightingVisualisation;

        bool controlPoints = false;
        bool triangleGrid = false;

        public mainForm()
        {
            InitializeComponent();

            int size = visualisationPictureBox.Width < visualisationPictureBox.Height ?
                visualisationPictureBox.Width : visualisationPictureBox.Height;
            lightingVisualisation = new LightingVisualisation(size);

            triangulationTrackBar.Value = lightingVisualisation.bezeierSurface.triangleGrid.n;
            kdTrackBar.Value = (int)(lightingVisualisation.kd * 10);
            ksTrackBar.Value = (int)(lightingVisualisation.ks * 10);
            mTrackBar.Value = lightingVisualisation.m;
        }
        private void visualisationPictureBox_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawImage(lightingVisualisation.visualisationBM.Bitmap, Point.Empty);
            if (triangleGrid)
                e.Graphics.DrawImage(lightingVisualisation.bezeierSurface.triangleGrid.triangleGridBM.Bitmap, Point.Empty);
            if (controlPoints)
                e.Graphics.DrawImage(lightingVisualisation.bezeierSurface.controlPointsBM.Bitmap, Point.Empty);
        }

        private void visualisationPictureBox_SizeChanged(object sender, EventArgs e)
        {
            ((PictureBox)sender).Invalidate();
        }

        #region parameters

        #region control points

        private void controlPointsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (controlPointsCheckBox.Checked)
                controlPoints = true;
            else
                controlPoints = false;

            visualisationPictureBox.Invalidate();
        }

        private void zControlPointTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setZ((float)(((TrackBar)sender).Value) / 10);
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #endregion

        private void visualisationPictureBox_Click(object sender, EventArgs e)
        {

            if (lightingVisualisation.bezeierSurface.clickedOnControlPoint(visualisationPictureBox.PointToClient(MousePosition), textBox1))
            {
                zControlPointTrackBar.Value = (int)(lightingVisualisation.bezeierSurface.controlPoints
                    [lightingVisualisation.bezeierSurface.chosenPoint.i,
                    lightingVisualisation.bezeierSurface.chosenPoint.j].z * 10);
                zControlPointTrackBar.Enabled = true;
            }
            else
            {
                zControlPointTrackBar.Value = 0;
                zControlPointTrackBar.Enabled = false;
            }
            visualisationPictureBox.Invalidate();
        }

        #region triangle grid

        private void triangleGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (triangleGridCheckBox.Checked)
                triangleGrid = true;
            else
                triangleGrid = false;

            visualisationPictureBox.Invalidate();
        }

        private void triangulationTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setN(triangulationTrackBar.Value);
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #region parameters

        private void kdTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setKd((float)(kdTrackBar.Value) / 10);
            visualisationPictureBox.Invalidate();
        }

        private void ksTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setKs((float)(ksTrackBar.Value) / 10);
            visualisationPictureBox.Invalidate();
        }

        private void mTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setM(mTrackBar.Value);
            visualisationPictureBox.Invalidate();
        }


        #endregion
    }
}