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
    /// Calcula los puntos de ocupación a partir de la fórmula guardada en la base,
    /// por ejemplo "EDU×4" o "EDU×2 + elegir entre DES×2 o FUE×2".
    /// Simplificación: cuando hay que elegir, se toma la característica más alta.
    /// </summary>
    public int CalcularPuntos(Investigador inv)
    {
        var terminos = Regex.Matches(FormulaPuntos, @"(FUE|CON|TAM|DES|APA|INT|POD|EDU)×(\d)");
        if (terminos.Count == 0) return inv.EDU * 4;

        int fijo = inv.ValorDe(terminos[0].Groups[1].Value) * int.Parse(terminos[0].Groups[2].Value);
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