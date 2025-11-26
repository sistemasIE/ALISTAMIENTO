namespace ALISTAMIENTO_IE.Forms
{
    partial class EtqsEliminadasEnAlistamiento
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tlpRoot = new TableLayoutPanel();
            lstEtiquetasEliminadas = new ListView();
            colEtiqueta = new ColumnHeader();
            colFecha = new ColumnHeader();
            pnlRight = new TableLayoutPanel();
            dgvInfoEliminacion = new DataGridView();
            txtObservaciones = new TextBox();
            lblObservaciones = new Label();
            button1 = new Button();
            btnRevertir = new Button();
            tlpRoot.SuspendLayout();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInfoEliminacion).BeginInit();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.BackgroundImage = Properties.Resources.Background_IE;
            tlpRoot.ColumnCount = 2;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 259F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(lstEtiquetasEliminadas, 0, 0);
            tlpRoot.Controls.Add(pnlRight, 1, 0);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(8);
            tlpRoot.RowCount = 1;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.Size = new Size(548, 515);
            tlpRoot.TabIndex = 0;
            // 
            // lstEtiquetasEliminadas
            // 
            lstEtiquetasEliminadas.Columns.AddRange(new ColumnHeader[] { colEtiqueta, colFecha });
            lstEtiquetasEliminadas.Dock = DockStyle.Fill;
            lstEtiquetasEliminadas.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstEtiquetasEliminadas.FullRowSelect = true;
            lstEtiquetasEliminadas.Location = new Point(11, 11);
            lstEtiquetasEliminadas.MultiSelect = false;
            lstEtiquetasEliminadas.Name = "lstEtiquetasEliminadas";
            lstEtiquetasEliminadas.Size = new Size(253, 493);
            lstEtiquetasEliminadas.TabIndex = 0;
            lstEtiquetasEliminadas.UseCompatibleStateImageBehavior = false;
            lstEtiquetasEliminadas.View = View.Details;
            lstEtiquetasEliminadas.DoubleClick += lstEtiquetasEliminadas_DoubleClick;
            // 
            // colEtiqueta
            // 
            colEtiqueta.Text = "Etiqueta";
            colEtiqueta.Width = 140;
            // 
            // colFecha
            // 
            colFecha.Text = "Fecha Eliminación";
            colFecha.Width = 120;
            // 
            // pnlRight
            // 
            pnlRight.ColumnCount = 1;
            pnlRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRight.Controls.Add(dgvInfoEliminacion, 0, 2);
            pnlRight.Controls.Add(txtObservaciones, 0, 1);
            pnlRight.Controls.Add(lblObservaciones, 0, 0);
            pnlRight.Controls.Add(button1, 0, 4);
            pnlRight.Controls.Add(btnRevertir, 0, 3);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(270, 11);
            pnlRight.Name = "pnlRight";
            pnlRight.RowCount = 6;
            pnlRight.RowStyles.Add(new RowStyle());
            pnlRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 266F));
            pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 75.60241F));
            pnlRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 24.39759F));
            pnlRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            pnlRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            pnlRight.Size = new Size(267, 493);
            pnlRight.TabIndex = 1;
            // 
            // dgvInfoEliminacion
            // 
            dgvInfoEliminacion.AllowUserToAddRows = false;
            dgvInfoEliminacion.AllowUserToDeleteRows = false;
            dgvInfoEliminacion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInfoEliminacion.BackgroundColor = SystemColors.ActiveCaptionText;
            dgvInfoEliminacion.Dock = DockStyle.Fill;
            dgvInfoEliminacion.Location = new Point(3, 303);
            dgvInfoEliminacion.Name = "dgvInfoEliminacion";
            dgvInfoEliminacion.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.LimeGreen;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvInfoEliminacion.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvInfoEliminacion.RowHeadersVisible = false;
            dgvInfoEliminacion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInfoEliminacion.Size = new Size(261, 79);
            dgvInfoEliminacion.TabIndex = 4;
            // 
            // txtObservaciones
            // 
            txtObservaciones.BackColor = SystemColors.HighlightText;
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtObservaciones.Location = new Point(3, 37);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ReadOnly = true;
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(261, 260);
            txtObservaciones.TabIndex = 3;
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.Dock = DockStyle.Top;
            lblObservaciones.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObservaciones.Location = new Point(0, 0);
            lblObservaciones.Margin = new Padding(0, 0, 0, 4);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(267, 30);
            lblObservaciones.TabIndex = 0;
            lblObservaciones.Text = "OBSERVACIONES:";
            lblObservaciones.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Location = new Point(3, 448);
            button1.Name = "button1";
            button1.Size = new Size(8, 1);
            button1.TabIndex = 5;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // btnRevertir
            // 
            btnRevertir.BackColor = Color.Brown;
            btnRevertir.Cursor = Cursors.Hand;
            btnRevertir.Dock = DockStyle.Fill;
            btnRevertir.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRevertir.ForeColor = SystemColors.Control;
            btnRevertir.Location = new Point(3, 388);
            btnRevertir.Name = "btnRevertir";
            btnRevertir.Size = new Size(261, 54);
            btnRevertir.TabIndex = 6;
            btnRevertir.Text = "Revertir";
            btnRevertir.UseVisualStyleBackColor = false;
            // 
            // EtqsEliminadasEnAlistamiento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(548, 515);
            Controls.Add(tlpRoot);
            Name = "EtqsEliminadasEnAlistamiento";
            Text = "Etiquetas Eliminadas de Alistamiento";
            Load += EtqsEliminadasEnAlistamiento_Load;
            tlpRoot.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInfoEliminacion).EndInit();
            ResumeLayout(false);
        }

        private TableLayoutPanel tlpRoot;
        private ListView lstEtiquetasEliminadas;
        private ColumnHeader colEtiqueta;
        private ColumnHeader colFecha;
        private TableLayoutPanel pnlRight;
        private DataGridView dgvInfoEliminacion;
        private TextBox txtObservaciones;
        private Label lblObservaciones;
        private Button button1;
        private Button btnRevertir;
    }
}