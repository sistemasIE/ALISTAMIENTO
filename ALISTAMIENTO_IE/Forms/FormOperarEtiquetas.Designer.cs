namespace ALISTAMIENTO_IE.Forms
{
    partial class FormOperarEtiquetas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblPlaca = new Label();
            txtBoxEtiquetas = new TextBox();
            btnEjecutar = new Button();
            button1 = new Button();
            tlpMain = new TableLayoutPanel();
            flpButtons = new FlowLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tlpMain.SuspendLayout();
            flpButtons.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblPlaca
            // 
            lblPlaca.AutoSize = true;
            lblPlaca.Dock = DockStyle.Fill;
            lblPlaca.Font = new Font("Segoe UI Variable Display", 28.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlaca.Location = new Point(12, 10);
            lblPlaca.Margin = new Padding(0, 0, 0, 8);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(431, 51);
            lblPlaca.TabIndex = 0;
            lblPlaca.Text = "OPERAR ETIQUETAS";
            lblPlaca.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtBoxEtiquetas
            // 
            txtBoxEtiquetas.Dock = DockStyle.Fill;
            txtBoxEtiquetas.Font = new Font("Consolas", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBoxEtiquetas.Location = new Point(12, 69);
            txtBoxEtiquetas.Margin = new Padding(0, 0, 0, 12);
            txtBoxEtiquetas.Multiline = true;
            txtBoxEtiquetas.Name = "txtBoxEtiquetas";
            txtBoxEtiquetas.ScrollBars = ScrollBars.Vertical;
            txtBoxEtiquetas.Size = new Size(431, 289);
            txtBoxEtiquetas.TabIndex = 1;
            // 
            // btnEjecutar
            // 
            btnEjecutar.BackColor = Color.ForestGreen;
            btnEjecutar.Dock = DockStyle.Fill;
            btnEjecutar.FlatStyle = FlatStyle.Flat;
            btnEjecutar.Font = new Font("Segoe UI Variable Display", 13.8F, FontStyle.Bold);
            btnEjecutar.ForeColor = Color.White;
            btnEjecutar.Location = new Point(228, 0);
            btnEjecutar.Margin = new Padding(8, 0, 0, 0);
            btnEjecutar.MinimumSize = new Size(120, 45);
            btnEjecutar.Name = "btnEjecutar";
            btnEjecutar.Size = new Size(212, 57);
            btnEjecutar.TabIndex = 0;
            btnEjecutar.Text = "Ejecutar";
            btnEjecutar.UseVisualStyleBackColor = false;
            btnEjecutar.Click += btnEjecutar_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Firebrick;
            button1.Dock = DockStyle.Fill;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Variable Display", 13.8F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(8, 0);
            button1.Margin = new Padding(8, 0, 0, 0);
            button1.MinimumSize = new Size(120, 45);
            button1.Name = "button1";
            button1.Size = new Size(212, 57);
            button1.TabIndex = 1;
            button1.Text = "Cancelar";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnCancelar_Click;
            // 
            // tlpMain
            // 
            tlpMain.BackColor = Color.WhiteSmoke;
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(lblPlaca, 0, 0);
            tlpMain.Controls.Add(txtBoxEtiquetas, 0, 1);
            tlpMain.Controls.Add(flpButtons, 0, 2);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.Name = "tlpMain";
            tlpMain.Padding = new Padding(12, 10, 12, 10);
            tlpMain.RowCount = 3;
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.Size = new Size(455, 480);
            tlpMain.TabIndex = 0;
            // 
            // flpButtons
            // 
            flpButtons.Controls.Add(tableLayoutPanel1);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Location = new Point(12, 370);
            flpButtons.Margin = new Padding(0);
            flpButtons.Name = "flpButtons";
            flpButtons.Size = new Size(431, 100);
            flpButtons.TabIndex = 2;
            flpButtons.WrapContents = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btnEjecutar, 1, 0);
            tableLayoutPanel1.Controls.Add(button1, 0, 0);
            tableLayoutPanel1.Location = new Point(-12, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(440, 57);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // FormOperarEtiquetas
            // 
            AcceptButton = btnEjecutar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = button1;
            ClientSize = new Size(455, 480);
            Controls.Add(tlpMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormOperarEtiquetas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Operar Etiquetas";
            Load += FormOperarEtiquetas_Load;
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            flpButtons.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblPlaca;
        private TextBox txtBoxEtiquetas;
        private Button btnEjecutar;
        private Button button1; // cancel
        private TableLayoutPanel tlpMain;
        private FlowLayoutPanel flpButtons;
        private TableLayoutPanel tableLayoutPanel1;
    }
}