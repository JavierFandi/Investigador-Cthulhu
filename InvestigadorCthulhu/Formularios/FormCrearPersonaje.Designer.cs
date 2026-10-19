namespace InvestigadorCthulhu.Formularios
{
    partial class FormCrearPersonaje
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblName = new Label();
            txtNombre = new TextBox();
            lblEdad = new Label();
            numEdad = new NumericUpDown();
            label1 = new Label();
            cboOcupacion = new ComboBox();
            lblElegirCaract = new Label();
            cboCaracteristicaOcupacion = new ComboBox();
            txtInfoOcupacion = new TextBox();
            gbCaracteristicas = new GroupBox();
            btnTirar = new Button();
            lblSuerte = new Label();
            lblEDU = new Label();
            lblPOD = new Label();
            lblINT = new Label();
            lblAPA = new Label();
            lblDES = new Label();
            lblTAM = new Label();
            lblCON = new Label();
            lblFUE = new Label();
            lblDerivados = new Label();
            lblPuntosOcupacion = new Label();
            lblPuntosInteres = new Label();
            dgvHabilidades = new DataGridView();
            btnCancelar = new Button();
            btnGuardar = new Button();
            ((System.ComponentModel.ISupportInitialize)numEdad).BeginInit();
            gbCaracteristicas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHabilidades).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(19, 38);
            lblName.Name = "lblName";
            lblName.Size = new Size(64, 20);
            lblName.TabIndex = 0;
            lblName.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(105, 35);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(250, 27);
            txtNombre.TabIndex = 1;
            // 
            // lblEdad
            // 
            lblEdad.AutoSize = true;
            lblEdad.Location = new Point(385, 38);
            lblEdad.Name = "lblEdad";
            lblEdad.Size = new Size(43, 20);
            lblEdad.TabIndex = 2;
            lblEdad.Text = "Edad";
            // 
            // numEdad
            // 
            numEdad.Location = new Point(471, 36);
            numEdad.Maximum = new decimal(new int[] { 89, 0, 0, 0 });
            numEdad.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            numEdad.Name = "numEdad";
            numEdad.Size = new Size(62, 27);
            numEdad.TabIndex = 3;
            numEdad.Value = new decimal(new int[] { 25, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 83);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            lblName.TabIndex = 4;
            label1.Text = "Ocupación";
            // 
            // cboOcupacion
            // 
            cboOcupacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOcupacion.FormattingEnabled = true;
            cboOcupacion.Location = new Point(105, 80);
            cboOcupacion.Name = "cboOcupacion";
            cboOcupacion.Size = new Size(250, 28);
            cboOcupacion.TabIndex = 5;
            // 
            // lblElegirCaract
            // 
            lblElegirCaract.AutoSize = true;
            lblElegirCaract.Location = new Point(370, 83);
            lblElegirCaract.Name = "lblElegirCaract";
            lblElegirCaract.Size = new Size(48, 20);
            lblElegirCaract.TabIndex = 14;
            lblElegirCaract.Text = "Elegir:";
            lblElegirCaract.Visible = false;
            // 
            // cboCaracteristicaOcupacion
            // 
            cboCaracteristicaOcupacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCaracteristicaOcupacion.FormattingEnabled = true;
            cboCaracteristicaOcupacion.Location = new Point(425, 80);
            cboCaracteristicaOcupacion.Name = "cboCaracteristicaOcupacion";
            cboCaracteristicaOcupacion.Size = new Size(108, 28);
            cboCaracteristicaOcupacion.TabIndex = 15;
            cboCaracteristicaOcupacion.Visible = false;
            // 
            // txtInfoOcupacion
            // 
            txtInfoOcupacion.Location = new Point(19, 127);
            txtInfoOcupacion.Multiline = true;
            txtInfoOcupacion.Name = "txtInfoOcupacion";
            txtInfoOcupacion.ReadOnly = true;
            txtInfoOcupacion.ScrollBars = ScrollBars.Vertical;
            txtInfoOcupacion.Size = new Size(514, 110);
            txtInfoOcupacion.TabIndex = 6;
            // 
            // gbCaracteristicas
            // 
            gbCaracteristicas.Controls.Add(btnTirar);
            gbCaracteristicas.Controls.Add(lblSuerte);
            gbCaracteristicas.Controls.Add(lblEDU);
            gbCaracteristicas.Controls.Add(lblPOD);
            gbCaracteristicas.Controls.Add(lblINT);
            gbCaracteristicas.Controls.Add(lblAPA);
            gbCaracteristicas.Controls.Add(lblDES);
            gbCaracteristicas.Controls.Add(lblTAM);
            gbCaracteristicas.Controls.Add(lblCON);
            gbCaracteristicas.Controls.Add(lblFUE);
            gbCaracteristicas.Location = new Point(562, 35);
            gbCaracteristicas.Name = "gbCaracteristicas";
            gbCaracteristicas.Size = new Size(341, 222);
            gbCaracteristicas.TabIndex = 7;
            gbCaracteristicas.TabStop = false;
            gbCaracteristicas.Text = "Características";
            // 
            // btnTirar
            // 
            btnTirar.Location = new Point(92, 173);
            btnTirar.Name = "btnTirar";
            btnTirar.Size = new Size(147, 29);
            btnTirar.TabIndex = 9;
            btnTirar.Text = "Tirar Características";
            btnTirar.UseVisualStyleBackColor = true;
            // 
            // lblSuerte
            // 
            lblSuerte.AutoSize = true;
            lblSuerte.Location = new Point(255, 133);
            lblSuerte.Name = "lblSuerte";
            lblSuerte.Size = new Size(58, 20);
            lblSuerte.TabIndex = 8;
            lblSuerte.Text = "label10";
            // 
            // lblEDU
            // 
            lblEDU.AutoSize = true;
            lblEDU.Location = new Point(263, 84);
            lblEDU.Name = "lblEDU";
            lblEDU.Size = new Size(50, 20);
            lblEDU.TabIndex = 7;
            lblEDU.Text = "label9";
            // 
            // lblPOD
            // 
            lblPOD.AutoSize = true;
            lblPOD.Location = new Point(263, 39);
            lblPOD.Name = "lblPOD";
            lblPOD.Size = new Size(50, 20);
            lblPOD.TabIndex = 6;
            lblPOD.Text = "label8";
            // 
            // lblINT
            // 
            lblINT.AutoSize = true;
            lblINT.Location = new Point(141, 133);
            lblINT.Name = "lblINT";
            lblINT.Size = new Size(50, 20);
            lblINT.TabIndex = 5;
            lblINT.Text = "label7";
            // 
            // lblAPA
            // 
            lblAPA.AutoSize = true;
            lblAPA.Location = new Point(141, 84);
            lblAPA.Name = "lblAPA";
            lblAPA.Size = new Size(50, 20);
            lblAPA.TabIndex = 4;
            lblAPA.Text = "label6";
            // 
            // lblDES
            // 
            lblDES.AutoSize = true;
            lblDES.Location = new Point(141, 39);
            lblDES.Name = "lblDES";
            lblDES.Size = new Size(50, 20);
            lblDES.TabIndex = 3;
            lblDES.Text = "label5";
            // 
            // lblTAM
            // 
            lblTAM.AutoSize = true;
            lblTAM.Location = new Point(23, 133);
            lblTAM.Name = "lblTAM";
            lblTAM.Size = new Size(50, 20);
            lblTAM.TabIndex = 2;
            lblTAM.Text = "label4";
            // 
            // lblCON
            // 
            lblCON.AutoSize = true;
            lblCON.Location = new Point(23, 84);
            lblCON.Name = "lblCON";
            lblCON.Size = new Size(50, 20);
            lblCON.TabIndex = 1;
            lblCON.Text = "label3";
            // 
            // lblFUE
            // 
            lblFUE.AutoSize = true;
            lblFUE.Location = new Point(23, 39);
            lblFUE.Name = "lblFUE";
            lblFUE.Size = new Size(50, 20);
            lblFUE.TabIndex = 0;
            lblFUE.Text = "label2";
            // 
            // lblDerivados
            // 
            lblDerivados.AutoSize = true;
            lblDerivados.Location = new Point(19, 265);
            lblDerivados.Name = "lblDerivados";
            lblDerivados.Size = new Size(50, 20);
            lblDerivados.TabIndex = 8;
            lblDerivados.Text = "label2";
            // 
            // lblPuntosOcupacion
            // 
            lblPuntosOcupacion.AutoSize = true;
            lblPuntosOcupacion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPuntosOcupacion.Location = new Point(260, 309);
            lblPuntosOcupacion.Name = "lblPuntosOcupacion";
            lblPuntosOcupacion.Size = new Size(51, 20);
            lblPuntosOcupacion.TabIndex = 9;
            lblPuntosOcupacion.Text = "label2";
            // 
            // lblPuntosInteres
            // 
            lblPuntosInteres.AutoSize = true;
            lblPuntosInteres.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPuntosInteres.Location = new Point(19, 309);
            lblPuntosInteres.Name = "lblPuntosInteres";
            lblPuntosInteres.Size = new Size(51, 20);
            lblPuntosInteres.TabIndex = 10;
            lblPuntosInteres.Text = "label2";
            // 
            // dgvHabilidades
            // 
            dgvHabilidades.AllowUserToAddRows = false;
            dgvHabilidades.AllowUserToDeleteRows = false;
            dgvHabilidades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHabilidades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHabilidades.Location = new Point(19, 355);
            dgvHabilidades.Name = "dgvHabilidades";
            dgvHabilidades.RowHeadersVisible = false;
            dgvHabilidades.RowHeadersWidth = 51;
            dgvHabilidades.Size = new Size(884, 230);
            dgvHabilidades.TabIndex = 11;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(182, 615);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(94, 29);
            btnCancelar.TabIndex = 12;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(587, 615);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(166, 29);
            btnGuardar.TabIndex = 13;
            btnGuardar.Text = "Guardar investigador";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // FormCrearPersonaje
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 673);
            Controls.Add(cboCaracteristicaOcupacion);
            Controls.Add(lblElegirCaract);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            Controls.Add(dgvHabilidades);
            Controls.Add(lblPuntosInteres);
            Controls.Add(lblPuntosOcupacion);
            Controls.Add(lblDerivados);
            Controls.Add(gbCaracteristicas);
            Controls.Add(txtInfoOcupacion);
            Controls.Add(cboOcupacion);
            Controls.Add(label1);
            Controls.Add(numEdad);
            Controls.Add(lblEdad);
            Controls.Add(txtNombre);
            Controls.Add(lblName);
            Name = "FormCrearPersonaje";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Crear investigador";
            ((System.ComponentModel.ISupportInitialize)numEdad).EndInit();
            gbCaracteristicas.ResumeLayout(false);
            gbCaracteristicas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHabilidades).EndInit();
            ResumeLayout(false);
            PerformLayout();

            // Conectar eventos DESPUÉS de EndInit()
            Load += FormCrearPersonaje_Load;
            cboOcupacion.SelectedIndexChanged += cboOcupacion_SelectedIndexChanged;
            cboCaracteristicaOcupacion.SelectedIndexChanged += cboCaracteristicaOcupacion_SelectedIndexChanged;
            btnTirar.Click += btnTirar_Click;
            dgvHabilidades.CellEndEdit += dgvHabilidades_CellEndEdit;
            dgvHabilidades.DataBindingComplete += dgvHabilidades_DataBindingComplete;
            dgvHabilidades.DataError += dgvHabilidades_DataError;
            btnGuardar.Click += btnGuardar_Click;
        }

        #endregion

        private Label lblName;
        private TextBox txtNombre;
        private Label lblEdad;
        private NumericUpDown numEdad;
        private Label label1;
        private ComboBox cboOcupacion;
        private Label lblElegirCaract;
        private ComboBox cboCaracteristicaOcupacion;
        private TextBox txtInfoOcupacion;
        private GroupBox gbCaracteristicas;
        private Label lblSuerte;
        private Label lblEDU;
        private Label lblPOD;
        private Label lblINT;
        private Label lblAPA;
        private Label lblDES;
        private Label lblTAM;
        private Label lblCON;
        private Label lblFUE;
        private Button btnTirar;
        private Label lblDerivados;
        private Label lblPuntosOcupacion;
        private Label lblPuntosInteres;
        private DataGridView dgvHabilidades;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}