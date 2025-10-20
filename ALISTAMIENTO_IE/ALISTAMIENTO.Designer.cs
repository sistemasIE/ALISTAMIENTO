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
            grpErrores = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            label5 = new Label();
            lblNE = new Label();
            label1 = new Label();
            lstErrores = new ListBox();
            dgvLeidos = new DataGridView();
            dgvMain = new DataGridView();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnPausa = new Button();
            btnBuscar = new Button();
            btnTerminar = new Button();
            grpErrores.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
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
            lblPlaca.Dock = DockStyle.Left;
            lblPlaca.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlaca.Location = new Point(7, 0);
            lblPlaca.Margin = new Padding(7, 0, 7, 0);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(162, 71);
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
            lblLlevas.Location = new Point(368, 0);
            lblLlevas.Margin = new Padding(7, 0, 7, 0);
            lblLlevas.Name = "lblLlevas";
            lblLlevas.Size = new Size(132, 71);
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
            lblTimer.Location = new Point(514, 0);
            lblTimer.Margin = new Padding(7, 0, 7, 0);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(173, 71);
            lblTimer.TabIndex = 2;
            lblTimer.Text = "00:00:00";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
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
            lblEtiqueta.Size = new Size(937, 55);
            lblEtiqueta.TabIndex = 5;
            lblEtiqueta.Text = "ETIQUETA";
            lblEtiqueta.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEtiqueta
            // 
            txtEtiqueta.BackColor = SystemColors.InactiveBorder;
            txtEtiqueta.BorderStyle = BorderStyle.FixedSingle;
            txtEtiqueta.Dock = DockStyle.Fill;
            txtEtiqueta.Enabled = false;
            txtEtiqueta.Font = new Font("Segoe UI", 14F);
            txtEtiqueta.Location = new Point(7, 60);
            txtEtiqueta.Margin = new Padding(7, 5, 7, 5);
            txtEtiqueta.Name = "txtEtiqueta";
            txtEtiqueta.Size = new Size(937, 32);
            txtEtiqueta.TabIndex = 6;
            txtEtiqueta.TextAlign = HorizontalAlignment.Center;
            txtEtiqueta.TextChanged += txtEtiqueta_TextChanged;
            // 
            // grpErrores
            // 
            grpErrores.BackColor = Color.Transparent;
            grpErrores.Controls.Add(tableLayoutPanel4);
            grpErrores.Controls.Add(lstErrores);
            grpErrores.Dock = DockStyle.Fill;
            grpErrores.Font = new Font("Segoe UI", 14F);
            grpErrores.ForeColor = Color.Red;
            grpErrores.Location = new Point(10, 8);
            grpErrores.Margin = new Padding(7, 5, 7, 5);
            grpErrores.Name = "grpErrores";
            grpErrores.Padding = new Padding(7, 5, 7, 5);
            grpErrores.Size = new Size(329, 574);
            grpErrores.TabIndex = 0;
            grpErrores.TabStop = false;
            grpErrores.Text = "Lista de Errores";
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
            // lstErrores
            // 
            lstErrores.Dock = DockStyle.Top;
            lstErrores.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lstErrores.ItemHeight = 21;
            lstErrores.Location = new Point(7, 30);
            lstErrores.Margin = new Padding(7, 5, 7, 5);
            lstErrores.Name = "lstErrores";
            lstErrores.Size = new Size(315, 424);
            lstErrores.TabIndex = 0;
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
            dgvLeidos.Size = new Size(937, 192);
            dgvLeidos.TabIndex = 13;
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
            dgvMain.Size = new Size(937, 246);
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
            tableLayoutPanel1.Dock = DockStyle.Left;
            tableLayoutPanel1.Location = new Point(0, 71);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(349, 590);
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
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tableLayoutPanel2.Location = new Point(349, 71);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 41.48936F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 58.51064F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 256F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 201F));
            tableLayoutPanel2.Size = new Size(951, 590);
            tableLayoutPanel2.TabIndex = 21;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Transparent;
            tableLayoutPanel3.ColumnCount = 6;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.2031555F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.7968445F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 187F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 251F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 259F));
            tableLayoutPanel3.Controls.Add(btnPausa, 4, 0);
            tableLayoutPanel3.Controls.Add(btnBuscar, 3, 0);
            tableLayoutPanel3.Controls.Add(lblPlaca, 0, 0);
            tableLayoutPanel3.Controls.Add(lblLlevas, 1, 0);
            tableLayoutPanel3.Controls.Add(btnTerminar, 5, 0);
            tableLayoutPanel3.Controls.Add(lblTimer, 2, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(1300, 71);
            tableLayoutPanel3.TabIndex = 22;
            // 
            // btnPausa
            // 
            btnPausa.BackColor = Color.Teal;
            btnPausa.Cursor = Cursors.WaitCursor;
            btnPausa.Dock = DockStyle.Fill;
            btnPausa.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnPausa.ForeColor = Color.White;
            btnPausa.Location = new Point(796, 5);
            btnPausa.Margin = new Padding(7, 5, 7, 5);
            btnPausa.Name = "btnPausa";
            btnPausa.Size = new Size(237, 61);
            btnPausa.TabIndex = 21;
            btnPausa.Text = "PAUSAR";
            btnPausa.UseVisualStyleBackColor = false;
            btnPausa.Click += btnPausa_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Cursor = Cursors.WaitCursor;
            btnBuscar.Dock = DockStyle.Fill;
            btnBuscar.Font = new Font("Segoe UI", 15.4F);
            btnBuscar.ForeColor = Color.DarkGreen;
            btnBuscar.Location = new Point(701, 5);
            btnBuscar.Margin = new Padding(7, 5, 7, 5);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(81, 61);
            btnBuscar.TabIndex = 20;
            btnBuscar.Text = "🔍";
            // 
            // btnTerminar
            // 
            btnTerminar.BackColor = Color.Red;
            btnTerminar.Cursor = Cursors.WaitCursor;
            btnTerminar.Dock = DockStyle.Fill;
            btnTerminar.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnTerminar.ForeColor = Color.White;
            btnTerminar.Location = new Point(1047, 5);
            btnTerminar.Margin = new Padding(7, 5, 7, 5);
            btnTerminar.Name = "btnTerminar";
            btnTerminar.Size = new Size(246, 61);
            btnTerminar.TabIndex = 19;
            btnTerminar.Text = "TERMINAR";
            btnTerminar.UseVisualStyleBackColor = false;
            btnTerminar.Click += BtnTerminar_Click;
            // 
            // ALISTAMIENTO
            // 
            AutoScaleDimensions = new SizeF(15F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background_IE;
            ClientSize = new Size(1300, 661);
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
            grpErrores.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
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
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnPausa;
        private Button btnBuscar;
        private Button btnTerminar;
    }
}
