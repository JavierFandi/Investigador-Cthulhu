namespace InvestigadorCthulhu.Modelo;

public class Investigador
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public int OcupacionId { get; set; }
    public string Ocupacion { get; set; } = "";
    public int Edad { get; set; } = 25;

    // Características
    public int FUE { get; set; }
    public int CON { get; set; }
    public int TAM { get; set; }
    public int DES { get; set; }
    public int APA { get; set; }
    public int INT { get; set; }
    public int POD { get; set; }
    public int EDU { get; set; }
    public int Suerte { get; set; }

    // Atributos derivados (capítulo 3 del manual)
    public int PVMaximos => (CON + TAM) / 10;
    public int PMMaximos => POD / 5;
    public int MOV
    {
        get
        {
            // ✏️ TU TURNO 2: devolvé 7, 8 o 9 según la tabla de reglas.

            if (DES < TAM && FUE < TAM) return 7;
            if (DES > TAM && FUE > TAM) return 9;
            return 8;
        }
    }
    public string BD { get; set; } = "0";
    public int Corpulencia { get; set; }
    public int Credito { get; set; }

    // Estado durante la partida
    public int PV { get; set; }
    public int Cordura { get; set; }
    public int PM { get; set; }

    public Dictionary<string, int> Habilidades { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public void TirarCaracteristicas()
    {
        FUE = Dados.Tirar(3, 6) * 5;
        CON = Dados.Tirar(3, 6) * 5;
        DES = Dados.Tirar(3, 6) * 5;
        APA = Dados.Tirar(3, 6) * 5;
        POD = Dados.Tirar(3, 6) * 5;
        TAM = (Dados.Tirar(2, 6) + 6) * 5;
        INT = (Dados.Tirar(2, 6) + 6) * 5;
        EDU = (Dados.Tirar(2, 6) + 6) * 5;
        Suerte = Dados.Tirar(3, 6) * 5;
    }

    /// <summary>Deja PV, Cordura y PM al máximo antes de empezar a jugar.</summary>
    public void ReiniciarEstado()
    {
        PV = PVMaximos;
        Cordura = POD;
        PM = PMMaximos;
    }

    /// <summary>Devuelve el valor de una característica (FUE, POD, SUERTE...) o de una habilidad.</summary>
    public int ValorDe(string nombre) => nombre.ToUpper() switch
    {
        "FUE" => FUE,
        "CON" => CON,
        "TAM" => TAM,
        "DES" => DES,
        "APA" => APA,
        "INT" => INT,
        "POD" => POD,
        "EDU" => EDU,
        "SUERTE" => Suerte,
        _ => Habilidades.TryGetValue(nombre, out int v) ? v : 0
    };
}