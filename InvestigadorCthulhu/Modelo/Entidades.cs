using System.Text.RegularExpressions;

namespace InvestigadorCthulhu.Modelo;

public class Ocupacion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string FormulaPuntos { get; set; } = "";
    public int CreditoMin { get; set; }
    public int CreditoMax { get; set; }
    public string HabilidadesTexto { get; set; } = "";
    public string Contactos { get; set; } = "";

    /// <summary>
    /// Devuelve las opciones para elegir (ej. ["DES×2", "FUE×2"]) si la fórmula tiene más de un término.
    /// </summary>
    public List<string> ObtenerOpcionesCaracteristica()
    {
        var terminos = Regex.Matches(FormulaPuntos, @"(FUE|CON|TAM|DES|APA|INT|POD|EDU)×(\d)");
        var opciones = new List<string>();
        for (int i = 1; i < terminos.Count; i++)
            opciones.Add(terminos[i].Value);
        return opciones;
    }

    /// <summary>
    /// Calcula los puntos de ocupación. Si el jugador eligió una característica en el ComboBox, usa esa.
    /// </summary>
    public int CalcularPuntos(Investigador inv, string? opcionElegida = null)
    {
        var terminos = Regex.Matches(FormulaPuntos, @"(FUE|CON|TAM|DES|APA|INT|POD|EDU)×(\d)");
        if (terminos.Count == 0) return inv.EDU * 4;

        int fijo = inv.ValorDe(terminos[0].Groups[1].Value) * int.Parse(terminos[0].Groups[2].Value);
        if (terminos.Count == 1) return fijo;

        if (!string.IsNullOrEmpty(opcionElegida))
        {
            var m = Regex.Match(opcionElegida, @"(FUE|CON|TAM|DES|APA|INT|POD|EDU)×(\d)");
            if (m.Success)
                return fijo + inv.ValorDe(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
        }

        int mejorOpcion = 0;
        for (int i = 1; i < terminos.Count; i++)
        {
            int valor = inv.ValorDe(terminos[i].Groups[1].Value) * int.Parse(terminos[i].Groups[2].Value);
            mejorOpcion = Math.Max(mejorOpcion, valor);
        }
        return fijo + mejorOpcion;
    }

    public override string ToString() => Nombre;
}

/// <summary>Una fila de la grilla de habilidades en la pantalla de creación.</summary>
public class HabilidadFila
{
    public string Nombre { get; set; } = "";
    public int Base { get; set; }
    public bool DeOcupacion { get; set; }
    public int PuntosOcupacion { get; set; }
    public int PuntosInteres { get; set; }
    public int Total => Base + PuntosOcupacion + PuntosInteres;
}

public class Escena
{
    public string Codigo { get; set; } = "";
    public string Titulo { get; set; } = "";
    public string Texto { get; set; } = "";
    public string? TiradaCordura { get; set; }
    public bool EsFinal { get; set; }
    public string? TipoFinal { get; set; }
}

public class Opcion
{
    public int Id { get; set; }
    public int Orden { get; set; }
    public string Texto { get; set; } = "";
    public string? Habilidad { get; set; }
    public string Dificultad { get; set; } = "Normal";
    public string EscenaExito { get; set; } = "";
    public string? EscenaFallo { get; set; }
    public string? TextoExito { get; set; }
    public string? TextoFallo { get; set; }
    public string? DanoFallo { get; set; }
    public string? CorduraFallo { get; set; }

    public override string ToString()
    {
        if (Habilidad == null) return Texto;
        string dif = Dificultad == "Normal" ? "" : $" · {Dificultad}";
        return $"{Texto}   [{Habilidad}{dif}]";
    }
}

public record ItemInvestigador(int Id, string Texto)
{
    public override string ToString() => Texto;
}

public record PartidaGuardada(int InvestigadorId, string EscenaCodigo, int PV, int Cordura, int PM);