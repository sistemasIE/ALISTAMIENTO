namespace LECTURA_DE_BANDA
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            panel1 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            TXT_USUARIO = new TextBox();
            TXT_CONTRASEÑA = new TextBox();
            label1 = new Label();
            BTN_LOGIN = new Button();
            linkLabel1 = new LinkLabel();
            BTN_CERRAR = new PictureBox();
            BTN_MINIMIZAR = new PictureBox();
            lbl_error = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BTN_CERRAR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BTN_MINIMIZAR).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(291, 382);
            panel1.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = ALISTAMIENTO_IE.Properties.Resources.logo;
            pictureBox2.Location = new Point(55, 189);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(175, 170);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(70, 32);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(130, 117);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // TXT_USUARIO
            // 
            TXT_USUARIO.BackColor = Color.White;
            TXT_USUARIO.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TXT_USUARIO.ForeColor = Color.Gray;
            TXT_USUARIO.Location = new Point(391, 100);
            TXT_USUARIO.Margin = new Padding(4);
            TXT_USUARIO.Name = "TXT_USUARIO";
            TXT_USUARIO.Size = new Size(455, 23);
            TXT_USUARIO.TabIndex = 1;
            TXT_USUARIO.Text = "USUARIO";
            TXT_USUARIO.TextChanged += TXT_USUARIO_TextChanged;
            // 
            // TXT_CONTRASEÑA
            // 
            TXT_CONTRASEÑA.BackColor = Color.White;
            TXT_CONTRASEÑA.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TXT_CONTRASEÑA.ForeColor = Color.Gray;
            TXT_CONTRASEÑA.Location = new Point(391, 148);
            TXT_CONTRASEÑA.Margin = new Padding(4);
            TXT_CONTRASEÑA.Name = "TXT_CONTRASEÑA";
            TXT_CONTRASEÑA.Size = new Size(455, 23);
            TXT_CONTRASEÑA.TabIndex = 2;
            TXT_CONTRASEÑA.Text = "CONTRASEÑA";
            TXT_CONTRASEÑA.KeyDown += TXT_CONTRASEÑA_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(490, 32);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(209, 22);
            label1.TabIndex = 3;
            label1.Text = "LOGIN ALISTAMIENTO";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // BTN_LOGIN
            // 
            BTN_LOGIN.BackColor = Color.WhiteSmoke;
            BTN_LOGIN.FlatAppearance.BorderSize = 0;
            BTN_LOGIN.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 28, 28);
            BTN_LOGIN.FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
            BTN_LOGIN.FlatStyle = FlatStyle.Flat;
            BTN_LOGIN.ForeColor = Color.DimGray;
            BTN_LOGIN.Location = new Point(391, 227);
            BTN_LOGIN.Margin = new Padding(4);
            BTN_LOGIN.Name = "BTN_LOGIN";
            BTN_LOGIN.Size = new Size(455, 46);
            BTN_LOGIN.TabIndex = 3;
            BTN_LOGIN.Text = "ACCEDER";
            BTN_LOGIN.UseVisualStyleBackColor = false;
            BTN_LOGIN.Click += BTN_LOGIN_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.ActiveLinkColor = Color.FromArgb(0, 122, 204);
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.DimGray;
            linkLabel1.Location = new Point(512, 277);
            linkLabel1.Margin = new Padding(4, 0, 4, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(198, 17);
            linkLabel1.TabIndex = 0;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "¿Ha olvidado la contraseña?";
            // 
            // BTN_CERRAR
            // 
            BTN_CERRAR.BackColor = Color.White;
            BTN_CERRAR.Image = (Image)resources.GetObject("BTN_CERRAR.Image");
            BTN_CERRAR.Location = new Point(886, 0);
            BTN_CERRAR.Margin = new Padding(4);
            BTN_CERRAR.Name = "BTN_CERRAR";
            BTN_CERRAR.Size = new Size(24, 22);
            BTN_CERRAR.SizeMode = PictureBoxSizeMode.StretchImage;
            BTN_CERRAR.TabIndex = 6;
            BTN_CERRAR.TabStop = false;
            BTN_CERRAR.Click += BTN_CERRAR_Click;
            // 
            // BTN_MINIMIZAR
            // 
            BTN_MINIMIZAR.Image = (Image)resources.GetObject("BTN_MINIMIZAR.Image");
            BTN_MINIMIZAR.Location = new Point(846, -2);
            BTN_MINIMIZAR.Margin = new Padding(4);
            BTN_MINIMIZAR.Name = "BTN_MINIMIZAR";
            BTN_MINIMIZAR.Size = new Size(34, 23);
            BTN_MINIMIZAR.SizeMode = PictureBoxSizeMode.StretchImage;
            BTN_MINIMIZAR.TabIndex = 7;
            BTN_MINIMIZAR.TabStop = false;
            BTN_MINIMIZAR.Click += BTN_MINIMIZAR_Click;
            // 
            // lbl_error
            // 
            lbl_error.AutoSize = true;
            lbl_error.BackColor = Color.White;
            lbl_error.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_error.ForeColor = Color.Black;
            lbl_error.Image = (Image)resources.GetObject("lbl_error.Image");
            lbl_error.ImageAlign = ContentAlignment.TopLeft;
            lbl_error.Location = new Point(391, 189);
            lbl_error.Margin = new Padding(4, 0, 4, 0);
            lbl_error.Name = "lbl_error";
            lbl_error.Size = new Size(97, 16);
            lbl_error.TabIndex = 8;
            lbl_error.Text = "Mensaje de error";
            lbl_error.Visible = false;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(910, 382);
            Controls.Add(lbl_error);
            Controls.Add(BTN_MINIMIZAR);
            Controls.Add(BTN_CERRAR);
            Controls.Add(linkLabel1);
            Controls.Add(BTN_LOGIN);
            Controls.Add(label1);
            Controls.Add(TXT_CONTRASEÑA);
            Controls.Add(TXT_USUARIO);
            Controls.Add(panel1);
            ForeColor = Color.DimGray;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)BTN_CERRAR).EndInit();
            ((System.ComponentModel.ISupportInitialize)BTN_MINIMIZAR).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox TXT_USUARIO;
        private System.Windows.Forms.TextBox TXT_CONTRASEÑA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BTN_LOGIN;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.PictureBox BTN_CERRAR;
        private System.Windows.Forms.PictureBox BTN_MINIMIZAR;
        private System.Windows.Forms.Label lbl_error;
        private PictureBox pictureBox2;
    }
}