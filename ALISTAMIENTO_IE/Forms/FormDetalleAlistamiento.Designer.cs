using System.Drawing;
using System.Windows.Forms;

namespace ALISTAMIENTO_IE.Forms
{
    partial class FormDetalleAlistamiento
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
            tlpRoot = new TableLayoutPanel();
            lblTitulo = new Label();
            tlpTop = new TableLayoutPanel();
            grpStats = new GroupBox();
            tlpStats = new TableLayoutPanel();
            lblTotalEtiquetasTitulo = new Label();
            lblTotalEtiquetas = new Label();
            lblTotalItemsTitulo = new Label();
            lblTotalItems = new Label();
            tlpSearch = new TableLayoutPanel();
            lblBuscar = new Label();
            txtBuscarItem = new TextBox();
            grpObservaciones = new GroupBox();
            txtObservaciones = new TextBox();
            dgvEtiquetas = new DataGridView();
            tlpRoot.SuspendLayout();
            tlpTop.SuspendLayout();
            grpStats.SuspendLayout();
            tlpStats.SuspendLayout();
            tlpSearch.SuspendLayout();
            grpObservaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEtiquetas).BeginInit();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.BackColor = Color.White;
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(lblTitulo, 0, 0);
            tlpRoot.Controls.Add(tlpTop, 0, 1);
            tlpRoot.Controls.Add(dgvEtiquetas, 0, 2);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(10);
            tlpRoot.RowCount = 3;
            tlpRoot.RowStyles.Add(new RowStyle());
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 170F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.Size = new Size(701, 492);
            tlpRoot.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(0, 80, 180);
            lblTitulo.Location = new Point(10, 10);
            lblTitulo.Margin = new Padding(0, 0, 0, 8);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(681, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "DETALLE DE ALISTAMIENTO";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tlpTop
            // 
            tlpTop.ColumnCount = 2;
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tlpTop.Controls.Add(grpStats, 0, 0);
            tlpTop.Controls.Add(grpObservaciones, 1, 0);
            tlpTop.Dock = DockStyle.Fill;
            tlpTop.Location = new Point(13, 62);
            tlpTop.Name = "tlpTop";
            tlpTop.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpTop.Size = new Size(675, 164);
            tlpTop.TabIndex = 1;
            // 
            // grpStats
            // 
            grpStats.BackColor = Color.White;
            grpStats.Controls.Add(tlpStats);
            grpStats.Dock = DockStyle.Fill;
            grpStats.ForeColor = Color.FromArgb(0, 80, 180);
            grpStats.Location = new Point(3, 3);
            grpStats.Name = "grpStats";
            grpStats.Padding = new Padding(8);
            grpStats.Size = new Size(297, 158);
            grpStats.TabIndex = 0;
            grpStats.TabStop = false;
            grpStats.Text = "Estadísticas y Búsqueda";
            // 
            // tlpStats
            // 
            tlpStats.ColumnCount = 2;
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpStats.Controls.Add(lblTotalEtiquetasTitulo, 0, 0);
            tlpStats.Controls.Add(lblTotalEtiquetas, 1, 0);
            tlpStats.Controls.Add(lblTotalItemsTitulo, 0, 1);
            tlpStats.Controls.Add(lblTotalItems, 1, 1);
            tlpStats.Controls.Add(tlpSearch, 0, 2);
            tlpStats.Dock = DockStyle.Fill;
            tlpStats.Location = new Point(8, 24);
            tlpStats.Name = "tlpStats";
            tlpStats.RowCount = 3;
            tlpStats.RowStyles.Add(new RowStyle());
            tlpStats.RowStyles.Add(new RowStyle());
            tlpStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpStats.Size = new Size(281, 126);
            tlpStats.TabIndex = 0;
            // 
            // lblTotalEtiquetasTitulo
            // 
            lblTotalEtiquetasTitulo.AutoSize = true;
            lblTotalEtiquetasTitulo.Dock = DockStyle.Fill;
            lblTotalEtiquetasTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalEtiquetasTitulo.ForeColor = Color.Black;
            lblTotalEtiquetasTitulo.Location = new Point(3, 0);
            lblTotalEtiquetasTitulo.Name = "lblTotalEtiquetasTitulo";
            lblTotalEtiquetasTitulo.Size = new Size(134, 25);
            lblTotalEtiquetasTitulo.TabIndex = 0;
            lblTotalEtiquetasTitulo.Text = "# Etiquetas:";
            // 
            // lblTotalEtiquetas
            // 
            lblTotalEtiquetas.AutoSize = true;
            lblTotalEtiquetas.Dock = DockStyle.Fill;
            lblTotalEtiquetas.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalEtiquetas.ForeColor = Color.FromArgb(0, 150, 70);
            lblTotalEtiquetas.Location = new Point(143, 0);
            lblTotalEtiquetas.Name = "lblTotalEtiquetas";
            lblTotalEtiquetas.Size = new Size(135, 25);
            lblTotalEtiquetas.TabIndex = 1;
            lblTotalEtiquetas.Text = "0";
            // 
            // lblTotalItemsTitulo
            // 
            lblTotalItemsTitulo.AutoSize = true;
            lblTotalItemsTitulo.Dock = DockStyle.Fill;
            lblTotalItemsTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalItemsTitulo.ForeColor = Color.Black;
            lblTotalItemsTitulo.Location = new Point(3, 25);
            lblTotalItemsTitulo.Name = "lblTotalItemsTitulo";
            lblTotalItemsTitulo.Size = new Size(134, 25);
            lblTotalItemsTitulo.TabIndex = 2;
            lblTotalItemsTitulo.Text = "# Items:";
            // 
            // lblTotalItems
            // 
            lblTotalItems.AutoSize = true;
            lblTotalItems.Dock = DockStyle.Fill;
            lblTotalItems.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalItems.ForeColor = Color.FromArgb(0, 150, 70);
            lblTotalItems.Location = new Point(143, 25);
            lblTotalItems.Name = "lblTotalItems";
            lblTotalItems.Size = new Size(135, 25);
            lblTotalItems.TabIndex = 3;
            lblTotalItems.Text = "0";
            // 
            // tlpSearch
            // 
            tlpSearch.ColumnCount = 2;
            tlpStats.SetColumnSpan(tlpSearch, 2);
            tlpSearch.ColumnStyles.Add(new ColumnStyle());
            tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSearch.Controls.Add(lblBuscar, 0, 0);
            tlpSearch.Controls.Add(txtBuscarItem, 1, 0);
            tlpSearch.Dock = DockStyle.Fill;
            tlpSearch.Location = new Point(3, 53);
            tlpSearch.Name = "tlpSearch";
            tlpSearch.Padding = new Padding(0, 8, 0, 0);
            tlpSearch.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpSearch.Size = new Size(275, 70);
            tlpSearch.TabIndex = 4;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Dock = DockStyle.Fill;
            lblBuscar.Font = new Font("Segoe UI", 10F);
            lblBuscar.ForeColor = Color.Black;
            lblBuscar.Location = new Point(3, 8);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(84, 62);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar Item:";
            // 
            // txtBuscarItem
            // 
            txtBuscarItem.Dock = DockStyle.Fill;
            txtBuscarItem.Font = new Font("Segoe UI", 11F);
            txtBuscarItem.Location = new Point(93, 11);
            txtBuscarItem.Name = "txtBuscarItem";
            txtBuscarItem.Size = new Size(179, 27);
            txtBuscarItem.TabIndex = 1;
            // 
            // grpObservaciones
            // 
            grpObservaciones.BackColor = Color.White;
            grpObservaciones.Controls.Add(txtObservaciones);
            grpObservaciones.Dock = DockStyle.Fill;
            grpObservaciones.ForeColor = Color.FromArgb(0, 80, 180);
            grpObservaciones.Location = new Point(306, 3);
            grpObservaciones.Name = "grpObservaciones";
            grpObservaciones.Padding = new Padding(8);
            grpObservaciones.Size = new Size(366, 158);
            grpObservaciones.TabIndex = 1;
            grpObservaciones.TabStop = false;
            grpObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Font = new Font("Consolas", 10F);
            txtObservaciones.Location = new Point(8, 24);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ReadOnly = true;
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(350, 126);
            txtObservaciones.TabIndex = 0;
            // 
            // dgvEtiquetas
            // 
            dgvEtiquetas.AllowUserToAddRows = false;
            dgvEtiquetas.AllowUserToDeleteRows = false;
            dgvEtiquetas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEtiquetas.BackgroundColor = Color.White;
            dgvEtiquetas.Dock = DockStyle.Fill;
            dgvEtiquetas.Location = new Point(13, 232);
            dgvEtiquetas.Name = "dgvEtiquetas";
            dgvEtiquetas.ReadOnly = true;
            dgvEtiquetas.RowHeadersVisible = false;
            dgvEtiquetas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEtiquetas.Size = new Size(675, 247);
            dgvEtiquetas.TabIndex = 2;
            // 
            // FormDetalleAlistamiento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(701, 492);
            Controls.Add(tlpRoot);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDetalleAlistamiento";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Detalle Alistamiento";
            Load += FormDetalleAlistamiento_Load;
            tlpRoot.ResumeLayout(false);
            tlpRoot.PerformLayout();
            tlpTop.ResumeLayout(false);
            grpStats.ResumeLayout(false);
            tlpStats.ResumeLayout(false);
            tlpStats.PerformLayout();
            tlpSearch.ResumeLayout(false);
            tlpSearch.PerformLayout();
            grpObservaciones.ResumeLayout(false);
            grpObservaciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEtiquetas).EndInit();
            ResumeLayout(false);
        }

        private TableLayoutPanel tlpRoot;
        private Label lblTitulo;
        private TableLayoutPanel tlpTop;
        private GroupBox grpStats;
        private TableLayoutPanel tlpStats;
        private Label lblTotalEtiquetasTitulo;
        private Label lblTotalEtiquetas;
        private Label lblTotalItemsTitulo;
        private Label lblTotalItems;
        private TableLayoutPanel tlpSearch;
        private Label lblBuscar;
        private TextBox txtBuscarItem;
        private GroupBox grpObservaciones;
        private TextBox txtObservaciones;
        private DataGridView dgvEtiquetas;
    }
}