namespace ALISTAMIENTO_IE
{
    partial class OBSERVACIONES
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
            lblDinamico = new Label();
            txtObservaciones = new TextBox();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblDinamico
            // 
            lblDinamico.AutoSize = true;
            lblDinamico.Location = new Point(26, 22);
            lblDinamico.Name = "lblDinamico";
            lblDinamico.Size = new Size(44, 15);
            lblDinamico.TabIndex = 0;
            lblDinamico.Text = "Acción";
            // 
            // txtObservaciones
            // 
            txtObservaciones.Location = new Point(26, 52);
            txtObservaciones.Margin = new Padding(3, 2, 3, 2);
            txtObservaciones.MaxLength = 255;
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.Size = new Size(280, 91);
            txtObservaciones.TabIndex = 1;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(52, 165);
            btnAceptar.Margin = new Padding(3, 2, 3, 2);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(88, 26);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(175, 165);
            btnCancelar.Margin = new Padding(3, 2, 3, 2);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 26);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // OBSERVACIONES
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(333, 225);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(txtObservaciones);
            Controls.Add(lblDinamico);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "OBSERVACIONES";
            Text = "OBSERVACIONES";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDinamico;
        private TextBox txtObservaciones;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}