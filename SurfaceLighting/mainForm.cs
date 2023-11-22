using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing;
using System.Security;
using System.Security.Policy;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics.Metrics;
using static System.Net.WebRequestMethods;

namespace SurfaceLighting
{
    public partial class mainForm : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private ErrorProvider errorProvider = new();

        LightingVisualisation lightingVisualisation;

        bool filling = true;
        bool controlPoints = false;
        bool triangleGrid = false;

        public mainForm()
        {
            InitializeComponent();
            InitializeTimer();

            int size = visualisationPictureBox.Width < visualisationPictureBox.Height ?
                visualisationPictureBox.Width : visualisationPictureBox.Height;
            lightingVisualisation = new LightingVisualisation(size);

            InitializeValues();

        }

        private void InitializeValues()
        {
            zLightTrackBar.Minimum = zControlPointTrackBar.Maximum;

            triangulationTrackBar.Value = lightingVisualisation.bezeierSurface.triangleGrid.n;
            kdTrackBar.Value = (int)(lightingVisualisation.kd * 10);
            ksTrackBar.Value = (int)(lightingVisualisation.ks * 10);
            mTrackBar.Value = lightingVisualisation.m;
            zLightTrackBar.Value = (int)(lightingVisualisation.lightSource.z * 10);

            triangulationTextBox.Text = lightingVisualisation.bezeierSurface.triangleGrid.n.ToString();
            kdTextBox.Text = lightingVisualisation.kd.ToString();
            ksTextBox.Text = lightingVisualisation.ks.ToString();
            mTextBox.Text = lightingVisualisation.m.ToString();
            zLightTextBox.Text = lightingVisualisation.lightSource.z.ToString();

            objectColorButton.BackColor = Color.FromArgb((int)(lightingVisualisation.Io[0] * 255),
                (int)(lightingVisualisation.Io[1] * 255), (int)(lightingVisualisation.Io[2] * 255));
        }

        #region timer

        private void InitializeTimer()
        {
            timer.Interval = 20;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lightingVisualisation.moveLight();
            visualisationPictureBox.Invalidate();
        }

        #endregion

        private void visualisationPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (filling)
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

        #region textBoxes

        string prevValue = "";
        private void TextBox_Enter(object sender, EventArgs e)
        {
            prevValue = ((TextBox)sender).Text;
            ((TextBox)sender).Clear();
        }

        private void decimalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                (sender as TextBox).Parent.Focus();

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void integerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                (sender as TextBox).Parent.Focus();

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void validatingTextBox(object sender, System.ComponentModel.CancelEventArgs e,
            float min, float max)
        {
            if (((TextBox)sender).Text == " ")
                return;

            if (string.IsNullOrEmpty(((TextBox)sender).Text))
                ((TextBox)sender).Text = prevValue;

            string errorMsg = $"Enter a value between {min} and {max}";
            try
            {
                float result = float.Parse(((TextBox)sender).Text.ToString());
                if (result > max || result < min)
                {
                    e.Cancel = true;
                    ((TextBox)sender).Select(0, ((TextBox)sender).Text.Length);
                    this.errorProvider.SetError((TextBox)sender, errorMsg);
                }
                else
                    this.errorProvider.Clear();
            }
            catch
            {
                e.Cancel = true;
                this.errorProvider.SetError((TextBox)sender, errorMsg);
            }
        }

