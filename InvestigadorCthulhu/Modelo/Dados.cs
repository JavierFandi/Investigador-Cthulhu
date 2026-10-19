namespace InvestigadorCthulhu.Modelo;

/// <summary>
/// Tiradas de dados: 1D6, 3D6, D100 y expresiones como "2D6+1".
/// </summary>
public static class Dados
{
    private static readonly Random azar = new();

    public static int Tirar(int caras) => azar.Next(1, caras + 1);

    public static int Tirar(int cantidad, int caras)
    {
        int total = 0;
        for (int i = 0; i < cantidad; i++)
            total += Tirar(caras);
        return total;
    }

    public static int D100() => Tirar(100);

    /// <summary>
    /// Interpreta "1D6", "2D6+1", "1D4+2", "3" o "1D10-1" y devuelve el resultado.
    /// </summary>
    public static int TirarExpresion(string? expresion)
    {
        if (string.IsNullOrWhiteSpace(expresion)) return 0;

        string texto = expresion.ToUpper().Replace(" ", "").Replace("-", "+-");
        int total = 0;

        foreach (string parte in texto.Split('+', StringSplitOptions.RemoveEmptyEntries))
        {
            int signo = parte.StartsWith('-') ? -1 : 1;
            string p = parte.TrimStart('-');

            if (p.Contains('D'))
            {
                string[] d = p.Split('D');
                int cantidad = d[0] == "" ? 1 : int.Parse(d[0]);
                int caras = int.Parse(d[1]);
                total += signo * Tirar(cantidad, caras);
            }
            else
            {
                total += signo * int.Parse(p);
            }
        }
        return total;
    }
}
