namespace InvestigadorCthulhu.Formularios;

public static class TemaCthulhu
{
    public static readonly Color FondoVentana = Color.FromArgb(22, 27, 29);        // Gris pizarra abismal
    public static readonly Color FondoPanel = Color.FromArgb(32, 39, 43);          // Gris verdoso oscuro
    public static readonly Color FondoInput = Color.FromArgb(40, 48, 53);          // Fondo de TextBox / Listas
    public static readonly Color TextoPergamino = Color.FromArgb(232, 224, 206);   // Crema / Pergamino claro
    public static readonly Color DoradoAntiguo = Color.FromArgb(212, 175, 55);     // Títulos y bordes destacados
    public static readonly Color VerdeArcano = Color.FromArgb(44, 82, 70);         // Botones principales
    public static readonly Color VerdeHover = Color.FromArgb(62, 112, 96);         // Botones al pasar el mouse
    public static readonly Color BordeSuave = Color.FromArgb(75, 95, 90);          // Bordes finos
    public static readonly Color FilaOcupacion = Color.FromArgb(66, 58, 32);       // Dorado oscuro para la grilla

    public static void Aplicar(Form form)
    {
        form.BackColor = FondoVentana;
        form.ForeColor = TextoPergamino;
        form.Font = new Font("Georgia", 10F, FontStyle.Regular);

        EstilizarControles(form.Controls);
    }

    private static void EstilizarControles(Control.ControlCollection controles)
    {
        foreach (Control c in controles)
        {
            switch (c)
            {
                case Button btn:
                    EstilizarBoton(btn);
                    break;

                case TextBox txt:
                    txt.BackColor = FondoInput;
                    txt.ForeColor = TextoPergamino;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ListBox lst:
                    lst.BackColor = FondoInput;
                    lst.ForeColor = TextoPergamino;
                    lst.BorderStyle = BorderStyle.FixedSingle;
                    lst.Font = new Font("Segoe UI", 10F);
                    break;

                case ComboBox cbo:
                    cbo.BackColor = FondoInput;
                    cbo.ForeColor = TextoPergamino;
                    cbo.FlatStyle = FlatStyle.Flat;
                    break;

                case NumericUpDown num:
                    num.BackColor = FondoInput;
                    num.ForeColor = TextoPergamino;
                    num.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case GroupBox gb:
                    gb.BackColor = FondoPanel;
                    gb.ForeColor = DoradoAntiguo;
                    gb.Font = new Font("Georgia", 10.5F, FontStyle.Bold);
                    EstilizarControles(gb.Controls);
                    break;

                case FlowLayoutPanel flp:
                    flp.BackColor = FondoVentana;
                    EstilizarControles(flp.Controls);
                    break;

                case DataGridView dgv:
                    EstilizarGrilla(dgv);
                    break;

                case Label lbl:
                    // Si es un título grande, lo pintamos en Dorado Antiguo
                    if (lbl.Font.Size >= 13F)
                        lbl.ForeColor = DoradoAntiguo;
                    else
                        lbl.ForeColor = TextoPergamino;
                    break;
            }

            if (c.HasChildren && c is not GroupBox && c is not FlowLayoutPanel)
                EstilizarControles(c.Controls);
        }
    }

    public static void EstilizarBoton(Button btn)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderColor = DoradoAntiguo;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.MouseOverBackColor = VerdeHover;
        btn.BackColor = VerdeArcano;
        btn.ForeColor = TextoPergamino;
        btn.Cursor = Cursors.Hand;
        btn.Font = new Font("Georgia", 10F, FontStyle.Bold);

        btn.MouseDown -= SonidoBoton_MouseDown;
        btn.MouseDown += SonidoBoton_MouseDown;
    }

    private static void SonidoBoton_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            InvestigadorCthulhu.Modelo.GestorSonido.ReproducirEfecto("boton.mp3", 900);
    }


    private static void EstilizarGrilla(DataGridView dgv)
    {
        dgv.BackgroundColor = FondoPanel;
        dgv.BorderStyle = BorderStyle.FixedSingle;
        dgv.GridColor = BordeSuave;
        dgv.EnableHeadersVisualStyles = false;

        // Encabezados de la tabla
        dgv.ColumnHeadersDefaultCellStyle.BackColor = VerdeArcano;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = DoradoAntiguo;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Georgia", 10F, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = VerdeArcano;

        // Celdas normales
        dgv.DefaultCellStyle.BackColor = FondoPanel;
        dgv.DefaultCellStyle.ForeColor = TextoPergamino;
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 95, 85);
        dgv.DefaultCellStyle.SelectionForeColor = Color.White;
        dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
    }

    /// <summary>
    /// Muestra un cuadro de diálogo con el tema oscuro de Cthulhu en vez del MessageBox blanco de Windows.
    /// </summary>
    public static void MostrarMensaje(string texto, string titulo = "Aviso")
    {
        using var formAviso = new Form
        {
            Text = titulo,
            Size = new Size(420, 210),
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var lblTitulo = new Label
        {
            Text = titulo,
            Font = new Font("Georgia", 13F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(22, 18)
        };

        var lblMensaje = new Label
        {
            Text = texto,
            Font = new Font("Georgia", 10.5F, FontStyle.Regular),
            MaximumSize = new Size(365, 0),
            AutoSize = true,
            Location = new Point(22, 55)
        };

        var btnAceptar = new Button
        {
            Text = "Aceptar",
            Size = new Size(130, 36),
            Location = new Point(138, 120),
            DialogResult = DialogResult.OK
        };

        formAviso.Controls.Add(lblTitulo);
        formAviso.Controls.Add(lblMensaje);
        formAviso.Controls.Add(btnAceptar);
        formAviso.AcceptButton = btnAceptar;

        Aplicar(formAviso);
        formAviso.ShowDialog();
    }
}