        #endregion

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
            zControlPointTextBox.Text = ((float)(((TrackBar)sender).Value) / 10).ToString();
            visualisationPictureBox.Invalidate();
        }

        #region textBox

        private void zControlPointTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validatingTextBox(sender, e,
                zControlPointTrackBar.Minimum / 10, zControlPointTrackBar.Maximum / 10);
        }

        private void zControlPointTextBox_Validated(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == " ")
                return;

            float result = float.Parse(zControlPointTextBox.Text.ToString());
            lightingVisualisation.setZ(result);
            zControlPointTrackBar.Value = (int)(result * 10);
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

                zControlPointTextBox.Text = ((float)(zControlPointTrackBar.Value / 10)).ToString();
                zControlPointTextBox.Enabled = true;
            }
            else
            {
                zControlPointTrackBar.Value = 0;
                zControlPointTrackBar.Enabled = false;

                zControlPointTextBox.Text = " ";
                zControlPointTextBox.Enabled = false;
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
            triangulationTextBox.Text = triangulationTrackBar.Value.ToString();
            visualisationPictureBox.Invalidate();
        }

        #region textBox

        private void triangulationTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validatingTextBox(sender, e,
               triangulationTrackBar.Minimum, triangulationTrackBar.Maximum);
        }
        private void triangulationTextBox_Validated(object sender, EventArgs e)
        {
            int result = int.Parse(((TextBox)sender).Text.ToString());
            lightingVisualisation.setN(result);
            triangulationTrackBar.Value = result;
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #endregion

        #region light parameters

        #region kd

        private void kdTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setKd((float)(kdTrackBar.Value) / 10);
            kdTextBox.Text = ((float)(kdTrackBar.Value) / 10).ToString();
            visualisationPictureBox.Invalidate();
        }

        #region textBox

        private void kdTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validatingTextBox(sender, e,
                kdTrackBar.Minimum / 10, kdTrackBar.Maximum / 10);
        }

        private void kdTextBox_Validated(object sender, EventArgs e)
        {
            float result = float.Parse(kdTextBox.Text.ToString());
            lightingVisualisation.setKd(result);
            kdTrackBar.Value = (int)(result * 10);
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #endregion

        #region ks

        private void ksTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setKs((float)(ksTrackBar.Value) / 10);
            ksTextBox.Text = ((float)(ksTrackBar.Value) / 10).ToString();
            visualisationPictureBox.Invalidate();
        }

        #region textBox

        private void ksTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validatingTextBox(sender, e,
                ksTrackBar.Minimum / 10, ksTrackBar.Maximum / 10);
        }

        private void ksTextBox_Validated(object sender, EventArgs e)
        {
            float result = float.Parse(ksTextBox.Text.ToString());
            lightingVisualisation.setKs(result);
            ksTrackBar.Value = (int)(result * 10);
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #endregion

        #region m

        private void mTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setM(mTrackBar.Value);
            mTextBox.Text = mTrackBar.Value.ToString();
            visualisationPictureBox.Invalidate();
        }

        #region textBox

        private void mTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validatingTextBox(sender, e,
                mTrackBar.Minimum, mTrackBar.Maximum);
        }

        private void mTextBox_Validated(object sender, EventArgs e)
        {
            int result = int.Parse(mTextBox.Text.ToString());
            lightingVisualisation.setM(result);
            mTrackBar.Value = result;
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #endregion

        #endregion

        #region object color

        private void fillCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (fillCheckBox.Checked)
                filling = true;
            else
                filling = false;

            visualisationPictureBox.Invalidate();
        }
        private void objectColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                objectColorButton.BackColor = colorDialog.Color;
                lightingVisualisation.setIo(colorDialog.Color);
                visualisationPictureBox.Invalidate();
            }
        }

        private void objectImageButton_Click(object sender, EventArgs e)
        {
            string dir = Application.StartupPath;
            for (int i = 0; i < 2; i++)
                dir = Directory.GetParent(dir).Parent.FullName;
            string imagesDirectory = Path.Combine(dir, "Images");


            OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = imagesDirectory;
            openFileDialog.Filter = "Images (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lightingVisualisation.setImage(openFileDialog.FileName);
                visualisationPictureBox.Invalidate();
            }
        }

        private void imageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (imageRadioButton.Checked)
            {
                objectColorButton.Enabled = false;
                objectImageButton.Enabled = true;

                lightingVisualisation.setObjColor(ObjColor.Image);
            }

            if (solidColorRadioButton.Checked)
            {
                objectColorButton.Enabled = true;
                objectImageButton.Enabled = false;

                lightingVisualisation.setObjColor(ObjColor.Solid);
            }

            visualisationPictureBox.Invalidate();
        }

        #endregion

        #region light 

        private void lightColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lightColorButton.BackColor = colorDialog.Color;
                lightingVisualisation.setIl(colorDialog.Color);
                visualisationPictureBox.Invalidate();
            }
        }

        private void lightMovementCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (lightMovementCheckBox.Checked)
                timer.Start();
            else
                timer.Stop();
        }

        private void zLightTrackBar_Scroll(object sender, EventArgs e)
        {
            lightingVisualisation.setLightZ((float)zLightTrackBar.Value / 10);
            zLightTextBox.Text = ((float)zLightTrackBar.Value / 10).ToString();
            visualisationPictureBox.Invalidate();
        }

        #region textBox

        private void zLightTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            validatingTextBox(sender, e,
                zLightTrackBar.Minimum / 10, zLightTrackBar.Maximum / 10);
        }

        private void zLightTextBox_Validated(object sender, EventArgs e)
        {
            float result = float.Parse(zLightTextBox.Text.ToString());
            lightingVisualisation.setLightZ(result);
            zLightTrackBar.Value = (int)(result * 10);
            visualisationPictureBox.Invalidate();
        }

        #endregion

        #endregion

        #region normal map

        private void normalMapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (normalMapCheckBox.Checked)
                normalMapButton.Enabled = true;
            else
                normalMapButton.Enabled = false;

            lightingVisualisation.setNormalMapBool(normalMapCheckBox.Checked);
            visualisationPictureBox.Invalidate();
        }
        private void normalMapButton_Click(object sender, EventArgs e)
        {
            string dir = Application.StartupPath;
            for (int i = 0; i < 2; i++)
                dir = Directory.GetParent(dir).Parent.FullName;
            string imagesDirectory = Path.Combine(dir, "NormalMaps");


            OpenFileDialog openFileDialog = new();
            openFileDialog.InitialDirectory = imagesDirectory;
            openFileDialog.Filter = "Images (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lightingVisualisation.setNormalMap(openFileDialog.FileName);
                visualisationPictureBox.Invalidate();
            }
        }

        #endregion
        #endregion

    }
}