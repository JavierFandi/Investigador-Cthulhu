namespace InvestigadorCthulhu.Formularios
{
    partial class FormPrincipal
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
            lblEncabezado = new Label();
            lstInvestigadores = new ListBox();
            btnCrear = new Button();
            btnJugar = new Button();
            btnContinuar = new Button();
            SuspendLayout();
            // 
            // lblEncabezado
            // 
            lblEncabezado.AutoSize = true;
            lblEncabezado.Font = new Font("Georgia", 14F, FontStyle.Bold);
            lblEncabezado.Location = new Point(25, 20);
            lblEncabezado.Name = "lblEncabezado";
            lblEncabezado.Size = new Size(493, 29);
            lblEncabezado.TabIndex = 0;
            lblEncabezado.Text = "La llamada de Cthulhu · Investigadores";
            // 
            // lstInvestigadores
            // 
            lstInvestigadores.Location = new Point(25, 65);
            lstInvestigadores.Name = "lstInvestigadores";
            lstInvestigadores.Size = new Size(460, 264);
            lstInvestigadores.TabIndex = 1;
            lstInvestigadores.SelectedIndexChanged += lstInvestigadores_SelectedIndexChanged;
            // 
            // btnCrear
            // 
            btnCrear.Location = new Point(25, 365);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(145, 38);
            btnCrear.TabIndex = 2;
            btnCrear.Text = "Crear investigador";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // btnJugar
            // 
            btnJugar.Location = new Point(180, 365);
            btnJugar.Name = "btnJugar";
            btnJugar.Size = new Size(150, 38);
            btnJugar.TabIndex = 3;
            btnJugar.Text = "Nueva partida";
            btnJugar.UseVisualStyleBackColor = true;
            btnJugar.Click += btnJugar_Click;
            // 
            // btnContinuar
            // 
            btnContinuar.Enabled = false;
            btnContinuar.Location = new Point(340, 365);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(145, 38);
            btnContinuar.TabIndex = 4;
            btnContinuar.Text = "Continuar";
            btnContinuar.UseVisualStyleBackColor = true;
            btnContinuar.Click += btnContinuar_Click;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(533, 425);
            Controls.Add(btnContinuar);
            Controls.Add(btnJugar);
            Controls.Add(btnCrear);
            Controls.Add(lstInvestigadores);
            Controls.Add(lblEncabezado);
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Investigadores de Haverford";
            Load += FormPrincipal_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblEncabezado;
        private ListBox lstInvestigadores;
        private Button btnCrear;
        private Button btnJugar;
        private Button btnContinuar;
    }
}