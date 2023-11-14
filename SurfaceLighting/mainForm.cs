using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

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

        #region light parameters

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

        #region object color

        private void objectColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                objectColorButton.BackColor = colorDialog.Color;
                lightingVisualisation.Io = new float[] { (float)colorDialog.Color.R / 255,
                    (float)colorDialog.Color.G / 255, (float)colorDialog.Color.B / 255};
                lightingVisualisation.initBitmap();
                visualisationPictureBox.Invalidate();
            }
        }

        private void imageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (imageRadioButton.Checked)
            {
                objectColorButton.Enabled = false;
                objectImageButton.Enabled = true;

                lightingVisualisation.objColor = ObjColor.Image;
            }

            if (solidColorRadioButton.Checked)
            {
                objectColorButton.Enabled = true;
                objectImageButton.Enabled = false;

                lightingVisualisation.objColor = ObjColor.Solid;
            }
        }

        #endregion

        #region light color

        private void lightColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lightColorButton.BackColor = colorDialog.Color;
                lightingVisualisation.Il = new float[] { (float)colorDialog.Color.R / 255,
                    (float)colorDialog.Color.G / 255, (float)colorDialog.Color.B / 255};
                lightingVisualisation.initBitmap();
                visualisationPictureBox.Invalidate();
            }
        }

        #endregion

        #endregion

    }
}