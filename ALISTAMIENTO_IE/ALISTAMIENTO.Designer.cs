namespace ALISTAMIENTO_IE
{
    partial class ALISTAMIENTO
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controles agregados para la interfaz solicitada
        private System.Windows.Forms.Label lblPlaca;
        private System.Windows.Forms.Label lblLlevas;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblEtiqueta;
        private System.Windows.Forms.TextBox txtEtiqueta;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnTerminar; 
        private System.Windows.Forms.GroupBox grpErrores;
        private System.Windows.Forms.ListBox lstErrores;
        private System.Windows.Forms.Label lblNE;
        private System.Windows.Forms.DataGridView dgvLeidos;
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
            lblPlaca = new Label();
            lblLlevas = new Label();
            lblTimer = new Label();
            lblEtiqueta = new Label();
            txtEtiqueta = new TextBox();
            btnBuscar = new Button();
            btnTerminar = new Button();
            grpErrores = new GroupBox();
            label1 = new Label();
            label5 = new Label();
            lstErrores = new ListBox();
            lblNE = new Label();
            dgvLeidos = new DataGridView();
            dgvMain = new DataGridView();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            grpErrores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeidos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // lblPlaca
            // 
            lblPlaca.AutoSize = true;
            lblPlaca.BackColor = Color.Transparent;
            lblPlaca.Dock = DockStyle.Fill;
            lblPlaca.Font = new Font("Segoe UI", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlaca.Location = new Point(7, 0);
            lblPlaca.Margin = new Padding(7, 0, 7, 0);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(251, 66);
            lblPlaca.TabIndex = 0;
            lblPlaca.Text = "PLACA: X";
            lblPlaca.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLlevas
            // 
            lblLlevas.AutoSize = true;
            lblLlevas.BackColor = Color.Transparent;
            lblLlevas.Dock = DockStyle.Fill;
            lblLlevas.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblLlevas.Location = new Point(272, 0);
            lblLlevas.Margin = new Padding(7, 0, 7, 0);
            lblLlevas.Name = "lblLlevas";
            lblLlevas.Size = new Size(227, 66);
            lblLlevas.TabIndex = 1;
            lblLlevas.Text = "LLEVAS:";
            lblLlevas.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.BackColor = Color.Transparent;
            lblTimer.Dock = DockStyle.Fill;
            lblTimer.Font = new Font("Segoe UI", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTimer.ForeColor = Color.MidnightBlue;
            lblTimer.Location = new Point(513, 0);
            lblTimer.Margin = new Padding(7, 0, 7, 0);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(239, 66);
            lblTimer.TabIndex = 2;
            lblTimer.Text = "00:00:00";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEtiqueta
            // 
            lblEtiqueta.AutoSize = true;
            lblEtiqueta.BackColor = Color.Transparent;
            lblEtiqueta.Dock = DockStyle.Fill;
            lblEtiqueta.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            lblEtiqueta.ForeColor = Color.DarkGreen;
            lblEtiqueta.Location = new Point(7, 0);
            lblEtiqueta.Margin = new Padding(7, 0, 7, 0);
            lblEtiqueta.Name = "lblEtiqueta";
            lblEtiqueta.Size = new Size(937, 37);
            lblEtiqueta.TabIndex = 5;
            lblEtiqueta.Text = "ETIQUETA";
            lblEtiqueta.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEtiqueta
            // 
            txtEtiqueta.BackColor = SystemColors.InactiveBorder;
            txtEtiqueta.BorderStyle = BorderStyle.FixedSingle;
            txtEtiqueta.Dock = DockStyle.Fill;
            txtEtiqueta.Font = new Font("Segoe UI", 14F);
            txtEtiqueta.Location = new Point(7, 42);
            txtEtiqueta.Margin = new Padding(7, 5, 7, 5);
            txtEtiqueta.Name = "txtEtiqueta";
            txtEtiqueta.Size = new Size(937, 32);
            txtEtiqueta.TabIndex = 6;
            txtEtiqueta.TextAlign = HorizontalAlignment.Center;
            txtEtiqueta.TextChanged += txtEtiqueta_TextChanged;
            // 
            // btnBuscar
            // 
            btnBuscar.Cursor = Cursors.Hand;
            btnBuscar.Font = new Font("Segoe UI", 15.4F);
            btnBuscar.ForeColor = Color.DarkGreen;
            btnBuscar.Location = new Point(766, 5);
            btnBuscar.Margin = new Padding(7, 5, 7, 5);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(57, 56);
            btnBuscar.TabIndex = 8;
            btnBuscar.Text = "🔍";
            // 
            // btnTerminar
            // 
            btnTerminar.BackColor = Color.Red;
            btnTerminar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnTerminar.ForeColor = Color.White;
            btnTerminar.Location = new Point(1263, 123);
            btnTerminar.Margin = new Padding(7, 5, 7, 5);
            btnTerminar.Name = "btnTerminar";
            btnTerminar.Size = new Size(150, 50);
            btnTerminar.TabIndex = 19;
            btnTerminar.Text = "TERMINAR";
            btnTerminar.UseVisualStyleBackColor = false;
            btnTerminar.Click += BtnTerminar_Click;
            // 
            // grpErrores
            // 
            grpErrores.BackColor = Color.Transparent;
            grpErrores.Controls.Add(label1);
            grpErrores.Controls.Add(label5);
            grpErrores.Controls.Add(lstErrores);
            grpErrores.Controls.Add(lblNE);
            grpErrores.Dock = DockStyle.Fill;
            grpErrores.Font = new Font("Segoe UI", 14F);
            grpErrores.ForeColor = Color.Red;
            grpErrores.Location = new Point(10, 8);
            grpErrores.Margin = new Padding(7, 5, 7, 5);
            grpErrores.Name = "grpErrores";
            grpErrores.Padding = new Padding(7, 5, 7, 5);
            grpErrores.Size = new Size(409, 699);
            grpErrores.TabIndex = 0;
            grpErrores.TabStop = false;
            grpErrores.Text = "Lista de Errores";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(36, 669);
            label1.Margin = new Padding(7, 0, 7, 0);
            label1.Name = "label1";
            label1.Size = new Size(211, 25);
            label1.TabIndex = 4;
            label1.Text = "N.P -> NO PERTENECE";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(36, 606);
            label5.Margin = new Padding(7, 0, 7, 0);
            label5.Name = "label5";
            label5.Size = new Size(174, 25);
            label5.TabIndex = 3;
            label5.Text = "DUP -> Duplicado";
            // 
            // lstErrores
            // 
            lstErrores.Dock = DockStyle.Top;
            lstErrores.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lstErrores.ItemHeight = 21;
            lstErrores.Location = new Point(7, 30);
            lstErrores.Margin = new Padding(7, 5, 7, 5);
            lstErrores.Name = "lstErrores";
            lstErrores.Size = new Size(395, 571);
            lstErrores.TabIndex = 0;
            // 
            // lblNE
            // 
            lblNE.AutoSize = true;
            lblNE.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            lblNE.ForeColor = Color.Black;
            lblNE.Location = new Point(35, 638);
            lblNE.Margin = new Padding(7, 0, 7, 0);
            lblNE.Name = "lblNE";
            lblNE.Size = new Size(156, 25);
            lblNE.TabIndex = 1;
            lblNE.Text = "N.E -> No existe";
            // 
            // dgvLeidos
            // 
            dgvLeidos.AllowUserToAddRows = false;
            dgvLeidos.AllowUserToDeleteRows = false;
            dgvLeidos.AllowUserToOrderColumns = true;
            dgvLeidos.BackgroundColor = Color.Silver;
            dgvLeidos.ColumnHeadersHeight = 29;
            dgvLeidos.Cursor = Cursors.No;
            dgvLeidos.Dock = DockStyle.Fill;
            dgvLeidos.Font = new Font("Segoe UI", 12F);
            dgvLeidos.Location = new Point(7, 372);
            dgvLeidos.Margin = new Padding(7, 5, 7, 5);
            dgvLeidos.Name = "dgvLeidos";
            dgvLeidos.ReadOnly = true;
            dgvLeidos.RowHeadersWidth = 51;
            dgvLeidos.Size = new Size(937, 248);
            dgvLeidos.TabIndex = 13;
            // 
            // dgvMain
            // 
            dgvMain.AllowUserToAddRows = false;
            dgvMain.AllowUserToDeleteRows = false;
            dgvMain.AllowUserToOrderColumns = true;
            dgvMain.BackgroundColor = Color.Silver;
            dgvMain.ColumnHeadersHeight = 29;
            dgvMain.Cursor = Cursors.No;
            dgvMain.Dock = DockStyle.Fill;
            dgvMain.Font = new Font("Segoe UI", 12F);
            dgvMain.GridColor = SystemColors.ScrollBar;
            dgvMain.Location = new Point(7, 134);
            dgvMain.Margin = new Padding(7, 5, 7, 5);
            dgvMain.Name = "dgvMain";
            dgvMain.ReadOnly = true;
            dgvMain.RowHeadersWidth = 51;
            dgvMain.Size = new Size(937, 228);
            dgvMain.TabIndex = 14;
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
            tableLayoutPanel1.Location = new Point(12, 22);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(429, 715);
            tableLayoutPanel1.TabIndex = 20;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.Transparent;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblEtiqueta, 0, 0);
            tableLayoutPanel2.Controls.Add(txtEtiqueta, 0, 1);
            tableLayoutPanel2.Controls.Add(dgvMain, 0, 2);
            tableLayoutPanel2.Controls.Add(dgvLeidos, 0, 3);
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tableLayoutPanel2.Location = new Point(466, 112);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 28.68217F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 71.31783F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 238F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 257F));
            tableLayoutPanel2.Size = new Size(951, 625);
            tableLayoutPanel2.TabIndex = 21;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Transparent;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.3592072F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.6407928F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 253F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 83F));
            tableLayoutPanel3.Controls.Add(lblPlaca, 0, 0);
            tableLayoutPanel3.Controls.Add(lblLlevas, 1, 0);
            tableLayoutPanel3.Controls.Add(lblTimer, 2, 0);
            tableLayoutPanel3.Controls.Add(btnBuscar, 3, 0);
            tableLayoutPanel3.Location = new Point(513, 22);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(843, 66);
            tableLayoutPanel3.TabIndex = 22;
            // 
            // ALISTAMIENTO
            // 
            AutoScaleDimensions = new SizeF(15F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background_IE;
            ClientSize = new Size(1429, 761);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnTerminar);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(tableLayoutPanel3);
            Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
            ForeColor = Color.DarkGreen;
            Margin = new Padding(7, 5, 7, 5);
            Name = "ALISTAMIENTO";
            Text = "ALISTAMIENTO";
            grpErrores.ResumeLayout(false);
            grpErrores.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLeidos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvMain;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
    }
}
