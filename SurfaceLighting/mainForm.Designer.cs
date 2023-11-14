namespace SurfaceLighting
{
    partial class mainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            statusStrip = new StatusStrip();
            menuStrip = new MenuStrip();
            ssToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel = new TableLayoutPanel();
            parametersPanel = new Panel();
            parametersTableLayoutPanel = new TableLayoutPanel();
            normalMapPanel = new Panel();
            normalMapButton = new Button();
            normalMapCheckBox = new CheckBox();
            textBox1 = new TextBox();
            triangleGridPanel = new Panel();
            triangulationTrackBar = new TrackBar();
            triangleGridLabel = new Label();
            triangleGridCheckBox = new CheckBox();
            coefficientsPanel = new Panel();
            mTrackBar = new TrackBar();
            mLabel = new Label();
            ksTrackBar = new TrackBar();
            ksLabel = new Label();
            kdTrackBar = new TrackBar();
            kdLabel = new Label();
            IoPanel = new Panel();
            objectImageButton = new Button();
            objectColorButton = new Button();
            imageRadioButton = new RadioButton();
            solidColorRadioButton = new RadioButton();
            IoLabel = new Label();
            controlPointsPanel = new Panel();
            zControlPointTrackBar = new TrackBar();
            controlPointsCheckBox = new CheckBox();
            controlPointsLabel = new Label();
            lightColorPanel = new Panel();
            lightColorButton = new Button();
            lightColorLabel = new Label();
            visualisationPictureBox = new PictureBox();
            menuStrip.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            parametersPanel.SuspendLayout();
            parametersTableLayoutPanel.SuspendLayout();
            normalMapPanel.SuspendLayout();
            triangleGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)triangulationTrackBar).BeginInit();
            coefficientsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ksTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kdTrackBar).BeginInit();
            IoPanel.SuspendLayout();
            controlPointsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)zControlPointTrackBar).BeginInit();
            lightColorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)visualisationPictureBox).BeginInit();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(24, 24);
            statusStrip.Location = new Point(0, 1192);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1678, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Items.AddRange(new ToolStripItem[] { ssToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1678, 33);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // ssToolStripMenuItem
            // 
            ssToolStripMenuItem.Name = "ssToolStripMenuItem";
            ssToolStripMenuItem.Size = new Size(44, 29);
            ssToolStripMenuItem.Text = "ss";
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 521F));
            tableLayoutPanel.Controls.Add(parametersPanel, 1, 0);
            tableLayoutPanel.Controls.Add(visualisationPictureBox, 0, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 33);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size = new Size(1678, 1159);
            tableLayoutPanel.TabIndex = 2;
            // 
            // parametersPanel
            // 
            parametersPanel.BackColor = Color.Snow;
            parametersPanel.Controls.Add(parametersTableLayoutPanel);
            parametersPanel.Dock = DockStyle.Fill;
            parametersPanel.Location = new Point(1160, 3);
            parametersPanel.Name = "parametersPanel";
            parametersPanel.Size = new Size(515, 1153);
            parametersPanel.TabIndex = 0;
            // 
            // parametersTableLayoutPanel
            // 
            parametersTableLayoutPanel.ColumnCount = 1;
            parametersTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            parametersTableLayoutPanel.Controls.Add(normalMapPanel, 0, 5);
            parametersTableLayoutPanel.Controls.Add(textBox1, 0, 6);
            parametersTableLayoutPanel.Controls.Add(triangleGridPanel, 0, 1);
            parametersTableLayoutPanel.Controls.Add(coefficientsPanel, 0, 2);
            parametersTableLayoutPanel.Controls.Add(IoPanel, 0, 3);
            parametersTableLayoutPanel.Controls.Add(controlPointsPanel, 0, 0);
            parametersTableLayoutPanel.Controls.Add(lightColorPanel, 0, 4);
            parametersTableLayoutPanel.Dock = DockStyle.Fill;
            parametersTableLayoutPanel.Location = new Point(0, 0);
            parametersTableLayoutPanel.Name = "parametersTableLayoutPanel";
            parametersTableLayoutPanel.RowCount = 7;
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 47.0394745F));
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 52.9605255F));
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 343F));
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 156F));
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            parametersTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 265F));
            parametersTableLayoutPanel.Size = new Size(515, 1153);
            parametersTableLayoutPanel.TabIndex = 0;
            // 
            // normalMapPanel
            // 
            normalMapPanel.BorderStyle = BorderStyle.FixedSingle;
            normalMapPanel.Controls.Add(normalMapButton);
            normalMapPanel.Controls.Add(normalMapCheckBox);
            normalMapPanel.Dock = DockStyle.Fill;
            normalMapPanel.Location = new Point(3, 830);
            normalMapPanel.Name = "normalMapPanel";
            normalMapPanel.Size = new Size(509, 54);
            normalMapPanel.TabIndex = 9;
            // 
            // normalMapButton
            // 
            normalMapButton.Enabled = false;
            normalMapButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            normalMapButton.Location = new Point(211, 5);
            normalMapButton.Name = "normalMapButton";
            normalMapButton.Size = new Size(112, 34);
            normalMapButton.TabIndex = 10;
            normalMapButton.Text = "Select...";
            normalMapButton.UseVisualStyleBackColor = true;
            // 
            // normalMapCheckBox
            // 
            normalMapCheckBox.AutoSize = true;
            normalMapCheckBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            normalMapCheckBox.Location = new Point(-1, 3);
            normalMapCheckBox.Name = "normalMapCheckBox";
            normalMapCheckBox.Size = new Size(167, 36);
            normalMapCheckBox.TabIndex = 6;
            normalMapCheckBox.Text = "NormalMap";
            normalMapCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(3, 890);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(509, 260);
            textBox1.TabIndex = 7;
            // 
            // triangleGridPanel
            // 
            triangleGridPanel.BorderStyle = BorderStyle.FixedSingle;
            triangleGridPanel.Controls.Add(triangulationTrackBar);
            triangleGridPanel.Controls.Add(triangleGridLabel);
            triangleGridPanel.Controls.Add(triangleGridCheckBox);
            triangleGridPanel.Dock = DockStyle.Fill;
            triangleGridPanel.Location = new Point(3, 127);
            triangleGridPanel.Name = "triangleGridPanel";
            triangleGridPanel.Size = new Size(509, 134);
            triangleGridPanel.TabIndex = 1;
            // 
            // triangulationTrackBar
            // 
            triangulationTrackBar.Location = new Point(4, 98);
            triangulationTrackBar.Maximum = 15;
            triangulationTrackBar.Minimum = 3;
            triangulationTrackBar.Name = "triangulationTrackBar";
            triangulationTrackBar.Size = new Size(499, 69);
            triangulationTrackBar.TabIndex = 5;
            triangulationTrackBar.Value = 3;
            triangulationTrackBar.Scroll += triangulationTrackBar_Scroll;
            // 
            // triangleGridLabel
            // 
            triangleGridLabel.AutoSize = true;
            triangleGridLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            triangleGridLabel.Location = new Point(4, 58);
            triangleGridLabel.Name = "triangleGridLabel";
            triangleGridLabel.Size = new Size(210, 28);
            triangleGridLabel.TabIndex = 4;
            triangleGridLabel.Text = "Triangulation accuracy:";
            // 
            // triangleGridCheckBox
            // 
            triangleGridCheckBox.AutoSize = true;
            triangleGridCheckBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            triangleGridCheckBox.Location = new Point(4, 3);
            triangleGridCheckBox.Name = "triangleGridCheckBox";
            triangleGridCheckBox.Size = new Size(173, 36);
            triangleGridCheckBox.TabIndex = 2;
            triangleGridCheckBox.Text = "Triangle grid\r\n";
            triangleGridCheckBox.UseVisualStyleBackColor = true;
            triangleGridCheckBox.CheckedChanged += triangleGridCheckBox_CheckedChanged;
            // 
            // coefficientsPanel
            // 
            coefficientsPanel.BorderStyle = BorderStyle.FixedSingle;
            coefficientsPanel.Controls.Add(mTrackBar);
            coefficientsPanel.Controls.Add(mLabel);
            coefficientsPanel.Controls.Add(ksTrackBar);
            coefficientsPanel.Controls.Add(ksLabel);
            coefficientsPanel.Controls.Add(kdTrackBar);
            coefficientsPanel.Controls.Add(kdLabel);
            coefficientsPanel.Dock = DockStyle.Fill;
            coefficientsPanel.Location = new Point(3, 267);
            coefficientsPanel.Name = "coefficientsPanel";
            coefficientsPanel.Size = new Size(509, 337);
            coefficientsPanel.TabIndex = 2;
            // 
            // mTrackBar
            // 
            mTrackBar.Location = new Point(4, 274);
            mTrackBar.Maximum = 100;
            mTrackBar.Name = "mTrackBar";
            mTrackBar.Size = new Size(499, 69);
            mTrackBar.TabIndex = 10;
            mTrackBar.Scroll += mTrackBar_Scroll;
            // 
            // mLabel
            // 
            mLabel.AutoSize = true;
            mLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            mLabel.Location = new Point(4, 229);
            mLabel.Name = "mLabel";
            mLabel.Size = new Size(236, 28);
            mLabel.TabIndex = 9;
            mLabel.Text = "Mirroring of the triangles:";
            // 
            // ksTrackBar
            // 
            ksTrackBar.Location = new Point(4, 178);
            ksTrackBar.Name = "ksTrackBar";
            ksTrackBar.Size = new Size(499, 69);
            ksTrackBar.TabIndex = 8;
            ksTrackBar.Scroll += ksTrackBar_Scroll;
            // 
            // ksLabel
            // 
            ksLabel.AutoSize = true;
            ksLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ksLabel.Location = new Point(4, 135);
            ksLabel.Name = "ksLabel";
            ksLabel.Size = new Size(333, 28);
            ksLabel.TabIndex = 7;
            ksLabel.Text = "Influence of the specular component:";
            // 
            // kdTrackBar
            // 
            kdTrackBar.Location = new Point(4, 84);
            kdTrackBar.Name = "kdTrackBar";
            kdTrackBar.Size = new Size(499, 69);
            kdTrackBar.TabIndex = 6;
            kdTrackBar.Scroll += kdTrackBar_Scroll;
            // 
            // kdLabel
            // 
            kdLabel.AutoSize = true;
            kdLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            kdLabel.Location = new Point(4, 11);
            kdLabel.Name = "kdLabel";
            kdLabel.Size = new Size(485, 56);
            kdLabel.TabIndex = 5;
            kdLabel.Text = "Influence of the diffuse component of the illumination \r\nrmodel (Lambert model):";
            // 
            // IoPanel
            // 
            IoPanel.BorderStyle = BorderStyle.FixedSingle;
            IoPanel.Controls.Add(objectImageButton);
            IoPanel.Controls.Add(objectColorButton);
            IoPanel.Controls.Add(imageRadioButton);
            IoPanel.Controls.Add(solidColorRadioButton);
            IoPanel.Controls.Add(IoLabel);
            IoPanel.Dock = DockStyle.Fill;
            IoPanel.Location = new Point(3, 610);
            IoPanel.Name = "IoPanel";
            IoPanel.Size = new Size(509, 150);
            IoPanel.TabIndex = 3;
            // 
            // objectImageButton
            // 
            objectImageButton.Enabled = false;
            objectImageButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            objectImageButton.Location = new Point(211, 94);
            objectImageButton.Name = "objectImageButton";
            objectImageButton.Size = new Size(112, 34);
            objectImageButton.TabIndex = 9;
            objectImageButton.Text = "Select...";
            objectImageButton.UseVisualStyleBackColor = true;
            // 
            // objectColorButton
            // 
            objectColorButton.Location = new Point(211, 54);
            objectColorButton.Name = "objectColorButton";
            objectColorButton.Size = new Size(34, 34);
            objectColorButton.TabIndex = 8;
            objectColorButton.UseVisualStyleBackColor = true;
            objectColorButton.Click += objectColorButton_Click;
            // 
            // imageRadioButton
            // 
            imageRadioButton.AutoSize = true;
            imageRadioButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            imageRadioButton.Location = new Point(21, 92);
            imageRadioButton.Name = "imageRadioButton";
            imageRadioButton.Size = new Size(91, 32);
            imageRadioButton.TabIndex = 2;
            imageRadioButton.Text = "Image";
            imageRadioButton.UseVisualStyleBackColor = true;
            imageRadioButton.CheckedChanged += imageRadioButton_CheckedChanged;
            // 
            // solidColorRadioButton
            // 
            solidColorRadioButton.AutoSize = true;
            solidColorRadioButton.Checked = true;
            solidColorRadioButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            solidColorRadioButton.Location = new Point(21, 54);
            solidColorRadioButton.Name = "solidColorRadioButton";
            solidColorRadioButton.Size = new Size(132, 32);
            solidColorRadioButton.TabIndex = 1;
            solidColorRadioButton.TabStop = true;
            solidColorRadioButton.Text = "Solid color";
            solidColorRadioButton.UseVisualStyleBackColor = true;
            // 
            // IoLabel
            // 
            IoLabel.AutoSize = true;
            IoLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            IoLabel.Location = new Point(4, 9);
            IoLabel.Name = "IoLabel";
            IoLabel.Size = new Size(220, 32);
            IoLabel.TabIndex = 0;
            IoLabel.Text = "Color of the object:";
            // 
            // controlPointsPanel
            // 
            controlPointsPanel.BorderStyle = BorderStyle.FixedSingle;
            controlPointsPanel.Controls.Add(zControlPointTrackBar);
            controlPointsPanel.Controls.Add(controlPointsCheckBox);
            controlPointsPanel.Controls.Add(controlPointsLabel);
            controlPointsPanel.Dock = DockStyle.Fill;
            controlPointsPanel.Location = new Point(3, 3);
            controlPointsPanel.Name = "controlPointsPanel";
            controlPointsPanel.Size = new Size(509, 118);
            controlPointsPanel.TabIndex = 4;
            // 
            // zControlPointTrackBar
            // 
            zControlPointTrackBar.BackColor = Color.Snow;
            zControlPointTrackBar.Enabled = false;
            zControlPointTrackBar.Location = new Point(3, 85);
            zControlPointTrackBar.Maximum = 20;
            zControlPointTrackBar.Name = "zControlPointTrackBar";
            zControlPointTrackBar.Size = new Size(501, 69);
            zControlPointTrackBar.TabIndex = 2;
            zControlPointTrackBar.Scroll += zControlPointTrackBar_Scroll;
            // 
            // controlPointsCheckBox
            // 
            controlPointsCheckBox.AutoSize = true;
            controlPointsCheckBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            controlPointsCheckBox.Location = new Point(3, 3);
            controlPointsCheckBox.Name = "controlPointsCheckBox";
            controlPointsCheckBox.Size = new Size(359, 36);
            controlPointsCheckBox.TabIndex = 1;
            controlPointsCheckBox.Text = "Bezier surface's control points";
            controlPointsCheckBox.UseVisualStyleBackColor = true;
            controlPointsCheckBox.CheckedChanged += controlPointsCheckBox_CheckedChanged;
            // 
            // controlPointsLabel
            // 
            controlPointsLabel.AutoSize = true;
            controlPointsLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            controlPointsLabel.Location = new Point(3, 54);
            controlPointsLabel.Name = "controlPointsLabel";
            controlPointsLabel.Size = new Size(379, 28);
            controlPointsLabel.TabIndex = 3;
            controlPointsLabel.Text = "Z coordinate of the selected control point:";
            // 
            // lightColorPanel
            // 
            lightColorPanel.BorderStyle = BorderStyle.FixedSingle;
            lightColorPanel.Controls.Add(lightColorButton);
            lightColorPanel.Controls.Add(lightColorLabel);
            lightColorPanel.Dock = DockStyle.Fill;
            lightColorPanel.Location = new Point(3, 766);
            lightColorPanel.Name = "lightColorPanel";
            lightColorPanel.Size = new Size(509, 58);
            lightColorPanel.TabIndex = 10;
            // 
            // lightColorButton
            // 
            lightColorButton.Location = new Point(211, 10);
            lightColorButton.Name = "lightColorButton";
            lightColorButton.Size = new Size(34, 34);
            lightColorButton.TabIndex = 7;
            lightColorButton.UseVisualStyleBackColor = true;
            // 
            // lightColorLabel
            // 
            lightColorLabel.AutoSize = true;
            lightColorLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lightColorLabel.Location = new Point(3, 9);
            lightColorLabel.Name = "lightColorLabel";
            lightColorLabel.Size = new Size(202, 32);
            lightColorLabel.TabIndex = 1;
            lightColorLabel.Text = "Color of the light:";
            // 
            // visualisationPictureBox
            // 
            visualisationPictureBox.Dock = DockStyle.Fill;
            visualisationPictureBox.Location = new Point(3, 3);
            visualisationPictureBox.Name = "visualisationPictureBox";
            visualisationPictureBox.Size = new Size(1151, 1153);
            visualisationPictureBox.TabIndex = 1;
            visualisationPictureBox.TabStop = false;
            visualisationPictureBox.Click += visualisationPictureBox_Click;
            visualisationPictureBox.Paint += visualisationPictureBox_Paint;
            // 
            // mainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1678, 1214);
            Controls.Add(tableLayoutPanel);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "mainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Surface Lighting";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            tableLayoutPanel.ResumeLayout(false);
            parametersPanel.ResumeLayout(false);
            parametersTableLayoutPanel.ResumeLayout(false);
            parametersTableLayoutPanel.PerformLayout();
            normalMapPanel.ResumeLayout(false);
            normalMapPanel.PerformLayout();
            triangleGridPanel.ResumeLayout(false);
            triangleGridPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)triangulationTrackBar).EndInit();
            coefficientsPanel.ResumeLayout(false);
            coefficientsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)ksTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)kdTrackBar).EndInit();
            IoPanel.ResumeLayout(false);
            IoPanel.PerformLayout();
            controlPointsPanel.ResumeLayout(false);
            controlPointsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)zControlPointTrackBar).EndInit();
            lightColorPanel.ResumeLayout(false);
            lightColorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)visualisationPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip;
        private MenuStrip menuStrip;
        private ToolStripMenuItem ssToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel;
        private Panel parametersPanel;
        private Panel visualisationPanel;
        private TableLayoutPanel parametersTableLayoutPanel;
        private RadioButton controlPointsRadioButton;
        private CheckBox controlPointsCheckBox;
        private TrackBar zControlPointTrackBar;
        private Label controlPointsLabel;
        private Panel triangleGridPanel;
        private TrackBar triangulationTrackBar;
        private Label triangleGridLabel;
        private CheckBox triangleGridCheckBox;
        private Panel coefficientsPanel;
        private Label kdLabel;
        private Label ksLabel;
        private TrackBar kdTrackBar;
        private TrackBar ksTrackBar;
        private TrackBar mTrackBar;
        private Label mLabel;
        private Panel IoPanel;
        private Label IoLabel;
        private RadioButton imageRadioButton;
        private RadioButton solidColorRadioButton;
        private Panel controlPointsPanel;
        private CheckBox normalMapCheckBox;
        private PictureBox visualisationPictureBox;
        private TextBox textBox1;
        private Panel normalMapPanel;
        private Button normalMapButton;
        private Button objectImageButton;
        private Button objectColorButton;
        private Panel lightColorPanel;
        private Button lightColorButton;
        private Label lightColorLabel;
    }
}