using System.Text;
using Microsoft.Data.Sqlite;

namespace InvestigadorCthulhu.Datos;

/// <summary>
/// Punto único de acceso al archivo SQLite.
/// </summary>
public static class BaseDatos
{
    // La base se copia junto al .exe (carpeta Datos) al compilar.
    public static string RutaArchivo =>
        Path.Combine(AppContext.BaseDirectory, "Datos", "cthulhu_guia_investigador.db");

    public static SqliteConnection Abrir()
    {
        var conexion = new SqliteConnection($"Data Source={RutaArchivo}");
        conexion.Open();

        using var cmd = conexion.CreateCommand();
        cmd.CommandText = "PRAGMA foreign_keys = ON;";
        cmd.ExecuteNonQuery();

        return conexion;
    }

    /// <summary>
    /// Si la aventura todavía no está cargada, ejecuta Datos/aventura.sql.
    /// </summary>
    public static void Inicializar()
    {
        if (!File.Exists(RutaArchivo))
            throw new FileNotFoundException("No se encontró la base de datos.", RutaArchivo);

        using var conexion = Abrir();
        using var cmd = conexion.CreateCommand();
        cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'escenas';";
        long existe = (long)cmd.ExecuteScalar()!;
        if (existe > 0) return;

        string rutaScript = Path.Combine(AppContext.BaseDirectory, "Datos", "aventura.sql");
        string script = File.ReadAllText(rutaScript);

        // Todo o nada: si una sentencia falla, no queda la aventura a medio cargar.
        using var tx = conexion.BeginTransaction();
        foreach (string sentencia in DividirSentencias(script))
        {
            using var c = conexion.CreateCommand();
            c.Transaction = tx;
            c.CommandText = sentencia;
            c.ExecuteNonQuery();
        }
        tx.Commit();
    }

    /// <summary>
    /// Separa un script SQL en sentencias por ';', ignorando los ';' que están
    /// dentro de textos entre comillas simples y los comentarios que empiezan con --.
    /// </summary>
    public static List<string> DividirSentencias(string script)
    {
        var sentencias = new List<string>();
        var actual = new StringBuilder();
        bool dentroDeTexto = false;

        for (int i = 0; i < script.Length; i++)
        {
            char c = script[i];

            // Comentario de línea: saltear hasta el final de la línea
            if (!dentroDeTexto && c == '-' && i + 1 < script.Length && script[i + 1] == '-')
            {
                while (i < script.Length && script[i] != '\n') i++;
                actual.Append('\n');
                continue;
            }

            if (c == '\'') dentroDeTexto = !dentroDeTexto;

            if (c == ';' && !dentroDeTexto)
            {
                if (actual.ToString().Trim().Length > 0) sentencias.Add(actual.ToString().Trim());
                actual.Clear();
            }
            else
            {
                actual.Append(c);
            }
        }

        if (actual.ToString().Trim().Length > 0) sentencias.Add(actual.ToString().Trim());
        return sentencias;
    }
}