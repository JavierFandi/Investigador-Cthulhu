namespace InvestigadorCthulhu.Modelo;

// El orden importa: cuanto más abajo, mejor resultado.
public enum NivelExito { Pifia, Fallo, Normal, Dificil, Extremo, Critico }

public record ResultadoTirada(string Nombre, int Valor, string Dificultad, int Dado, NivelExito Nivel, bool Superada)
{
    public override string ToString() =>
        $"{Nombre} ({Valor}%, {Dificultad}): sacaste {Dado} → {Nivel} {(Superada ? "✔" : "✘")}";
}

public static class Tiradas
{
    public static ResultadoTirada Hacer(string nombre, int valor, string dificultad = "Normal")
    {
        int dado = Dados.D100();
        NivelExito nivel;

        // ✏️ TU TURNO 3: asigná a 'nivel' el resultado según la tabla de tiradas.
        // Revisá en este orden: Crítico, Pifia, Extremo, Difícil, Normal y, si nada se cumple, Fallo.

        if (dado == 1)
        {
            nivel = NivelExito.Critico;
        } 
        else if ((valor < 50 && dado >= 96) || (valor > 50 && dado == 100))
        {
            nivel = NivelExito.Pifia;
        }
        else if (dado <= (valor/5))
        {
            nivel = NivelExito.Extremo;
        }
        else if (dado <= (valor/2))
        {
            nivel = NivelExito.Dificil;
        }
        else if (dado <= valor)
        {
            nivel = NivelExito.Normal;
        }
        else
        {
            nivel = NivelExito.Fallo;
        }

            NivelExito requerido = dificultad switch
            {
                "Difícil" => NivelExito.Dificil,
                "Extrema" => NivelExito.Extremo,
                _ => NivelExito.Normal
            };

        bool superada = nivel >= requerido;
        return new ResultadoTirada(nombre, valor, dificultad, dado, nivel, superada);
    }
}
