namespace ALISTAMIENTO_IE
{
    partial class ALISTAMIENTO
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblLlevas;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.GroupBox grpErrores;
        private System.Windows.Forms.ListBox lstMensajes;
        private System.Windows.Forms.Label lblNE;
        private System.Windows.Forms.Label lblPedido;

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
            grpErrores = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            label5 = new Label();
            lblNE = new Label();
            label1 = new Label();
            lstMensajes = new ListBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnRecargarUi = new Button();
            btnVerEliminadas = new Button();
            lblTimer = new Label();
            label6 = new Label();
            btnEliminarEtiquetasLeidas = new Button();
            lblPlaca = new Label();
            btnPausa = new Button();
            btnBuscar = new Button();
            btnTerminar = new Button();
            dgvLeidos = new DataGridView();
            dgvMain = new DataGridView();
            lblEtiqueta = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            txtEtiqueta = new TextBox();
            btnManual = new Button();
            grpErrores.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // grpErrores
            // 
            grpErrores.BackColor = Color.Transparent;
            grpErrores.Controls.Add(tableLayoutPanel4);
            grpErrores.Controls.Add(lstMensajes);
            grpErrores.Dock = DockStyle.Fill;
            grpErrores.Font = new Font("Segoe UI", 14F);
            grpErrores.ForeColor = Color.Blue;
            grpErrores.Location = new Point(10, 8);
            grpErrores.Margin = new Padding(7, 5, 7, 5);
            grpErrores.Name = "grpErrores";
            grpErrores.Padding = new Padding(7, 5, 7, 5);
            grpErrores.Size = new Size(329, 574);
            grpErrores.TabIndex = 0;
            grpErrores.TabStop = false;
            grpErrores.Text = "Lista de Mensajes";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(label5, 0, 0);
            tableLayoutPanel4.Controls.Add(lblNE, 0, 1);
            tableLayoutPanel4.Controls.Add(label1, 0, 2);
            tableLayoutPanel4.Location = new Point(25, 462);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel4.Size = new Size(275, 116);
            tableLayoutPanel4.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(7, 0);
            label5.Margin = new Padding(7, 0, 7, 0);
            label5.Name = "label5";
            label5.Size = new Size(261, 37);
            label5.TabIndex = 3;
            label5.Text = "DUP -> Duplicado";
            // 
            // lblNE
            // 
            lblNE.AutoSize = true;
            lblNE.Dock = DockStyle.Fill;
            lblNE.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            lblNE.ForeColor = Color.Black;
            lblNE.Location = new Point(7, 37);
            lblNE.Margin = new Padding(7, 0, 7, 0);
            lblNE.Name = "lblNE";
            lblNE.Size = new Size(261, 37);
            lblNE.TabIndex = 1;
            lblNE.Text = "N.E -> No existe";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(7, 74);
            label1.Margin = new Padding(7, 0, 7, 0);
            label1.Name = "label1";
            label1.Size = new Size(261, 42);
            label1.TabIndex = 4;
            label1.Text = "N.P -> NO PERTENECE";
            // 
            // lstMensajes
            // 
            lstMensajes.Dock = DockStyle.Top;
            lstMensajes.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lstMensajes.ItemHeight = 21;
            lstMensajes.Location = new Point(7, 30);
            lstMensajes.Margin = new Padding(7, 5, 7, 5);
            lstMensajes.Name = "lstMensajes";
            lstMensajes.Size = new Size(315, 424);
            lstMensajes.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.ForeColor = Color.ForestGreen;
            label2.Location = new Point(1098, 927);
            label2.Margin = new Padding(7, 0, 7, 0);
            label2.Name = "label2";
            label2.Size = new Size(56, 21);
            label2.TabIndex = 16;
            label2.Text = "LEÍDO";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            label3.ForeColor = Color.DarkGreen;
            label3.Location = new Point(3112, 283);
            label3.Margin = new Padding(7, 0, 7, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 29);
            label3.TabIndex = 17;
            label3.Text = "Limpiar";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            label4.ForeColor = Color.DarkGreen;
            label4.Location = new Point(3112, 79);
            label4.Margin = new Padding(7, 0, 7, 0);
            label4.Name = "label4";
            label4.Size = new Size(130, 29);
            label4.TabIndex = 18;
            label4.Text = "Búsqueda";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(grpErrores, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Left;
            tableLayoutPanel1.Location = new Point(0, 71);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(349, 590);
            tableLayoutPanel1.TabIndex = 20;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Transparent;
            tableLayoutPanel3.ColumnCount = 9;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.37931F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58.62069F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 79F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 76F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 257F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 178F));
            tableLayoutPanel3.Controls.Add(btnRecargarUi, 4, 0);
            tableLayoutPanel3.Controls.Add(btnVerEliminadas, 5, 0);
            tableLayoutPanel3.Controls.Add(lblTimer, 3, 0);
            tableLayoutPanel3.Controls.Add(label6, 2, 0);
            tableLayoutPanel3.Controls.Add(btnEliminarEtiquetasLeidas, 1, 0);
            tableLayoutPanel3.Controls.Add(lblPlaca, 0, 0);
            tableLayoutPanel3.Controls.Add(btnPausa, 7, 0);
            tableLayoutPanel3.Controls.Add(btnBuscar, 6, 0);
            tableLayoutPanel3.Controls.Add(btnTerminar, 8, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(1284, 71);
            tableLayoutPanel3.TabIndex = 22;
            // 
            // btnRecargarUi
            // 
            btnRecargarUi.Cursor = Cursors.Hand;
            btnRecargarUi.Dock = DockStyle.Fill;
            btnRecargarUi.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRecargarUi.ForeColor = Color.Blue;
            btnRecargarUi.Location = new Point(616, 5);
            btnRecargarUi.Margin = new Padding(7, 5, 7, 5);
            btnRecargarUi.Name = "btnRecargarUi";
            btnRecargarUi.Size = new Size(65, 61);
            btnRecargarUi.TabIndex = 28;
            btnRecargarUi.Text = " ⟳ ";
            btnRecargarUi.Click += button1_Click;
            // 
            // btnVerEliminadas
            // 
            btnVerEliminadas.Cursor = Cursors.Hand;
            btnVerEliminadas.Dock = DockStyle.Fill;
            btnVerEliminadas.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnVerEliminadas.ForeColor = Color.Crimson;
            btnVerEliminadas.Location = new Point(695, 5);
            btnVerEliminadas.Margin = new Padding(7, 5, 7, 5);
            btnVerEliminadas.Name = "btnVerEliminadas";
            btnVerEliminadas.Size = new Size(70, 61);
            btnVerEliminadas.TabIndex = 27;
            btnVerEliminadas.Text = "👁";
            btnVerEliminadas.Click += btnVerEliminadas_Click;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.BackColor = Color.Transparent;
            lblTimer.Dock = DockStyle.Fill;
            lblTimer.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTimer.ForeColor = Color.MidnightBlue;
            lblTimer.Location = new Point(432, 0);
            lblTimer.Margin = new Padding(7, 0, 7, 0);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(170, 71);
            lblTimer.TabIndex = 26;
            lblTimer.Text = "00:00:00";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label6.Location = new Point(302, 0);
            label6.Margin = new Padding(7, 0, 7, 0);
            label6.Name = "label6";
            label6.Size = new Size(116, 71);
            label6.TabIndex = 25;
            label6.Text = "LLEVAS:";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnEliminarEtiquetasLeidas
            // 
            btnEliminarEtiquetasLeidas.Cursor = Cursors.Hand;
            btnEliminarEtiquetasLeidas.Dock = DockStyle.Fill;
            btnEliminarEtiquetasLeidas.Font = new Font("Segoe UI", 15.4F);
            btnEliminarEtiquetasLeidas.ForeColor = Color.Crimson;
            btnEliminarEtiquetasLeidas.Location = new Point(227, 5);
            btnEliminarEtiquetasLeidas.Margin = new Padding(7, 5, 7, 5);
            btnEliminarEtiquetasLeidas.Name = "btnEliminarEtiquetasLeidas";
            btnEliminarEtiquetasLeidas.Size = new Size(61, 61);
            btnEliminarEtiquetasLeidas.TabIndex = 24;
            btnEliminarEtiquetasLeidas.Text = "🗑️";
            btnEliminarEtiquetasLeidas.Click += btnEliminarEtiquetasLeidas_Click;
            // 
            // lblPlaca
            // 
            lblPlaca.AutoSize = true;
            lblPlaca.BackColor = Color.Transparent;
            lblPlaca.Dock = DockStyle.Left;
            lblPlaca.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlaca.Location = new Point(7, 0);
            lblPlaca.Margin = new Padding(7, 0, 7, 0);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(162, 71);
            lblPlaca.TabIndex = 22;
            lblPlaca.Text = "PLACA: X";
            lblPlaca.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPausa
            // 
            btnPausa.BackColor = Color.Teal;
            btnPausa.Cursor = Cursors.WaitCursor;
            btnPausa.Dock = DockStyle.Fill;
            btnPausa.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnPausa.ForeColor = Color.White;
            btnPausa.Location = new Point(855, 5);
            btnPausa.Margin = new Padding(7, 5, 7, 5);
            btnPausa.Name = "btnPausa";
            btnPausa.Size = new Size(243, 61);
            btnPausa.TabIndex = 21;
            btnPausa.Text = "PAUSAR";
            btnPausa.UseVisualStyleBackColor = false;
            btnPausa.Click += btnPausa_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.Dock = DockStyle.Fill;
            btnBuscar.Font = new Font("Segoe UI", 15.4F);
            btnBuscar.ForeColor = Color.DarkGreen;
            btnBuscar.Location = new Point(779, 5);
            btnBuscar.Margin = new Padding(7, 5, 7, 5);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(62, 61);
            btnBuscar.TabIndex = 20;
            btnBuscar.Text = "🔍";
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnTerminar
            // 
            btnTerminar.BackColor = Color.Red;
            btnTerminar.Cursor = Cursors.WaitCursor;
            btnTerminar.Dock = DockStyle.Fill;
            btnTerminar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnTerminar.ForeColor = Color.White;
            btnTerminar.Location = new Point(1112, 5);
            btnTerminar.Margin = new Padding(7, 5, 7, 5);
            btnTerminar.Name = "btnTerminar";
            btnTerminar.Size = new Size(165, 61);
            btnTerminar.TabIndex = 19;
            btnTerminar.Text = "TERMINAR";
            btnTerminar.UseVisualStyleBackColor = false;
            btnTerminar.Click += BtnTerminar_Click;
            // 
            // dgvLeidos
            // 
            dgvLeidos.AllowUserToAddRows = false;
            dgvLeidos.AllowUserToDeleteRows = false;
            dgvLeidos.AllowUserToOrderColumns = true;
            dgvLeidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLeidos.BackgroundColor = Color.Silver;
            dgvLeidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLeidos.Cursor = Cursors.No;
            dgvLeidos.Dock = DockStyle.Fill;
            dgvLeidos.Font = new Font("Segoe UI", 12F);
            dgvLeidos.Location = new Point(7, 393);
            dgvLeidos.Margin = new Padding(7, 5, 7, 5);
            dgvLeidos.Name = "dgvLeidos";
            dgvLeidos.ReadOnly = true;
            dgvLeidos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dgvLeidos.Size = new Size(921, 192);
            dgvLeidos.TabIndex = 13;
            dgvLeidos.SelectionChanged += dgvLeidos_SelectionChanged;
            // 
            // dgvMain
            // 
            dgvMain.AllowUserToAddRows = false;
            dgvMain.AllowUserToDeleteRows = false;
            dgvMain.AllowUserToOrderColumns = true;
            dgvMain.BackgroundColor = Color.Silver;
            dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMain.Cursor = Cursors.No;
            dgvMain.Dock = DockStyle.Fill;
            dgvMain.Font = new Font("Segoe UI", 12F);
            dgvMain.GridColor = SystemColors.ScrollBar;
            dgvMain.Location = new Point(7, 137);
            dgvMain.Margin = new Padding(7, 5, 7, 5);
            dgvMain.Name = "dgvMain";
            dgvMain.ReadOnly = true;
            dgvMain.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dgvMain.Size = new Size(921, 246);
            dgvMain.TabIndex = 14;
            // 
            // lblEtiqueta
            // 
            lblEtiqueta.AutoSize = true;
            lblEtiqueta.BackColor = Color.Transparent;
            lblEtiqueta.Dock = DockStyle.Fill;
            lblEtiqueta.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEtiqueta.ForeColor = Color.DarkGreen;
            lblEtiqueta.Location = new Point(7, 0);
            lblEtiqueta.Margin = new Padding(7, 0, 7, 0);
            lblEtiqueta.Name = "lblEtiqueta";
            lblEtiqueta.Size = new Size(921, 55);
            lblEtiqueta.TabIndex = 5;
            lblEtiqueta.Text = "ETIQUETA";
            lblEtiqueta.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Transparent;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblEtiqueta, 0, 0);
            tableLayoutPanel2.Controls.Add(dgvMain, 0, 2);
            tableLayoutPanel2.Controls.Add(dgvLeidos, 0, 3);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tableLayoutPanel2.Location = new Point(349, 71);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 41.48936F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 58.51064F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 256F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 201F));
            tableLayoutPanel2.Size = new Size(935, 590);
            tableLayoutPanel2.TabIndex = 21;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 87.19577F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.8042326F));
            tableLayoutPanel5.Controls.Add(txtEtiqueta, 0, 0);
            tableLayoutPanel5.Controls.Add(btnManual, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 58);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(929, 71);
            tableLayoutPanel5.TabIndex = 15;
            // 
            // txtEtiqueta
            // 
            txtEtiqueta.BackColor = SystemColors.InactiveBorder;
            txtEtiqueta.BorderStyle = BorderStyle.FixedSingle;
            txtEtiqueta.Dock = DockStyle.Fill;
            txtEtiqueta.Enabled = false;
            txtEtiqueta.Font = new Font("Segoe UI", 14F);
            txtEtiqueta.Location = new Point(7, 5);
            txtEtiqueta.Margin = new Padding(7, 5, 7, 5);
            txtEtiqueta.Name = "txtEtiqueta";
            txtEtiqueta.Size = new Size(796, 32);
            txtEtiqueta.TabIndex = 7;
            txtEtiqueta.TextAlign = HorizontalAlignment.Center;
            txtEtiqueta.TextChanged += txtEtiqueta_TextChanged;
            // 
            // btnManual
            // 
            btnManual.Dock = DockStyle.Fill;
            btnManual.Location = new Point(813, 3);
            btnManual.Name = "btnManual";
            btnManual.Size = new Size(113, 65);
            btnManual.TabIndex = 8;
            btnManual.Text = "Manual";
            btnManual.UseVisualStyleBackColor = true;
            // 
            // ALISTAMIENTO
            // 
            AutoScaleDimensions = new SizeF(15F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background_IE;
            ClientSize = new Size(1284, 661);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(tableLayoutPanel3);
            Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            ForeColor = Color.DarkGreen;
            Margin = new Padding(7, 5, 7, 5);
            Name = "ALISTAMIENTO";
            Text = "ALISTAMIENTO";
            WindowState = FormWindowState.Maximized;
            Load += ALISTAMIENTO_Load_1;
            grpErrores.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnPausa;
        private Button btnBuscar;
        private Button btnTerminar;
        private Button btnEliminarEtiquetasLeidas;
        private Label lblPlaca;
        private Button btnVerEliminadas;
        private Label label6;
        private DataGridView dgvLeidos;
        private DataGridView dgvMain;
        private Label lblEtiqueta;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel5;
        private TextBox txtEtiqueta;
        private Button btnManual;
        private Button btnRecargarUi;
    }
}
