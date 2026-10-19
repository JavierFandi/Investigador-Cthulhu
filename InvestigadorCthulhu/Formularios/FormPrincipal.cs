using InvestigadorCthulhu.Datos;
using InvestigadorCthulhu.Modelo;

namespace InvestigadorCthulhu.Formularios;

public partial class FormPrincipal : Form
{
    private readonly Repositorio repo = new();

    public FormPrincipal()
    {
        InitializeComponent();
    }

    private void FormPrincipal_Load(object? sender, EventArgs e)
    {
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
        using var form = new FormCrearPersonaje();
        if (form.ShowDialog(this) == DialogResult.OK)
            CargarLista();
    }

    private void btnJugar_Click(object? sender, EventArgs e)
    {
        if (lstInvestigadores.SelectedItem is not ItemInvestigador item)
        {
            MessageBox.Show("Elegí un investigador de la lista.");
            return;
        }
        Investigador inv = repo.CargarInvestigador(item.Id);
        using var form = new FormJugar(inv);
        form.ShowDialog(this);
        ActualizarBotonContinuar();
    }

    private void btnContinuar_Click(object? sender, EventArgs e)
    {
        if (lstInvestigadores.SelectedItem is not ItemInvestigador item) return;
        PartidaGuardada? partida = repo.ObtenerPartida(item.Id);
        if (partida == null)
        {
            MessageBox.Show("Este investigador no tiene una partida en curso.");
            return;
        }

        Investigador inv = repo.CargarInvestigador(item.Id);
        using var form = new FormJugar(inv, partida);
        form.ShowDialog(this);
        ActualizarBotonContinuar();
    }
}