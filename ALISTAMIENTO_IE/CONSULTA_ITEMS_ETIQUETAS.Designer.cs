namespace LECTURA_DE_BANDA
{
    partial class CONSULTA_ITEMS_ETIQUETAS
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
            label1 = new Label();
            txtItems = new TextBox();
            btnBuscarEtiquetado = new Button();
            dgv = new DataGridView();
            btnImprimirEtiquetado = new Button();
            btnLimpiar = new Button();
            btnExportarExcel = new Button();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlText;
            label1.Location = new Point(46, 64);
            label1.Name = "label1";
            label1.Size = new Size(250, 18);
            label1.TabIndex = 0;
            label1.Text = "Escriba aquí los ITEMS necesitados:";
            // 
            // txtItems
            // 
            txtItems.Font = new Font("Segoe UI", 12F);
            txtItems.Location = new Point(40, 105);
            txtItems.Margin = new Padding(4, 4, 4, 4);
            txtItems.Multiline = true;
            txtItems.Name = "txtItems";
            txtItems.ScrollBars = ScrollBars.Vertical;
            txtItems.Size = new Size(316, 382);
            txtItems.TabIndex = 2;
            // 
            // btnBuscarEtiquetado
            // 
            btnBuscarEtiquetado.BackColor = Color.ForestGreen;
            btnBuscarEtiquetado.Cursor = Cursors.Hand;
            btnBuscarEtiquetado.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnBuscarEtiquetado.ForeColor = SystemColors.ButtonHighlight;
            btnBuscarEtiquetado.Location = new Point(182, 506);
            btnBuscarEtiquetado.Margin = new Padding(4, 4, 4, 4);
            btnBuscarEtiquetado.Name = "btnBuscarEtiquetado";
            btnBuscarEtiquetado.Size = new Size(108, 40);
            btnBuscarEtiquetado.TabIndex = 5;
            btnBuscarEtiquetado.Text = "Buscar 🔍️";
            btnBuscarEtiquetado.UseVisualStyleBackColor = false;
            // 
            // dgv
            // 
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new Point(403, 105);
            dgv.Name = "dgv";
            dgv.RowHeadersWidth = 51;
            dgv.RowTemplate.Height = 24;
            dgv.Size = new Size(717, 382);
            dgv.TabIndex = 6;
            // 
            // btnImprimirEtiquetado
            // 
            btnImprimirEtiquetado.BackColor = Color.Olive;
            btnImprimirEtiquetado.Cursor = Cursors.Hand;
            btnImprimirEtiquetado.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnImprimirEtiquetado.ForeColor = SystemColors.ButtonHighlight;
            btnImprimirEtiquetado.Location = new Point(880, 506);
            btnImprimirEtiquetado.Margin = new Padding(4, 4, 4, 4);
            btnImprimirEtiquetado.Name = "btnImprimirEtiquetado";
            btnImprimirEtiquetado.Size = new Size(108, 40);
            btnImprimirEtiquetado.TabIndex = 7;
            btnImprimirEtiquetado.Text = "Imprimir";
            btnImprimirEtiquetado.UseVisualStyleBackColor = false;
            btnImprimirEtiquetado.Click += btnImprimirEtiquetado_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLimpiar.Location = new Point(40, 506);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(108, 40);
            btnLimpiar.TabIndex = 8;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnExportarExcel
            // 
            btnExportarExcel.BackColor = Color.LimeGreen;
            btnExportarExcel.Cursor = Cursors.Hand;
            btnExportarExcel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnExportarExcel.ForeColor = SystemColors.ControlLightLight;
            btnExportarExcel.Location = new Point(606, 506);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.Size = new Size(180, 40);
            btnExportarExcel.TabIndex = 10;
            btnExportarExcel.Text = "Exportar Excel";
            btnExportarExcel.UseVisualStyleBackColor = false;
            btnExportarExcel.Click += btnExportarExcel_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Rockwell", 28.2F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.RoyalBlue;
            label2.Location = new Point(326, 8);
            label2.Name = "label2";
            label2.Size = new Size(249, 48);
            label2.TabIndex = 11;
            label2.Text = "CONSULTAS";
            // 
            // CONSULTA_ITEMS_ETIQUETAS
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1154, 610);
            Controls.Add(label2);
            Controls.Add(btnExportarExcel);
            Controls.Add(btnLimpiar);
            Controls.Add(btnImprimirEtiquetado);
            Controls.Add(dgv);
            Controls.Add(btnBuscarEtiquetado);
            Controls.Add(txtItems);
            Controls.Add(label1);
            Name = "CONSULTA_ITEMS_ETIQUETAS";
            Text = "FORM_CONSULTA_ITEMS";
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItems;
        private System.Windows.Forms.Button btnBuscarEtiquetado;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnImprimirEtiquetado;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Label label2;
    }
}