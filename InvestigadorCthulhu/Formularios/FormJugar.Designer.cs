namespace InvestigadorCthulhu.Formularios
{
    partial class FormJugar
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
            lblTitulo = new Label();
            lblEstado = new Label();
            btnVerFicha = new Button();
            txtTexto = new TextBox();
            flpOpciones = new FlowLayoutPanel();
            lblRegistro = new Label();
            lstRegistro = new ListBox();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Georgia", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(100, 31);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Título";
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(24, 60);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(50, 20);
            lblEstado.TabIndex = 1;
            lblEstado.Text = "Estado";
            // 
            // btnVerFicha
            // 
            btnVerFicha.Location = new Point(490, 52);
            btnVerFicha.Name = "btnVerFicha";
            btnVerFicha.Size = new Size(130, 32);
            btnVerFicha.TabIndex = 6;
            btnVerFicha.Text = "Ver ficha (>40%)";
            btnVerFicha.UseVisualStyleBackColor = true;
            btnVerFicha.Click += btnVerFicha_Click;
            // 
            // txtTexto
            // 
            txtTexto.Font = new Font("Georgia", 12F);
            txtTexto.Location = new Point(20, 95);
            txtTexto.Multiline = true;
            txtTexto.Name = "txtTexto";
            txtTexto.ReadOnly = true;
            txtTexto.ScrollBars = ScrollBars.Vertical;
            txtTexto.Size = new Size(600, 260);
            txtTexto.TabIndex = 2;
            // 
            // flpOpciones
            // 
            flpOpciones.AutoScroll = true;
            flpOpciones.FlowDirection = FlowDirection.TopDown;
            flpOpciones.Location = new Point(20, 370);
            flpOpciones.Name = "flpOpciones";
            flpOpciones.Size = new Size(600, 300);
            flpOpciones.TabIndex = 3;
            flpOpciones.WrapContents = false;
            // 
            // lblRegistro
            // 
            lblRegistro.AutoSize = true;
            lblRegistro.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRegistro.Location = new Point(640, 68);
            lblRegistro.Name = "lblRegistro";
            lblRegistro.Size = new Size(135, 20);
            lblRegistro.TabIndex = 4;
            lblRegistro.Text = "Registro de tiradas";
            // 
            // lstRegistro
            // 
            lstRegistro.HorizontalScrollbar = true;
            lstRegistro.ItemHeight = 20;
            lstRegistro.Location = new Point(640, 95);
            lstRegistro.Name = "lstRegistro";
            lstRegistro.Size = new Size(340, 560);
            lstRegistro.TabIndex = 5;
            // 
            // FormJugar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 700);
            Controls.Add(btnVerFicha);
            Controls.Add(lstRegistro);
            Controls.Add(lblRegistro);
            Controls.Add(flpOpciones);
            Controls.Add(txtTexto);
            Controls.Add(lblEstado);
            Controls.Add(lblTitulo);
            Name = "FormJugar";
            StartPosition = FormStartPosition.CenterParent;
            Text = "La última carta del profesor Ward";
            Load += FormJugar_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblEstado;
        private Button btnVerFicha;
        private TextBox txtTexto;
        private FlowLayoutPanel flpOpciones;
        private Label lblRegistro;
        private ListBox lstRegistro;
    }
}