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
            visualisationPanel = new Panel();
            menuStrip.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(24, 24);
            statusStrip.Location = new Point(0, 1186);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1678, 28);
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
            tableLayoutPanel.Controls.Add(visualisationPanel, 0, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 33);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size = new Size(1678, 1153);
            tableLayoutPanel.TabIndex = 2;
            // 
            // parametersPanel
            // 
            parametersPanel.BackColor = Color.Snow;
            parametersPanel.Dock = DockStyle.Fill;
            parametersPanel.Location = new Point(1160, 3);
            parametersPanel.Name = "parametersPanel";
            parametersPanel.Size = new Size(515, 1147);
            parametersPanel.TabIndex = 0;
            // 
            // visualisationPanel
            // 
            visualisationPanel.BackColor = SystemColors.AppWorkspace;
            visualisationPanel.Dock = DockStyle.Fill;
            visualisationPanel.Location = new Point(3, 3);
            visualisationPanel.Name = "visualisationPanel";
            visualisationPanel.Size = new Size(1151, 1147);
            visualisationPanel.TabIndex = 0;
            visualisationPanel.SizeChanged += visualisationPanel_SizeChanged;
            visualisationPanel.Paint += visualisationPanel_Paint;
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
    }
}