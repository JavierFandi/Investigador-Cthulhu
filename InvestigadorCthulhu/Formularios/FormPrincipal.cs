using InvestigadorCthulhu.Datos;
using InvestigadorCthulhu.Modelo;

namespace InvestigadorCthulhu.Formularios;

public partial class FormPrincipal : Form
{
    private readonly Repositorio repo = new();

    public FormPrincipal()
    {
        InitializeComponent();
        TemaCthulhu.Aplicar(this);
    }

    private void FormPrincipal_Load(object? sender, EventArgs e)
    {
        GestorSonido.ReproducirMusica("ambiente.mp3");
        CargarLista();
    }

    private void CargarLista()
    {
        lstInvestigadores.DataSource = repo.ListarInvestigadores();
        btnJugar.Enabled = lstInvestigadores.Items.Count > 0;
        ActualizarBotonContinuar();
    }

    private void lstInvestigadores_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ActualizarBotonContinuar();
    }

    private void ActualizarBotonContinuar()
    {
        if (lstInvestigadores.SelectedItem is ItemInvestigador item)
            btnContinuar.Enabled = repo.ObtenerPartida(item.Id) != null;
        else
            btnContinuar.Enabled = false;
    }

    private void btnCrear_Click(object? sender, EventArgs e)
    {
        this.Hide(); // Oculta el menú principal
        using var form = new FormCrearPersonaje();
        if (form.ShowDialog() == DialogResult.OK)
            CargarLista();
        this.Show(); // Vuelve a mostrar el menú al cerrar Crear Personaje
    }

    private void btnJugar_Click(object? sender, EventArgs e)
    {
        if (lstInvestigadores.SelectedItem is not ItemInvestigador item)
        {
            TemaCthulhu.MostrarMensaje("Elegí un investigador de la lista.", "Atención");
            return;
        }
        Investigador inv = repo.CargarInvestigador(item.Id);

        this.Hide(); // Oculta el menú principal
        using var form = new FormJugar(inv);
        form.ShowDialog();
        this.Show(); // Vuelve a mostrar el menú al salir de la partida

        ActualizarBotonContinuar();
    }

    private void btnContinuar_Click(object? sender, EventArgs e)
    {
        if (lstInvestigadores.SelectedItem is not ItemInvestigador item) return;
        PartidaGuardada? partida = repo.ObtenerPartida(item.Id);
        if (partida == null)
        {
            TemaCthulhu.MostrarMensaje("Este investigador no tiene una partida en curso.", "Atención");
            return;
        }

        Investigador inv = repo.CargarInvestigador(item.Id);

        this.Hide(); // Oculta el menú principal
        using var form = new FormJugar(inv, partida);
        form.ShowDialog();
        this.Show(); // Vuelve a mostrar el menú al salir de la partida

        ActualizarBotonContinuar();
    }
}