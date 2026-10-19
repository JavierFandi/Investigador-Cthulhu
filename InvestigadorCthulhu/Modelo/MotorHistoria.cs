using InvestigadorCthulhu.Datos;

namespace InvestigadorCthulhu.Modelo;

public class MotorHistoria
{
    private readonly Repositorio repo;
    public Investigador Inv { get; }
    public Escena EscenaActual { get; private set; } = new();
    public List<Opcion> OpcionesActuales { get; private set; } = new();
    public Opcion? OpcionForzable { get; private set; }

    public event Action<string>? Mensaje;

    public MotorHistoria(Repositorio repo, Investigador inv)
    {
        this.repo = repo;
        Inv = inv;
    }

    public void Comenzar()
    {
        OpcionForzable = null;
        Inv.ReiniciarEstado();
        Avisar($"— Comienza la partida de {Inv.Nombre} (PV {Inv.PV}, Cordura {Inv.Cordura}) —");
        IrA("INICIO");
    }

    public void Continuar(PartidaGuardada partida)
    {
        OpcionForzable = null;
        Inv.PV = partida.PV;
        Inv.Cordura = partida.Cordura;
        Inv.PM = partida.PM;
        Avisar($"— Continuando partida de {Inv.Nombre} en '{partida.EscenaCodigo}' (PV {Inv.PV}, Cordura {Inv.Cordura}) —");
        EscenaActual = repo.ObtenerEscena(partida.EscenaCodigo);
        OpcionesActuales = EscenaActual.EsFinal ? new List<Opcion>() : repo.ObtenerOpciones(partida.EscenaCodigo);
    }

    public void Elegir(Opcion opcion)
    {
        OpcionForzable = null;

        if (opcion.Habilidad == null)
        {
            IrA(opcion.EscenaExito);
            return;
        }

        int valor = Inv.ValorDe(opcion.Habilidad);
        ResultadoTirada tirada = Tiradas.Hacer(opcion.Habilidad, valor, opcion.Dificultad);
        Avisar("🎲 " + tirada);

        if (tirada.Superada)
        {
            if (opcion.TextoExito != null) Avisar(opcion.TextoExito);
            IrA(opcion.EscenaExito);
        }
        else if (tirada.Nivel != NivelExito.Pifia)
        {
            // Desafío Extra: permitir forzar la tirada (o aceptar el fallo) antes de avanzar
            OpcionForzable = opcion;
            Avisar("⚠️ Fallaste la tirada. Podés intentar FORZAR LA TIRADA (con peores consecuencias si fallás) o aceptar el fallo.");
        }
        else
        {
            Avisar("💥 ¡Pifia! No se puede forzar la tirada.");
            AplicarFalloYAvanzar(opcion, forzada: false);
        }
    }

    public void ForzarTirada()
    {
        if (OpcionForzable == null || OpcionForzable.Habilidad == null) return;
        Opcion op = OpcionForzable;
        OpcionForzable = null;

        int valor = Inv.ValorDe(op.Habilidad);
        ResultadoTirada tirada = Tiradas.Hacer(op.Habilidad + " [FORZADA]", valor, op.Dificultad);
        Avisar("🔥 " + tirada);

        if (tirada.Superada)
        {
            if (op.TextoExito != null) Avisar(op.TextoExito);
            IrA(op.EscenaExito);
        }
        else
        {
            Avisar("💀 ¡Fallaste la tirada forzada! Las consecuencias se agravan.");
            AplicarFalloYAvanzar(op, forzada: true);
        }
    }

    public void AceptarFallo()
    {
        if (OpcionForzable == null) return;
        Opcion op = OpcionForzable;
        OpcionForzable = null;
        AplicarFalloYAvanzar(op, forzada: false);
    }

    private void AplicarFalloYAvanzar(Opcion opcion, bool forzada)
    {
        if (opcion.TextoFallo != null) Avisar(opcion.TextoFallo);

        PerderPV(opcion.DanoFallo);
        PerderCordura(opcion.CorduraFallo);

        // Si falló una tirada forzada, sufre consecuencias adicionales
        if (forzada)
        {
            PerderPV(opcion.DanoFallo ?? "1D3");
            PerderCordura(opcion.CorduraFallo ?? "1D2");
        }

        string destino = opcion.EscenaFallo ?? opcion.EscenaExito;
        IrA(destino);
    }

    private void IrA(string codigo)
    {
        if (Inv.PV <= 0) codigo = "FINAL_MUERTE";
        else if (Inv.Cordura <= 0) codigo = "FINAL_LOCURA";

        EscenaActual = repo.ObtenerEscena(codigo);
        OpcionesActuales = EscenaActual.EsFinal ? new List<Opcion>() : repo.ObtenerOpciones(codigo);

        if (!EscenaActual.EsFinal && !string.IsNullOrEmpty(EscenaActual.TiradaCordura))
        {
            TiradaDeCordura(EscenaActual.TiradaCordura);
            if (Inv.Cordura <= 0)
            {
                IrA("FINAL_LOCURA");
                return;
            }
        }

        // Desafío Extra: Guardar partida automáticamente o borrarla si terminó
        if (EscenaActual.EsFinal)
            repo.BorrarPartida(Inv.Id);
        else
            repo.GuardarPartida(Inv, EscenaActual.Codigo);
    }

    private void TiradaDeCordura(string formato)
    {
        string[] partes = formato.Split('/');
        ResultadoTirada tirada = Tiradas.Hacer("Cordura", Inv.Cordura);
        Avisar("🧠 " + tirada);

        string perdida = tirada.Superada ? partes[0] : partes[1];
        PerderCordura(perdida);
    }

    private void PerderPV(string? dados)
    {
        int dano = Dados.TirarExpresion(dados);
        if (dano <= 0) return;
        Inv.PV = Math.Max(0, Inv.PV - dano);
        Avisar($"💔 Perdés {dano} PV (te quedan {Inv.PV}).");
    }

    private void PerderCordura(string? dados)
    {
        int perdida = Dados.TirarExpresion(dados);
        if (perdida <= 0) return;
        Inv.Cordura = Math.Max(0, Inv.Cordura - perdida);
        Avisar($"😱 Perdés {perdida} de Cordura (te quedan {Inv.Cordura}).");

        // Desafío Extra: Locura temporal si pierde 5 o más puntos de Cordura en una sola tirada
        if (perdida >= 5 && Inv.Cordura > 0)
        {
            ResultadoTirada tiradaInt = Tiradas.Hacer("INT (Locura temporal)", Inv.INT);
            Avisar("🌀 " + tiradaInt);
            if (tiradaInt.Superada)
                Avisar("😵 ¡Locura temporal! Tu mente comprende el horror y sufrís un acceso de pánico.");
            else
                Avisar("😶 Tu mente bloquea el recuerdo para protegerte del trauma.");
        }
    }

    private void Avisar(string texto) => Mensaje?.Invoke(texto);
}