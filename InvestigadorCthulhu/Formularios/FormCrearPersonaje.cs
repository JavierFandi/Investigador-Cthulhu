using System.ComponentModel;
using InvestigadorCthulhu.Datos;
using InvestigadorCthulhu.Modelo;

namespace InvestigadorCthulhu.Formularios;

public partial class FormCrearPersonaje : Form
{
    private readonly Repositorio repo = new();
    private readonly Investigador inv = new();
    private BindingList<HabilidadFila> filas = new();
    private int puntosOcupacion;
    private int puntosInteres;

    public FormCrearPersonaje()
    {
        InitializeComponent();
    }

    private void FormCrearPersonaje_Load(object? sender, EventArgs e)
    {
        inv.TirarCaracteristicas();
        MostrarCaracteristicas();
        cboOcupacion.DataSource = repo.ObtenerOcupaciones();
        cboOcupacion_SelectedIndexChanged(cboOcupacion, EventArgs.Empty);   // muestra la primera ocupación
    }

    // ---------------------------------------------------------------
    //  Paso 1: características
    // ---------------------------------------------------------------
    private void btnTirar_Click(object? sender, EventArgs e)
    {
        inv.TirarCaracteristicas();
        MostrarCaracteristicas();
        CargarHabilidades();   // Esquivar y Lengua propia cambian, y los puntos también
    }

    private void MostrarCaracteristicas()
    {
        lblFUE.Text = $"FUE {inv.FUE}";
        lblCON.Text = $"CON {inv.CON}";
        lblTAM.Text = $"TAM {inv.TAM}";
        lblDES.Text = $"DES {inv.DES}";
        lblAPA.Text = $"APA {inv.APA}";
        lblINT.Text = $"INT {inv.INT}";
        lblPOD.Text = $"POD {inv.POD}";
        lblEDU.Text = $"EDU {inv.EDU}";
        lblSuerte.Text = $"Suerte {inv.Suerte}";

        (inv.BD, inv.Corpulencia) = repo.ObtenerBonificacionDano(inv.FUE + inv.TAM);
        lblDerivados.Text =
            $"PV {inv.PVMaximos}   ·   Cordura {inv.POD}   ·   PM {inv.PMMaximos}   ·   " +
            $"MOV {inv.MOV}   ·   BD {inv.BD}   ·   Corpulencia {inv.Corpulencia}";
    }

