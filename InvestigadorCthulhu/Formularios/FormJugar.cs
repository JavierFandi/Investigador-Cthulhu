using InvestigadorCthulhu.Datos;
using InvestigadorCthulhu.Modelo;

namespace InvestigadorCthulhu.Formularios;

public partial class FormJugar : Form
{
    private readonly MotorHistoria motor;
    private readonly PartidaGuardada? partidaInicial;

    public FormJugar(Investigador inv, PartidaGuardada? partida = null)
    {
        InitializeComponent();
        TemaCthulhu.Aplicar(this);
        partidaInicial = partida;
        motor = new MotorHistoria(new Repositorio(), inv);
        motor.Mensaje += AgregarAlRegistro;
    }

    private void FormJugar_Load(object? sender, EventArgs e)
    {
        if (partidaInicial != null)
            motor.Continuar(partidaInicial);
        else
            motor.Comenzar();

        MostrarEscena();
    }

    private void AgregarAlRegistro(string texto)
    {
        lstRegistro.Items.Add(texto);
        lstRegistro.TopIndex = lstRegistro.Items.Count - 1;

        // Efectos de sonido automáticos según el evento de la historia
        if (texto.StartsWith("💔") || texto.StartsWith("😱") || texto.StartsWith("💀"))
            GestorSonido.ReproducirEfecto("hit.mp3");
        else if (texto.StartsWith("🎲") || texto.StartsWith("🔥"))
            GestorSonido.ReproducirEfecto(texto.Contains('✔') ? "exito.mp3" : "dados.mp3");
    }

    private void MostrarEscena()
    {
        Escena escena = motor.EscenaActual;
        Investigador inv = motor.Inv;

        lblTitulo.Text = escena.Titulo;
        txtTexto.Text = escena.Texto;
        lblEstado.Text = $"{inv.Nombre}  ·  PV {inv.PV}/{inv.PVMaximos}  ·  Cordura {inv.Cordura}/{inv.POD}  ·  Suerte {inv.Suerte}";

        flpOpciones.Controls.Clear();

        // Desafío Extra: Si acaba de fallar una tirada, ofrecer Forzar tirada o Aceptar el fallo
        if (motor.OpcionForzable != null)
        {
            flpOpciones.Controls.Add(CrearBoton($" Forzar tirada de {motor.OpcionForzable.Habilidad} (riesgo mayor)", "FORZAR"));
            flpOpciones.Controls.Add(CrearBoton("Aceptar el fallo y continuar", "ACEPTAR_FALLO"));
            return;
        }

        foreach (Opcion op in motor.OpcionesActuales)
            flpOpciones.Controls.Add(CrearBoton(op.ToString(), op));

        if (escena.EsFinal)
        {
            AgregarAlRegistro($"— FIN ({escena.TipoFinal}) —");
            flpOpciones.Controls.Add(CrearBoton("Jugar de nuevo", "REINICIAR"));
            flpOpciones.Controls.Add(CrearBoton("Cerrar", "CERRAR"));
        }
    }

    private Button CrearBoton(string texto, object etiqueta)
    {
        var boton = new Button
        {
            Text = texto,
            Tag = etiqueta,
            Width = flpOpciones.ClientSize.Width - 25,
            Height = 46,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
            Margin = new Padding(3, 4, 3, 4)
        };

        TemaCthulhu.EstilizarBoton(boton);

        if (etiqueta is "FORZAR")
        {
            boton.BackColor = Color.FromArgb(115, 35, 35); // Rojo sangre oscuro para tirada forzada
            boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(150, 45, 45);
        }

        boton.Click += Boton_Click;
        return boton;
    }

    private void Boton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button boton) return;

        switch (boton.Tag)
        {
            case Opcion op:
                motor.Elegir(op);
                MostrarEscena();
                break;

            case "FORZAR":
                motor.ForzarTirada();
                MostrarEscena();
                break;

            case "ACEPTAR_FALLO":
                motor.AceptarFallo();
                MostrarEscena();
                break;

            case "REINICIAR":
                lstRegistro.Items.Clear();
                motor.Comenzar();
                MostrarEscena();
                break;

            case "CERRAR":
                Close();
                break;
        }
    }

    // Desafío Extra: Ver ficha (habilidades mayores a 40)
    // Desafío Extra: Ver ficha con diseño personalizado Cthulhu
    private void btnVerFicha_Click(object? sender, EventArgs e)
    {
        Investigador inv = motor.Inv;
        var destacadas = inv.Habilidades
            .Where(h => h.Value > 40)
            .OrderByDescending(h => h.Value)
            .Select(h => $"  • {h.Key}: {h.Value}%")
            .ToList();

        string listado = destacadas.Count > 0
            ? string.Join("\r\n", destacadas)
            : "  Ninguna habilidad supera el 40%.";

        using var formFicha = new Form
        {
            Text = $"Ficha de {inv.Nombre}",
            Size = new Size(440, 480),
            StartPosition = FormStartPosition.CenterScreen,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var lblTituloFicha = new Label
        {
            Text = $"{inv.Nombre} — {inv.Ocupacion}",
            Font = new Font("Georgia", 13F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 18)
        };

        var lblSubtitulo = new Label
        {
            Text = $"FUE {inv.FUE} · CON {inv.CON} · TAM {inv.TAM} · DES {inv.DES} · INT {inv.INT} · POD {inv.POD} · EDU {inv.EDU}",
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(20, 50)
        };

        var txtHabilidades = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Location = new Point(20, 82),
            Size = new Size(385, 290),
            Font = new Font("Georgia", 10.5F),
            Text = $"HABILIDADES DESTACADAS (>40%):\r\n\r\n{listado}"
        };

        var btnCerrarFicha = new Button
        {
            Text = "Volver a la partida",
            Location = new Point(120, 388),
            Size = new Size(185, 36),
            DialogResult = DialogResult.OK
        };

        formFicha.Controls.Add(lblTituloFicha);
        formFicha.Controls.Add(lblSubtitulo);
        formFicha.Controls.Add(txtHabilidades);
        formFicha.Controls.Add(btnCerrarFicha);

        // Aplicamos el mismo tema visual oscuro y deseleccionamos el texto al abrir
        TemaCthulhu.Aplicar(formFicha);
        formFicha.Shown += (_, _) => txtHabilidades.SelectionLength = 0;

        //this.Hide(); // Oculta la ventana de juego mientras ves la ficha
        formFicha.ShowDialog();
        //this.Show(); // Vuelve a mostrar la ventana de juego al cerrar la ficha
    }
}