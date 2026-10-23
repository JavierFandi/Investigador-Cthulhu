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
            lblEncabezado.Location = new Point(72, 31);
            lblEncabezado.Name = "lblEncabezado";
            lblEncabezado.Size = new Size(395, 23);
            lblEncabezado.TabIndex = 0;
            lblEncabezado.Text = "La llamada de Cthulhu · Investigadores";
            // 
            // lstInvestigadores
            // 
            lstInvestigadores.Location = new Point(72, 71);
            lstInvestigadores.Margin = new Padding(3, 2, 3, 2);
            lstInvestigadores.Name = "lstInvestigadores";
            lstInvestigadores.Size = new Size(403, 199);
            lstInvestigadores.TabIndex = 1;
            lstInvestigadores.SelectedIndexChanged += lstInvestigadores_SelectedIndexChanged;
            // 
            // btnCrear
            // 
            btnCrear.Location = new Point(72, 290);
            btnCrear.Margin = new Padding(3, 2, 3, 2);
            btnCrear.Name = "btnCrear";
            btnCrear.Size = new Size(127, 28);
            btnCrear.TabIndex = 2;
            btnCrear.Text = "Crear investigador";
            btnCrear.UseVisualStyleBackColor = true;
            btnCrear.Click += btnCrear_Click;
            // 
            // btnJugar
            // 
            btnJugar.Location = new Point(208, 290);
            btnJugar.Margin = new Padding(3, 2, 3, 2);
            btnJugar.Name = "btnJugar";
            btnJugar.Size = new Size(131, 28);
            btnJugar.TabIndex = 3;
            btnJugar.Text = "Nueva partida";
            btnJugar.UseVisualStyleBackColor = true;
            btnJugar.Click += btnJugar_Click;
            // 
            // btnContinuar
            // 
            btnContinuar.Enabled = false;
            btnContinuar.Location = new Point(348, 290);
            btnContinuar.Margin = new Padding(3, 2, 3, 2);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(127, 28);
            btnContinuar.TabIndex = 4;
            btnContinuar.Text = "Continuar";
            btnContinuar.UseVisualStyleBackColor = true;
            btnContinuar.Click += btnContinuar_Click;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(543, 355);
            Controls.Add(btnContinuar);
            Controls.Add(btnJugar);
            Controls.Add(btnCrear);
            Controls.Add(lstInvestigadores);
            Controls.Add(lblEncabezado);
            Margin = new Padding(3, 2, 3, 2);
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