    // ---------------------------------------------------------------
    //  Paso 2: ocupación
    // ---------------------------------------------------------------
    private void cboOcupacion_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboOcupacion.SelectedItem is not Ocupacion oc) return;
        txtInfoOcupacion.Text =
            $"Puntos: {oc.FormulaPuntos}\r\n" +
            $"Crédito: {oc.CreditoMin} a {oc.CreditoMax}\r\n" +
            $"Habilidades: {oc.HabilidadesTexto}\r\n" +
            $"Contactos: {oc.Contactos}";
        CargarHabilidades();
    }

    // ---------------------------------------------------------------
    //  Paso 3: habilidades
    // ---------------------------------------------------------------
    private void CargarHabilidades()
    {
        if (cboOcupacion.SelectedItem is not Ocupacion oc) return;

        HashSet<string> deOcupacion = repo.ObtenerHabilidadesDeOcupacion(oc.Id);
        var lista = repo.ObtenerHabilidadesBase(inv);

        foreach (var f in lista)
        {
            // "Combatir (Pelea)" cuenta como de ocupación si la ocupación incluye "Combatir"
            string nombreGeneral = f.Nombre.Split(" (")[0];
            f.DeOcupacion = deOcupacion.Contains(f.Nombre)
                         || deOcupacion.Contains(nombreGeneral)
                         || f.Nombre == "Crédito";   // Crédito siempre es de ocupación
        }

        filas = new BindingList<HabilidadFila>(lista);
        dgvHabilidades.DataSource = filas;

        dgvHabilidades.Columns[nameof(HabilidadFila.Nombre)].ReadOnly = true;
        dgvHabilidades.Columns[nameof(HabilidadFila.Base)].ReadOnly = true;
        dgvHabilidades.Columns[nameof(HabilidadFila.DeOcupacion)].ReadOnly = true;
        dgvHabilidades.Columns[nameof(HabilidadFila.DeOcupacion)].HeaderText = "Ocupación";
        dgvHabilidades.Columns[nameof(HabilidadFila.PuntosOcupacion)].HeaderText = "+ Ocup.";
        dgvHabilidades.Columns[nameof(HabilidadFila.PuntosInteres)].HeaderText = "+ Interés";

        puntosOcupacion = oc.CalcularPuntos(inv);
        puntosInteres = inv.INT * 2;
        ActualizarPuntos();
    }

    private void dgvHabilidades_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow row in dgvHabilidades.Rows)
        {
            if (row.DataBoundItem is HabilidadFila f && f.DeOcupacion)
                row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
        }
    }

    private void dgvHabilidades_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        ActualizarPuntos();
    }

    private void dgvHabilidades_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        MessageBox.Show("Ingresá un número entero.");
        e.Cancel = true;
    }

    private void ActualizarPuntos()
    {
        int usadosOcup = filas.Sum(f => f.PuntosOcupacion);
        int usadosInt = filas.Sum(f => f.PuntosInteres);

        lblPuntosOcupacion.Text = $"Ocupación: {puntosOcupacion - usadosOcup} de {puntosOcupacion}";
        lblPuntosInteres.Text = $"Interés: {puntosInteres - usadosInt} de {puntosInteres}";

        lblPuntosOcupacion.ForeColor = usadosOcup > puntosOcupacion ? Color.Red : SystemColors.ControlText;
        lblPuntosInteres.ForeColor = usadosInt > puntosInteres ? Color.Red : SystemColors.ControlText;

        dgvHabilidades.Refresh();   // para que se vea el Total actualizado
    }

    // ---------------------------------------------------------------
    //  Paso 4: validar y guardar
    // ---------------------------------------------------------------
    private string? Validar(Ocupacion oc)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
            return "El investigador necesita un nombre.";

        if (filas.Any(f => f.PuntosOcupacion < 0 || f.PuntosInteres < 0))
            return "No se pueden asignar puntos negativos.";

        if (filas.Any(f => !f.DeOcupacion && f.PuntosOcupacion > 0))
            return "Los puntos de ocupación solo van en las habilidades marcadas como de ocupación.";

        if (filas.Sum(f => f.PuntosOcupacion) > puntosOcupacion)
            return "Gastaste más puntos de ocupación de los que tenés.";

        if (filas.Sum(f => f.PuntosInteres) > puntosInteres)
            return "Gastaste más puntos de interés de los que tenés.";

        if (filas.Any(f => f.Total > 99))
            return "Ninguna habilidad puede superar 99.";

        // ✏️ TU TURNO 4: el Crédito tiene que quedar entre oc.CreditoMin y oc.CreditoMax.
        // Buscá en 'filas' la fila llamada "Crédito", tomá su Total y, si está fuera de rango,
        // devolvé un mensaje que diga el rango correcto.

        if (filas.First(f => f.Nombre == "Credito") != null)
        {
            if(filas.Any(f => f.Total >= oc.CreditoMax || f.Total <= oc.CreditoMin))
            {
                return "Tu Rango de Crédito es " + oc.CreditoMin + " a " + oc.CreditoMax;
            }
        }

        return null;
    }

    private void btnGuardar_Click(object? sender, EventArgs e)
    {
        dgvHabilidades.EndEdit();
        ActualizarPuntos();

        if (cboOcupacion.SelectedItem is not Ocupacion oc) return;

        string? error = Validar(oc);
        if (error != null)
        {
            MessageBox.Show(error, "Revisá el investigador", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        inv.Nombre = txtNombre.Text.Trim();
        inv.Edad = (int)numEdad.Value;
        inv.OcupacionId = oc.Id;
        inv.Ocupacion = oc.Nombre;
        inv.Habilidades.Clear();
        foreach (var f in filas)
            inv.Habilidades[f.Nombre] = f.Total;
        inv.Habilidades["Mitos de Cthulhu"] = 0;
        inv.Credito = inv.Habilidades["Crédito"];

        repo.GuardarInvestigador(inv);
        MessageBox.Show($"{inv.Nombre} está listo para investigar.", "Guardado");
        DialogResult = DialogResult.OK;
    }
}

