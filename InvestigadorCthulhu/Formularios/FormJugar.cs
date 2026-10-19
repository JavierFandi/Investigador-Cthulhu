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
            Height = 44,
            TextAlign = ContentAlignment.MiddleLeft
        };
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
    private void btnVerFicha_Click(object? sender, EventArgs e)
    {
        Investigador inv = motor.Inv;
        var destacadas = inv.Habilidades
            .Where(h => h.Value > 40)
            .OrderByDescending(h => h.Value)
            .Select(h => $"• {h.Key}: {h.Value}%")
            .ToList();

        string listado = destacadas.Count > 0
            ? string.Join("\n", destacadas)
            : "Ninguna habilidad supera el 40%.";

        MessageBox.Show(
            $"Ficha de {inv.Nombre} ({inv.Ocupacion})\n\nHabilidades mayores a 40%:\n{listado}",
            "Ficha del Investigador",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}