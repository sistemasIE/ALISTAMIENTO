namespace ALISTAMIENTO_IE
{
    partial class Menu
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
            tabMain = new TabControl();
            tabAlistar = new TabPage();
            splMain = new SplitContainer();
            tlpLeft = new TableLayoutPanel();
            lblListasTitulo = new Label();
            lvwListasCamiones = new ListView();
            colPlaca = new ColumnHeader();
            colFecha = new ColumnHeader();
            colCantidad = new ColumnHeader();
            grpDetalleCamion = new GroupBox();
            tlpRight = new TableLayoutPanel();
            lblFechaTitulo = new Label();
            lblFechaValor = new Label();
            lblItemsTitulo = new Label();
            dgvItems = new DataGridView();
            lblTituloCamion = new Label();
            tlpBotones = new TableLayoutPanel();
            btnImprimir = new Button();
            btnVerMas = new Button();
            btnRecargar = new Button();
            btnAlistar = new Button();
            tabReportes = new TabPage();
            tlpReportes = new TableLayoutPanel();
            tlpFiltros = new TableLayoutPanel();
            lblEscogeFecha = new Label();
            grpFecha = new GroupBox();
            tlpFecha = new TableLayoutPanel();
            dtpFechaReporte = new DateTimePicker();
            lblFechaTituloRpt = new Label();
            tlpResumenHost = new TableLayoutPanel();
            tbcTurnos = new TabControl();
            tabTotal = new TabPage();
            tabTurno1 = new TabPage();
            tabTurno2 = new TabPage();
            tabTurno3 = new TabPage();
            grpResumen = new GroupBox();
            tlpResumen = new TableLayoutPanel();
            dgvResumen = new DataGridView();
            pnlKpis = new Panel();
            lblCamionesTexto = new Label();
            lblCamionesNumero = new Label();
            lblUnidadesTexto = new Label();
            lblUnidadesPacas = new Label();
            dgvMovimientos = new DataGridView();
            tabCargueMasivo = new TabPage();
            label4 = new Label();
            lstErrores = new ListBox();
            label3 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            lblProgreso = new Label();
            dtgAgrupada = new DataGridView();
            btnCargarArchivo = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnCrearCamiones = new Button();
            progressBar1 = new ProgressBar();
            dtgCargueMasivo = new DataGridView();
            tabAdmonCamiones = new TabPage();
            lstCamiones = new CheckedListBox();
            btnCerrarCamion = new Button();
            dataGridView1 = new DataGridView();
            btnExportar = new Button();
            tabMain.SuspendLayout();
            tabAlistar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splMain).BeginInit();
            splMain.Panel1.SuspendLayout();
            splMain.Panel2.SuspendLayout();
            splMain.SuspendLayout();
            tlpLeft.SuspendLayout();
            grpDetalleCamion.SuspendLayout();
            tlpRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            tlpBotones.SuspendLayout();
            tabReportes.SuspendLayout();
            tlpReportes.SuspendLayout();
            tlpFiltros.SuspendLayout();
            grpFecha.SuspendLayout();
            tlpFecha.SuspendLayout();
            tlpResumenHost.SuspendLayout();
            tbcTurnos.SuspendLayout();
            grpResumen.SuspendLayout();
            tlpResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResumen).BeginInit();
            pnlKpis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).BeginInit();
            tabCargueMasivo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtgAgrupada).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtgCargueMasivo).BeginInit();
            tabAdmonCamiones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabAlistar);
            tabMain.Controls.Add(tabReportes);
            tabMain.Controls.Add(tabCargueMasivo);
            tabMain.Controls.Add(tabAdmonCamiones);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 0);
            tabMain.Margin = new Padding(2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1210, 516);
            tabMain.TabIndex = 0;
            // 
            // tabAlistar
            // 
            tabAlistar.Controls.Add(splMain);
            tabAlistar.Location = new Point(4, 30);
            tabAlistar.Margin = new Padding(2);
            tabAlistar.Name = "tabAlistar";
            tabAlistar.Padding = new Padding(6);
            tabAlistar.Size = new Size(1202, 482);
            tabAlistar.TabIndex = 0;
            tabAlistar.Text = "Alistar";
            tabAlistar.UseVisualStyleBackColor = true;
            // 
            // splMain
            // 
            splMain.Dock = DockStyle.Fill;
            splMain.Location = new Point(6, 6);
            splMain.Margin = new Padding(2);
            splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            splMain.Panel1.Controls.Add(tlpLeft);
            splMain.Panel1.Padding = new Padding(6);
            // 
            // splMain.Panel2
            // 
            splMain.Panel2.Controls.Add(grpDetalleCamion);
            splMain.Panel2.Padding = new Padding(6);
            splMain.Size = new Size(1190, 470);
            splMain.SplitterDistance = 522;
            splMain.SplitterWidth = 3;
            splMain.TabIndex = 0;
            // 
            // tlpLeft
            // 
            tlpLeft.ColumnCount = 1;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 253F));
            tlpLeft.Controls.Add(lblListasTitulo, 0, 0);
            tlpLeft.Controls.Add(lvwListasCamiones, 0, 1);
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.Location = new Point(6, 6);
            tlpLeft.Margin = new Padding(2);
            tlpLeft.Name = "tlpLeft";
            tlpLeft.RowCount = 2;
            tlpLeft.RowStyles.Add(new RowStyle());
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            tlpLeft.Size = new Size(510, 458);
            tlpLeft.TabIndex = 0;
            // 
            // lblListasTitulo
            // 
            lblListasTitulo.AutoSize = true;
            lblListasTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblListasTitulo.Location = new Point(0, 0);
            lblListasTitulo.Margin = new Padding(0, 0, 0, 5);
            lblListasTitulo.Name = "lblListasTitulo";
            lblListasTitulo.Size = new Size(154, 21);
            lblListasTitulo.TabIndex = 0;
            lblListasTitulo.Text = "Listas de Camiones";
            // 
            // lvwListasCamiones
            // 
            lvwListasCamiones.Columns.AddRange(new ColumnHeader[] { colPlaca, colFecha, colCantidad });
            lvwListasCamiones.Cursor = Cursors.Hand;
            lvwListasCamiones.Dock = DockStyle.Fill;
            lvwListasCamiones.FullRowSelect = true;
            lvwListasCamiones.Location = new Point(2, 28);
            lvwListasCamiones.Margin = new Padding(2);
            lvwListasCamiones.Name = "lvwListasCamiones";
            lvwListasCamiones.Size = new Size(506, 428);
            lvwListasCamiones.TabIndex = 0;
            lvwListasCamiones.UseCompatibleStateImageBehavior = false;
            lvwListasCamiones.View = View.Details;
            lvwListasCamiones.SelectedIndexChanged += lvwListasCamiones_SelectedIndexChanged;
            lvwListasCamiones.MouseDown += lvwListasCamiones_MouseDown;
            // 
            // colPlaca
            // 
            colPlaca.Text = "Placa";
            colPlaca.Width = 130;
            // 
            // colFecha
            // 
            colFecha.Text = "Fecha";
            colFecha.Width = 150;
            // 
            // colCantidad
            // 
            colCantidad.Text = "Cantidad";
            colCantidad.Width = 130;
            // 
            // grpDetalleCamion
            // 
            grpDetalleCamion.Controls.Add(tlpRight);
            grpDetalleCamion.Dock = DockStyle.Fill;
            grpDetalleCamion.Location = new Point(6, 6);
            grpDetalleCamion.Margin = new Padding(2);
            grpDetalleCamion.Name = "grpDetalleCamion";
            grpDetalleCamion.Padding = new Padding(8);
            grpDetalleCamion.Size = new Size(653, 458);
            grpDetalleCamion.TabIndex = 0;
            grpDetalleCamion.TabStop = false;
            grpDetalleCamion.Text = "Escoger Camión";
            // 
            // tlpRight
            // 
            tlpRight.ColumnCount = 1;
            tlpRight.ColumnStyles.Add(new ColumnStyle());
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRight.Controls.Add(lblFechaTitulo, 0, 1);
            tlpRight.Controls.Add(lblFechaValor, 1, 1);
            tlpRight.Controls.Add(lblItemsTitulo, 0, 2);
            tlpRight.Controls.Add(dgvItems, 0, 4);
            tlpRight.Controls.Add(lblTituloCamion, 1, 0);
            tlpRight.Controls.Add(tlpBotones, 0, 6);
            tlpRight.Dock = DockStyle.Fill;
            tlpRight.Location = new Point(8, 30);
            tlpRight.Margin = new Padding(2);
            tlpRight.Name = "tlpRight";
            tlpRight.RowCount = 7;
            tlpRight.RowStyles.Add(new RowStyle());
            tlpRight.RowStyles.Add(new RowStyle());
            tlpRight.RowStyles.Add(new RowStyle());
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRight.RowStyles.Add(new RowStyle());
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            tlpRight.Size = new Size(637, 420);
            tlpRight.TabIndex = 0;
            // 
            // lblFechaTitulo
            // 
            lblFechaTitulo.AutoSize = true;
            lblFechaTitulo.Font = new Font("Segoe UI", 10F);
            lblFechaTitulo.Location = new Point(0, 35);
            lblFechaTitulo.Margin = new Padding(0, 0, 5, 5);
            lblFechaTitulo.Name = "lblFechaTitulo";
            lblFechaTitulo.Size = new Size(47, 19);
            lblFechaTitulo.TabIndex = 1;
            lblFechaTitulo.Text = "Fecha:";
            // 
            // lblFechaValor
            // 
            lblFechaValor.AutoSize = true;
            lblFechaValor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFechaValor.Location = new Point(0, 59);
            lblFechaValor.Margin = new Padding(0, 0, 0, 5);
            lblFechaValor.Name = "lblFechaValor";
            lblFechaValor.Size = new Size(69, 19);
            lblFechaValor.TabIndex = 2;
            lblFechaValor.Text = "--/--/----";
            // 
            // lblItemsTitulo
            // 
            lblItemsTitulo.AutoSize = true;
            tlpRight.SetColumnSpan(lblItemsTitulo, 2);
            lblItemsTitulo.Font = new Font("Segoe UI", 10F);
            lblItemsTitulo.Location = new Point(0, 88);
            lblItemsTitulo.Margin = new Padding(0, 5, 0, 5);
            lblItemsTitulo.Name = "lblItemsTitulo";
            lblItemsTitulo.Size = new Size(97, 19);
            lblItemsTitulo.TabIndex = 3;
            lblItemsTitulo.Text = "Lista de Items:";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.BackgroundColor = SystemColors.Window;
            dgvItems.ColumnHeadersHeight = 35;
            tlpRight.SetColumnSpan(dgvItems, 2);
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Location = new Point(2, 119);
            dgvItems.Margin = new Padding(2);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowHeadersWidth = 51;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(633, 231);
            dgvItems.TabIndex = 4;
            // 
            // lblTituloCamion
            // 
            lblTituloCamion.AutoSize = true;
            tlpRight.SetColumnSpan(lblTituloCamion, 2);
            lblTituloCamion.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTituloCamion.Location = new Point(0, 0);
            lblTituloCamion.Margin = new Padding(0, 0, 0, 5);
            lblTituloCamion.Name = "lblTituloCamion";
            lblTituloCamion.Size = new Size(119, 30);
            lblTituloCamion.TabIndex = 0;
            lblTituloCamion.Text = "CAMIÓN -";
            // 
            // tlpBotones
            // 
            tlpBotones.ColumnCount = 4;
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.24138F));
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57.75862F));
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 183F));
            tlpBotones.Controls.Add(btnImprimir, 3, 0);
            tlpBotones.Controls.Add(btnVerMas, 2, 0);
            tlpBotones.Controls.Add(btnRecargar, 1, 0);
            tlpBotones.Controls.Add(btnAlistar, 0, 0);
            tlpBotones.Dock = DockStyle.Fill;
            tlpBotones.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tlpBotones.Location = new Point(3, 355);
            tlpBotones.Name = "tlpBotones";
            tlpBotones.RowCount = 1;
            tlpBotones.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpBotones.Size = new Size(631, 62);
            tlpBotones.TabIndex = 8;
            // 
            // btnImprimir
            // 
            btnImprimir.AutoSize = true;
            btnImprimir.BackColor = Color.MediumBlue;
            btnImprimir.Cursor = Cursors.Hand;
            btnImprimir.Dock = DockStyle.Fill;
            btnImprimir.FlatAppearance.BorderColor = Color.FromArgb(155, 187, 89);
            btnImprimir.FlatStyle = FlatStyle.Flat;
            btnImprimir.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnImprimir.ForeColor = Color.White;
            btnImprimir.Location = new Point(449, 2);
            btnImprimir.Margin = new Padding(2);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Padding = new Padding(10, 5, 10, 5);
            btnImprimir.Size = new Size(180, 58);
            btnImprimir.TabIndex = 11;
            btnImprimir.Text = "IMPRIMIR";
            btnImprimir.UseVisualStyleBackColor = false;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnVerMas
            // 
            btnVerMas.AutoSize = true;
            btnVerMas.BackColor = Color.Gray;
            btnVerMas.Cursor = Cursors.Hand;
            btnVerMas.Dock = DockStyle.Fill;
            btnVerMas.FlatAppearance.BorderColor = Color.FromArgb(155, 187, 89);
            btnVerMas.FlatStyle = FlatStyle.Flat;
            btnVerMas.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnVerMas.ForeColor = Color.White;
            btnVerMas.Location = new Point(275, 2);
            btnVerMas.Margin = new Padding(2);
            btnVerMas.Name = "btnVerMas";
            btnVerMas.Padding = new Padding(10, 5, 10, 5);
            btnVerMas.Size = new Size(170, 58);
            btnVerMas.TabIndex = 10;
            btnVerMas.Text = "VER MÁS";
            btnVerMas.UseVisualStyleBackColor = false;
            btnVerMas.Click += btnVerMas_Click;
            // 
            // btnRecargar
            // 
            btnRecargar.Cursor = Cursors.Hand;
            btnRecargar.Dock = DockStyle.Fill;
            btnRecargar.Location = new Point(149, 3);
            btnRecargar.Name = "btnRecargar";
            btnRecargar.Size = new Size(121, 56);
            btnRecargar.TabIndex = 8;
            btnRecargar.Text = "RECARGAR";
            btnRecargar.UseVisualStyleBackColor = true;
            btnRecargar.Click += btnRecargar_Click;
            // 
            // btnAlistar
            // 
            btnAlistar.AutoSize = true;
            btnAlistar.BackColor = Color.FromArgb(198, 239, 206);
            btnAlistar.Cursor = Cursors.Hand;
            btnAlistar.Dock = DockStyle.Fill;
            btnAlistar.FlatAppearance.BorderColor = Color.FromArgb(155, 187, 89);
            btnAlistar.FlatStyle = FlatStyle.Flat;
            btnAlistar.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnAlistar.Location = new Point(2, 2);
            btnAlistar.Margin = new Padding(2);
            btnAlistar.Name = "btnAlistar";
            btnAlistar.Padding = new Padding(10, 5, 10, 5);
            btnAlistar.Size = new Size(142, 58);
            btnAlistar.TabIndex = 7;
            btnAlistar.Text = "ALISTAR";
            btnAlistar.UseVisualStyleBackColor = false;
            btnAlistar.Visible = false;
            btnAlistar.Click += btnAlistar_Click;
            // 
            // tabReportes
            // 
            tabReportes.Controls.Add(tlpReportes);
            tabReportes.Location = new Point(4, 24);
            tabReportes.Margin = new Padding(2);
            tabReportes.Name = "tabReportes";
            tabReportes.Padding = new Padding(6);
            tabReportes.Size = new Size(1202, 488);
            tabReportes.TabIndex = 1;
            tabReportes.Text = "Reportes";
            tabReportes.UseVisualStyleBackColor = true;
            // 
            // tlpReportes
            // 
            tlpReportes.ColumnCount = 2;
            tlpReportes.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 224F));
            tlpReportes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpReportes.Controls.Add(tlpFiltros, 0, 0);
            tlpReportes.Controls.Add(tlpResumenHost, 1, 0);
            tlpReportes.Dock = DockStyle.Fill;
            tlpReportes.Location = new Point(6, 6);
            tlpReportes.Margin = new Padding(2);
            tlpReportes.Name = "tlpReportes";
            tlpReportes.RowCount = 1;
            tlpReportes.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            tlpReportes.Size = new Size(1190, 476);
            tlpReportes.TabIndex = 0;
            // 
            // tlpFiltros
            // 
            tlpFiltros.ColumnCount = 1;
            tlpFiltros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 206F));
            tlpFiltros.Controls.Add(lblEscogeFecha, 0, 0);
            tlpFiltros.Controls.Add(grpFecha, 0, 1);
            tlpFiltros.Dock = DockStyle.Fill;
            tlpFiltros.Location = new Point(2, 2);
            tlpFiltros.Margin = new Padding(2);
            tlpFiltros.Name = "tlpFiltros";
            tlpFiltros.Padding = new Padding(6);
            tlpFiltros.RowCount = 6;
            tlpFiltros.RowStyles.Add(new RowStyle());
            tlpFiltros.RowStyles.Add(new RowStyle());
            tlpFiltros.RowStyles.Add(new RowStyle());
            tlpFiltros.RowStyles.Add(new RowStyle());
            tlpFiltros.RowStyles.Add(new RowStyle());
            tlpFiltros.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFiltros.Size = new Size(220, 472);
            tlpFiltros.TabIndex = 0;
            // 
            // lblEscogeFecha
            // 
            lblEscogeFecha.AutoSize = true;
            lblEscogeFecha.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblEscogeFecha.Location = new Point(6, 6);
            lblEscogeFecha.Margin = new Padding(0, 0, 0, 6);
            lblEscogeFecha.Name = "lblEscogeFecha";
            lblEscogeFecha.Size = new Size(129, 21);
            lblEscogeFecha.TabIndex = 0;
            lblEscogeFecha.Text = "Escoge la Fecha";
            // 
            // grpFecha
            // 
            grpFecha.Controls.Add(tlpFecha);
            grpFecha.Dock = DockStyle.Top;
            grpFecha.Location = new Point(8, 35);
            grpFecha.Margin = new Padding(2);
            grpFecha.Name = "grpFecha";
            grpFecha.Padding = new Padding(6);
            grpFecha.Size = new Size(204, 360);
            grpFecha.TabIndex = 1;
            grpFecha.TabStop = false;
            // 
            // tlpFecha
            // 
            tlpFecha.ColumnCount = 1;
            tlpFecha.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 189F));
            tlpFecha.Controls.Add(dtpFechaReporte, 0, 1);
            tlpFecha.Controls.Add(lblFechaTituloRpt, 0, 0);
            tlpFecha.Dock = DockStyle.Fill;
            tlpFecha.Location = new Point(6, 28);
            tlpFecha.Margin = new Padding(2);
            tlpFecha.Name = "tlpFecha";
            tlpFecha.RowCount = 2;
            tlpFecha.RowStyles.Add(new RowStyle());
            tlpFecha.RowStyles.Add(new RowStyle());
            tlpFecha.Size = new Size(192, 326);
            tlpFecha.TabIndex = 0;
            // 
            // dtpFechaReporte
            // 
            dtpFechaReporte.CustomFormat = "dd/MM/yyyy";
            dtpFechaReporte.Dock = DockStyle.Top;
            dtpFechaReporte.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dtpFechaReporte.Format = DateTimePickerFormat.Custom;
            dtpFechaReporte.Location = new Point(2, 27);
            dtpFechaReporte.Margin = new Padding(2);
            dtpFechaReporte.Name = "dtpFechaReporte";
            dtpFechaReporte.Size = new Size(188, 29);
            dtpFechaReporte.TabIndex = 1;
            // 
            // lblFechaTituloRpt
            // 
            lblFechaTituloRpt.AutoSize = true;
            lblFechaTituloRpt.Font = new Font("Segoe UI", 11F);
            lblFechaTituloRpt.Location = new Point(0, 0);
            lblFechaTituloRpt.Margin = new Padding(0, 0, 0, 5);
            lblFechaTituloRpt.Name = "lblFechaTituloRpt";
            lblFechaTituloRpt.Size = new Size(50, 20);
            lblFechaTituloRpt.TabIndex = 0;
            lblFechaTituloRpt.Text = "Fecha:";
            // 
            // tlpResumenHost
            // 
            tlpResumenHost.ColumnCount = 1;
            tlpResumenHost.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 16F));
            tlpResumenHost.Controls.Add(tbcTurnos, 0, 0);
            tlpResumenHost.Controls.Add(grpResumen, 0, 1);
            tlpResumenHost.Dock = DockStyle.Fill;
            tlpResumenHost.Location = new Point(226, 2);
            tlpResumenHost.Margin = new Padding(2);
            tlpResumenHost.Name = "tlpResumenHost";
            tlpResumenHost.RowCount = 2;
            tlpResumenHost.RowStyles.Add(new RowStyle());
            tlpResumenHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpResumenHost.Size = new Size(962, 472);
            tlpResumenHost.TabIndex = 1;
            // 
            // tbcTurnos
            // 
            tbcTurnos.Controls.Add(tabTotal);
            tbcTurnos.Controls.Add(tabTurno1);
            tbcTurnos.Controls.Add(tabTurno2);
            tbcTurnos.Controls.Add(tabTurno3);
            tbcTurnos.Dock = DockStyle.Fill;
            tbcTurnos.ItemSize = new Size(100, 28);
            tbcTurnos.Location = new Point(2, 2);
            tbcTurnos.Margin = new Padding(2);
            tbcTurnos.Name = "tbcTurnos";
            tbcTurnos.SelectedIndex = 0;
            tbcTurnos.Size = new Size(958, 80);
            tbcTurnos.SizeMode = TabSizeMode.Fixed;
            tbcTurnos.TabIndex = 0;
            // 
            // tabTotal
            // 
            tabTotal.Location = new Point(4, 32);
            tabTotal.Margin = new Padding(2);
            tabTotal.Name = "tabTotal";
            tabTotal.Size = new Size(950, 44);
            tabTotal.TabIndex = 0;
            tabTotal.Text = "TOTAL";
            tabTotal.UseVisualStyleBackColor = true;
            // 
            // tabTurno1
            // 
            tabTurno1.Location = new Point(4, 32);
            tabTurno1.Margin = new Padding(2);
            tabTurno1.Name = "tabTurno1";
            tabTurno1.Size = new Size(950, 44);
            tabTurno1.TabIndex = 1;
            tabTurno1.Text = "TURNO1";
            tabTurno1.UseVisualStyleBackColor = true;
            // 
            // tabTurno2
            // 
            tabTurno2.Location = new Point(4, 32);
            tabTurno2.Margin = new Padding(2);
            tabTurno2.Name = "tabTurno2";
            tabTurno2.Size = new Size(950, 44);
            tabTurno2.TabIndex = 2;
            tabTurno2.Text = "TURNO2";
            tabTurno2.UseVisualStyleBackColor = true;
            // 
            // tabTurno3
            // 
            tabTurno3.Location = new Point(4, 32);
            tabTurno3.Margin = new Padding(2);
            tabTurno3.Name = "tabTurno3";
            tabTurno3.Size = new Size(950, 44);
            tabTurno3.TabIndex = 3;
            tabTurno3.Text = "TURNO3";
            tabTurno3.UseVisualStyleBackColor = true;
            // 
            // grpResumen
            // 
            grpResumen.Controls.Add(tlpResumen);
            grpResumen.Dock = DockStyle.Fill;
            grpResumen.Location = new Point(2, 86);
            grpResumen.Margin = new Padding(2);
            grpResumen.Name = "grpResumen";
            grpResumen.Padding = new Padding(6);
            grpResumen.Size = new Size(958, 384);
            grpResumen.TabIndex = 1;
            grpResumen.TabStop = false;
            // 
            // tlpResumen
            // 
            tlpResumen.ColumnCount = 2;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpResumen.Controls.Add(dgvResumen, 0, 0);
            tlpResumen.Controls.Add(pnlKpis, 1, 0);
            tlpResumen.Controls.Add(dgvMovimientos, 0, 1);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Location = new Point(6, 28);
            tlpResumen.Margin = new Padding(2);
            tlpResumen.Name = "tlpResumen";
            tlpResumen.RowCount = 2;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tlpResumen.Size = new Size(946, 350);
            tlpResumen.TabIndex = 0;
            // 
            // dgvResumen
            // 
            dgvResumen.AllowUserToAddRows = false;
            dgvResumen.AllowUserToDeleteRows = false;
            dgvResumen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResumen.BackgroundColor = SystemColors.Window;
            dgvResumen.ColumnHeadersHeight = 29;
            dgvResumen.Dock = DockStyle.Fill;
            dgvResumen.Location = new Point(2, 2);
            dgvResumen.Margin = new Padding(2);
            dgvResumen.Name = "dgvResumen";
            dgvResumen.ReadOnly = true;
            dgvResumen.RowHeadersVisible = false;
            dgvResumen.RowHeadersWidth = 51;
            dgvResumen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResumen.Size = new Size(658, 188);
            dgvResumen.TabIndex = 0;
            // 
            // pnlKpis
            // 
            pnlKpis.Controls.Add(lblCamionesTexto);
            pnlKpis.Controls.Add(lblCamionesNumero);
            pnlKpis.Controls.Add(lblUnidadesTexto);
            pnlKpis.Controls.Add(lblUnidadesPacas);
            pnlKpis.Dock = DockStyle.Fill;
            pnlKpis.Location = new Point(664, 2);
            pnlKpis.Margin = new Padding(2);
            pnlKpis.Name = "pnlKpis";
            pnlKpis.Padding = new Padding(6);
            pnlKpis.Size = new Size(280, 188);
            pnlKpis.TabIndex = 1;
            // 
            // lblCamionesTexto
            // 
            lblCamionesTexto.AutoSize = true;
            lblCamionesTexto.Font = new Font("Segoe UI", 11F);
            lblCamionesTexto.Location = new Point(10, 109);
            lblCamionesTexto.Margin = new Padding(2, 0, 2, 0);
            lblCamionesTexto.Name = "lblCamionesTexto";
            lblCamionesTexto.Size = new Size(72, 20);
            lblCamionesTexto.TabIndex = 0;
            lblCamionesTexto.Text = "camiones";
            // 
            // lblCamionesNumero
            // 
            lblCamionesNumero.AutoSize = true;
            lblCamionesNumero.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblCamionesNumero.Location = new Point(6, 74);
            lblCamionesNumero.Margin = new Padding(2, 0, 2, 0);
            lblCamionesNumero.Name = "lblCamionesNumero";
            lblCamionesNumero.Size = new Size(38, 45);
            lblCamionesNumero.TabIndex = 1;
            lblCamionesNumero.Text = "0";
            // 
            // lblUnidadesTexto
            // 
            lblUnidadesTexto.AutoSize = true;
            lblUnidadesTexto.Font = new Font("Segoe UI", 11F);
            lblUnidadesTexto.Location = new Point(10, 42);
            lblUnidadesTexto.Margin = new Padding(2, 0, 2, 0);
            lblUnidadesTexto.Name = "lblUnidadesTexto";
            lblUnidadesTexto.Size = new Size(69, 20);
            lblUnidadesTexto.TabIndex = 2;
            lblUnidadesTexto.Text = "unidades";
            // 
            // lblUnidadesPacas
            // 
            lblUnidadesPacas.AutoSize = true;
            lblUnidadesPacas.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblUnidadesPacas.Location = new Point(6, 6);
            lblUnidadesPacas.Margin = new Padding(2, 0, 2, 0);
            lblUnidadesPacas.Name = "lblUnidadesPacas";
            lblUnidadesPacas.Size = new Size(38, 45);
            lblUnidadesPacas.TabIndex = 3;
            lblUnidadesPacas.Text = "0";
            // 
            // dgvMovimientos
            // 
            dgvMovimientos.AllowUserToAddRows = false;
            dgvMovimientos.AllowUserToDeleteRows = false;
            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovimientos.BackgroundColor = SystemColors.Window;
            dgvMovimientos.ColumnHeadersHeight = 29;
            tlpResumen.SetColumnSpan(dgvMovimientos, 2);
            dgvMovimientos.Dock = DockStyle.Fill;
            dgvMovimientos.Location = new Point(2, 194);
            dgvMovimientos.Margin = new Padding(2);
            dgvMovimientos.Name = "dgvMovimientos";
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.RowHeadersVisible = false;
            dgvMovimientos.RowHeadersWidth = 51;
            dgvMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovimientos.Size = new Size(942, 154);
            dgvMovimientos.TabIndex = 2;
            // 
            // tabCargueMasivo
            // 
            tabCargueMasivo.Controls.Add(label4);
            tabCargueMasivo.Controls.Add(lstErrores);
            tabCargueMasivo.Controls.Add(label3);
            tabCargueMasivo.Controls.Add(label2);
            tabCargueMasivo.Controls.Add(pictureBox1);
            tabCargueMasivo.Controls.Add(label1);
            tabCargueMasivo.Controls.Add(lblProgreso);
            tabCargueMasivo.Controls.Add(dtgAgrupada);
            tabCargueMasivo.Controls.Add(btnCargarArchivo);
            tabCargueMasivo.Controls.Add(tableLayoutPanel2);
            tabCargueMasivo.Location = new Point(4, 24);
            tabCargueMasivo.Name = "tabCargueMasivo";
            tabCargueMasivo.Size = new Size(1202, 488);
            tabCargueMasivo.TabIndex = 2;
            tabCargueMasivo.Text = "Cargue Masivo";
            tabCargueMasivo.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(853, 2);
            label4.Name = "label4";
            label4.Size = new Size(150, 41);
            label4.TabIndex = 11;
            label4.Text = "ERRORES";
            // 
            // lstErrores
            // 
            lstErrores.FormattingEnabled = true;
            lstErrores.ItemHeight = 21;
            lstErrores.Location = new Point(853, 46);
            lstErrores.Name = "lstErrores";
            lstErrores.Size = new Size(291, 172);
            lstErrores.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(488, 0);
            label3.Name = "label3";
            label3.Size = new Size(177, 41);
            label3.TabIndex = 9;
            label3.Text = "CAMIONES";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(24, 180);
            label2.Name = "label2";
            label2.Size = new Size(238, 41);
            label2.TabIndex = 8;
            label2.Text = "DOCUMENTOS:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.green_truck;
            pictureBox1.InitialImage = Properties.Resources.green_truck;
            pictureBox1.Location = new Point(54, 33);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(66, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 28.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LimeGreen;
            label1.Location = new Point(132, 37);
            label1.Name = "label1";
            label1.Size = new Size(332, 44);
            label1.TabIndex = 5;
            label1.Text = "Cargar Camiones";
            // 
            // lblProgreso
            // 
            lblProgreso.AutoSize = true;
            lblProgreso.Location = new Point(30, 449);
            lblProgreso.Name = "lblProgreso";
            lblProgreso.Size = new Size(0, 21);
            lblProgreso.TabIndex = 3;
            // 
            // dtgAgrupada
            // 
            dtgAgrupada.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgAgrupada.Location = new Point(488, 44);
            dtgAgrupada.Name = "dtgAgrupada";
            dtgAgrupada.Size = new Size(351, 163);
            dtgAgrupada.TabIndex = 1;
            // 
            // btnCargarArchivo
            // 
            btnCargarArchivo.BackColor = Color.LimeGreen;
            btnCargarArchivo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCargarArchivo.ForeColor = SystemColors.Window;
            btnCargarArchivo.Location = new Point(204, 108);
            btnCargarArchivo.Name = "btnCargarArchivo";
            btnCargarArchivo.Size = new Size(278, 44);
            btnCargarArchivo.TabIndex = 0;
            btnCargarArchivo.Text = "Cargar Archivo";
            btnCargarArchivo.UseVisualStyleBackColor = false;
            btnCargarArchivo.Click += btnCargarArchivo_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnCrearCamiones, 0, 2);
            tableLayoutPanel2.Controls.Add(progressBar1, 0, 1);
            tableLayoutPanel2.Controls.Add(dtgCargueMasivo, 0, 0);
            tableLayoutPanel2.Location = new Point(21, 224);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 172F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 49.64029F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50.35971F));
            tableLayoutPanel2.Size = new Size(1126, 255);
            tableLayoutPanel2.TabIndex = 7;
            // 
            // btnCrearCamiones
            // 
            btnCrearCamiones.Dock = DockStyle.Fill;
            btnCrearCamiones.Location = new Point(3, 216);
            btnCrearCamiones.Name = "btnCrearCamiones";
            btnCrearCamiones.Size = new Size(1120, 36);
            btnCrearCamiones.TabIndex = 0;
            btnCrearCamiones.Text = "CREAR CAMIONES";
            btnCrearCamiones.UseVisualStyleBackColor = true;
            btnCrearCamiones.Click += button1_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(3, 175);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1120, 35);
            progressBar1.TabIndex = 2;
            // 
            // dtgCargueMasivo
            // 
            dtgCargueMasivo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgCargueMasivo.Location = new Point(3, 3);
            dtgCargueMasivo.Name = "dtgCargueMasivo";
            dtgCargueMasivo.Size = new Size(1120, 166);
            dtgCargueMasivo.TabIndex = 1;
            // 
            // tabAdmonCamiones
            // 
            tabAdmonCamiones.Controls.Add(lstCamiones);
            tabAdmonCamiones.Controls.Add(btnExportar);
            tabAdmonCamiones.Controls.Add(btnCerrarCamion);
            tabAdmonCamiones.Controls.Add(dataGridView1);
            tabAdmonCamiones.Location = new Point(4, 30);
            tabAdmonCamiones.Name = "tabAdmonCamiones";
            tabAdmonCamiones.Size = new Size(1202, 482);
            tabAdmonCamiones.TabIndex = 3;
            tabAdmonCamiones.Text = "AdministracionCamiones";
            tabAdmonCamiones.UseVisualStyleBackColor = true;
            // 
            // lstCamiones
            // 
            lstCamiones.FormattingEnabled = true;
            lstCamiones.Location = new Point(8, 31);
            lstCamiones.Name = "lstCamiones";
            lstCamiones.Size = new Size(304, 412);
            lstCamiones.TabIndex = 14;
            lstCamiones.SelectedIndexChanged += lstCamiones_SelectedIndexChanged_1;
            // 
            // btnCerrarCamion
            // 
            btnCerrarCamion.FlatStyle = FlatStyle.Flat;
            btnCerrarCamion.Location = new Point(1011, 31);
            btnCerrarCamion.Name = "btnCerrarCamion";
            btnCerrarCamion.Size = new Size(183, 80);
            btnCerrarCamion.TabIndex = 13;
            btnCerrarCamion.Text = "ANULAR PROGRAMACION CAMION";
            btnCerrarCamion.UseVisualStyleBackColor = true;
            btnCerrarCamion.Click += btnCerrarCamion_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(318, 31);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(674, 421);
            dataGridView1.TabIndex = 12;
            // 
            // btnExportar
            // 
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.Location = new Point(1011, 144);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(183, 80);
            btnExportar.TabIndex = 13;
            btnExportar.Text = "EXPORTAR REPORTE";
            btnExportar.UseVisualStyleBackColor = true;
            btnExportar.Click += btnExportar_Click;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1210, 516);
            Controls.Add(tabMain);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(2);
            MinimumSize = new Size(643, 408);
            Name = "Menu";
            Text = "Alistar Camión";
            FormClosing += ALISTAR_CAMION_FormClosing;
            Load += ALISTAR_CAMION_Load;
            tabMain.ResumeLayout(false);
            tabAlistar.ResumeLayout(false);
            splMain.Panel1.ResumeLayout(false);
            splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splMain).EndInit();
            splMain.ResumeLayout(false);
            tlpLeft.ResumeLayout(false);
            tlpLeft.PerformLayout();
            grpDetalleCamion.ResumeLayout(false);
            tlpRight.ResumeLayout(false);
            tlpRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            tlpBotones.ResumeLayout(false);
            tlpBotones.PerformLayout();
            tabReportes.ResumeLayout(false);
            tlpReportes.ResumeLayout(false);
            tlpFiltros.ResumeLayout(false);
            tlpFiltros.PerformLayout();
            grpFecha.ResumeLayout(false);
            tlpFecha.ResumeLayout(false);
            tlpFecha.PerformLayout();
            tlpResumenHost.ResumeLayout(false);
            tbcTurnos.ResumeLayout(false);
            grpResumen.ResumeLayout(false);
            tlpResumen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResumen).EndInit();
            pnlKpis.ResumeLayout(false);
            pnlKpis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).EndInit();
            tabCargueMasivo.ResumeLayout(false);
            tabCargueMasivo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtgAgrupada).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtgCargueMasivo).EndInit();
            tabAdmonCamiones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabMain; // tab
        private TabPage tabAlistar; // tab
        private TabPage tabReportes; // tab
        private SplitContainer splMain; // spl

        // Reportes fields
        private TableLayoutPanel tlpReportes; // tlp
        private TableLayoutPanel tlpFiltros; // tlp
        private Label lblEscogeFecha; // lbl
        private GroupBox grpFecha; // grp
        private TableLayoutPanel tlpFecha; // tlp
        private Label lblFechaTituloRpt; // lbl
        private DateTimePicker dtpFechaReporte; // dtp
        private TableLayoutPanel tlpResumenHost; // tlp
        private TabControl tbcTurnos; // tab
        private TabPage tabTurno3; // tab
        private GroupBox grpResumen; // grp
        private TableLayoutPanel tlpResumen; // tlp
        private DataGridView dgvResumen; // dgv
        private Panel pnlKpis; // pnl
        private Label lblUnidadesPacas; // lbl
        private Label lblUnidadesTexto; // lbl
        private Label lblCamionesNumero; // lbl
        private Label lblCamionesTexto; // lbl
        private DataGridView dgvMovimientos; // dgv
        private TableLayoutPanel tlpLeft;
        private Label lblListasTitulo;
        private ListView lvwListasCamiones;
        private ColumnHeader colPlaca;
        private ColumnHeader colFecha;
        private ColumnHeader colCantidad;
        private GroupBox grpDetalleCamion;
        private TableLayoutPanel tlpRight;
        private Label lblFechaTitulo;
        private Label lblFechaValor;
        private Label lblTituloCamion;
        private Label lblItemsTitulo;
        private TabPage tabTotal;
        private TabPage tabTurno1;
        private TabPage tabTurno2;
        private TabPage tabCargueMasivo;
        private DataGridView dtgCargueMasivo;
        private Button btnCargarArchivo;
        private DataGridView dtgAgrupada;
        private Button btnCrearCamiones;
        private ProgressBar progressBar1;
        private Label lblProgreso;
        private Label label1;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label3;
        private Label label2;
        private Label label4;
        private ListBox lstErrores;
        private TabPage tabAdmonCamiones;
        private DataGridView dataGridView1;
        private Button btnCerrarCamion;
        private DataGridView dgvItems;
        private TableLayoutPanel tlpBotones;
        private Button btnRecargar;
        private Button btnAlistar;
        private Button btnImprimir;
        private Button btnVerMas;
        private CheckedListBox lstCamiones;
        private Button btnExportar;
    }
}