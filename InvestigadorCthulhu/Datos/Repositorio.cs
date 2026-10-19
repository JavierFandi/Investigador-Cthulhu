using InvestigadorCthulhu.Modelo;
using Microsoft.Data.Sqlite;

namespace InvestigadorCthulhu.Datos;

/// <summary>
/// Todas las consultas SQL del proyecto viven acá.
/// </summary>
public class Repositorio
{
    private static string? Texto(SqliteDataReader r, int i) => r.IsDBNull(i) ? null : r.GetString(i);

    // ---------------------------------------------------------------
    //  Creación de personaje
    // ---------------------------------------------------------------
    public List<Ocupacion> ObtenerOcupaciones()
    {
        var lista = new List<Ocupacion>();
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT id, nombre_completo, puntos_habilidad, credito_min, credito_max,
                   habilidades_texto, contactos_sugeridos
            FROM ocupaciones
            ORDER BY nombre_completo;";
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Ocupacion
            {
                Id = r.GetInt32(0),
                Nombre = r.GetString(1),
                FormulaPuntos = Texto(r, 2) ?? "EDU×4",
                CreditoMin = r.IsDBNull(3) ? 0 : r.GetInt32(3),
                CreditoMax = r.IsDBNull(4) ? 99 : r.GetInt32(4),
                HabilidadesTexto = Texto(r, 5) ?? "",
                Contactos = Texto(r, 6) ?? ""
            });
        }
        return lista;
    }

    /// <summary>Nombres de las habilidades que la ocupación incluye (tabla ocupacion_habilidades).</summary>
    public HashSet<string> ObtenerHabilidadesDeOcupacion(int ocupacionId)
    {
        var nombres = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT h.nombre
            FROM ocupacion_habilidades oh
            JOIN habilidades h ON h.id = oh.habilidad_id
            WHERE oh.ocupacion_id = $id;";
        cmd.Parameters.AddWithValue("$id", ocupacionId);
        using var r = cmd.ExecuteReader();
        while (r.Read())
            nombres.Add(r.GetString(0));
        return nombres;
    }

    /// <summary>
    /// Arma la lista de habilidades con su valor base para un investigador.
    /// Esquivar y Lengua propia dependen de las características.
    /// Combatir y Armas de fuego se dividen en especialidades: usamos las más comunes.
    /// </summary>
    public List<HabilidadFila> ObtenerHabilidadesBase(Investigador inv)
    {
        var filas = new List<HabilidadFila>();
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();

        // 1) Habilidades con valor base fijo (excepto Mitos de Cthulhu, que no se compra al crear)
        cmd.CommandText = @"
            SELECT nombre, valor_base_num
            FROM habilidades
            WHERE valor_base_num IS NOT NULL AND nombre <> 'Mitos de Cthulhu'
            ORDER BY nombre;";
        using (var r = cmd.ExecuteReader())
        {
            while (r.Read())
                filas.Add(new HabilidadFila { Nombre = r.GetString(0), Base = r.GetInt32(1) });
        }

        // 2) Especialidades de combate y armas de fuego
        cmd.CommandText = @"
            SELECT h.nombre || ' (' || e.nombre || ')', e.valor_base_num
            FROM habilidad_especialidades e
            JOIN habilidades h ON h.id = e.habilidad_id
            WHERE e.nombre IN ('Pelea', 'Arma corta', 'Fusil/Escopeta');";
        using (var r = cmd.ExecuteReader())
        {
            while (r.Read())
                filas.Add(new HabilidadFila { Nombre = r.GetString(0), Base = r.GetInt32(1) });
        }

        // 3) Las que dependen de características
        filas.Add(new HabilidadFila { Nombre = "Esquivar", Base = inv.DES / 2 });
        filas.Add(new HabilidadFila { Nombre = "Lengua propia", Base = inv.EDU });

        return filas.OrderBy(f => f.Nombre).ToList();
    }

    public (string bd, int corpulencia) ObtenerBonificacionDano(int fueMasTam)
    {
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT bonificacion_dano, corpulencia
            FROM bonificacion_dano
            WHERE $v BETWEEN fue_tam_min AND fue_tam_max;";
        cmd.Parameters.AddWithValue("$v", fueMasTam);
        using var r = cmd.ExecuteReader();
        return r.Read() ? (r.GetString(0), r.GetInt32(1)) : ("0", 0);
    }

    public int GuardarInvestigador(Investigador inv)
    {
        using var con = BaseDatos.Abrir();
        using var tx = con.BeginTransaction();

        using var cmd = con.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = @"
            INSERT INTO investigadores
              (nombre, ocupacion_id, edad, fue, con, tam, des, apa, intel, pod, edu,
               suerte, pv, cordura, pm, mov, bd, corpulencia, credito)
            VALUES
              ($nombre, $ocup, $edad, $fue, $con, $tam, $des, $apa, $int, $pod, $edu,
               $suerte, $pv, $cordura, $pm, $mov, $bd, $corp, $credito);
            SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$nombre", inv.Nombre);
        cmd.Parameters.AddWithValue("$ocup", inv.OcupacionId);
        cmd.Parameters.AddWithValue("$edad", inv.Edad);
        cmd.Parameters.AddWithValue("$fue", inv.FUE);
        cmd.Parameters.AddWithValue("$con", inv.CON);
        cmd.Parameters.AddWithValue("$tam", inv.TAM);
        cmd.Parameters.AddWithValue("$des", inv.DES);
        cmd.Parameters.AddWithValue("$apa", inv.APA);
        cmd.Parameters.AddWithValue("$int", inv.INT);
        cmd.Parameters.AddWithValue("$pod", inv.POD);
        cmd.Parameters.AddWithValue("$edu", inv.EDU);
        cmd.Parameters.AddWithValue("$suerte", inv.Suerte);
        cmd.Parameters.AddWithValue("$pv", inv.PVMaximos);
        cmd.Parameters.AddWithValue("$cordura", inv.POD);
        cmd.Parameters.AddWithValue("$pm", inv.PMMaximos);
        cmd.Parameters.AddWithValue("$mov", inv.MOV);
        cmd.Parameters.AddWithValue("$bd", inv.BD);
        cmd.Parameters.AddWithValue("$corp", inv.Corpulencia);
        cmd.Parameters.AddWithValue("$credito", inv.Credito);
        int id = Convert.ToInt32(cmd.ExecuteScalar());

        using var cmdHab = con.CreateCommand();
        cmdHab.Transaction = tx;
        cmdHab.CommandText = @"
            INSERT INTO investigador_habilidades (investigador_id, habilidad, valor)
            VALUES ($id, $hab, $valor);";
        var pId = cmdHab.Parameters.AddWithValue("$id", id);
        var pHab = cmdHab.Parameters.AddWithValue("$hab", "");
        var pValor = cmdHab.Parameters.AddWithValue("$valor", 0);
        foreach (var par in inv.Habilidades)
        {
            pHab.Value = par.Key;
            pValor.Value = par.Value;
            cmdHab.ExecuteNonQuery();
        }

        tx.Commit();
        inv.Id = id;
        return id;
    }

    public List<ItemInvestigador> ListarInvestigadores()
    {
        var lista = new List<ItemInvestigador>();
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT i.id, i.nombre || ' — ' || o.nombre_completo
            FROM investigadores i
            JOIN ocupaciones o ON o.id = i.ocupacion_id
            ORDER BY i.id DESC;";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            lista.Add(new ItemInvestigador(r.GetInt32(0), r.GetString(1)));
        return lista;
    }

    public Investigador CargarInvestigador(int id)
    {
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT i.nombre, i.ocupacion_id, o.nombre_completo, i.edad,
                   i.fue, i.con, i.tam, i.des, i.apa, i.intel, i.pod, i.edu,
                   i.suerte, i.bd, i.corpulencia, i.credito
            FROM investigadores i
            JOIN ocupaciones o ON o.id = i.ocupacion_id
            WHERE i.id = $id;";
        cmd.Parameters.AddWithValue("$id", id);

        Investigador inv;
        using (var r = cmd.ExecuteReader())
        {
            if (!r.Read()) throw new InvalidOperationException($"No existe el investigador {id}.");
            inv = new Investigador
            {
                Id = id,
                Nombre = r.GetString(0),
                OcupacionId = r.GetInt32(1),
                Ocupacion = r.GetString(2),
                Edad = r.GetInt32(3),
                FUE = r.GetInt32(4),
                CON = r.GetInt32(5),
                TAM = r.GetInt32(6),
                DES = r.GetInt32(7),
                APA = r.GetInt32(8),
                INT = r.GetInt32(9),
                POD = r.GetInt32(10),
                EDU = r.GetInt32(11),
                Suerte = r.GetInt32(12),
                BD = r.GetString(13),
                Corpulencia = r.GetInt32(14),
                Credito = r.GetInt32(15)
            };
        }

        cmd.CommandText = "SELECT habilidad, valor FROM investigador_habilidades WHERE investigador_id = $id;";
        using (var r = cmd.ExecuteReader())
        {
            while (r.Read())
                inv.Habilidades[r.GetString(0)] = r.GetInt32(1);
        }
        return inv;
    }

    // ---------------------------------------------------------------
    //  Historia
    // ---------------------------------------------------------------
    public Escena ObtenerEscena(string codigo)
    {
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT codigo, titulo, texto, tirada_cordura, es_final, tipo_final
            FROM escenas WHERE codigo = $codigo;";
        cmd.Parameters.AddWithValue("$codigo", codigo);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) throw new InvalidOperationException($"No existe la escena '{codigo}'.");
        return new Escena
        {
            Codigo = r.GetString(0),
            Titulo = r.GetString(1),
            Texto = r.GetString(2),
            TiradaCordura = Texto(r, 3),
            EsFinal = r.GetInt32(4) == 1,
            TipoFinal = Texto(r, 5)
        };
    }

    public List<Opcion> ObtenerOpciones(string codigoEscena)
    {
        var lista = new List<Opcion>();
        using var con = BaseDatos.Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            SELECT id, orden, texto, habilidad, dificultad, escena_exito, escena_fallo,
                   texto_exito, texto_fallo, dano_fallo, cordura_fallo
            FROM opciones
            WHERE escena_codigo = $codigo
            ORDER BY orden;";
        cmd.Parameters.AddWithValue("$codigo", codigoEscena);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            lista.Add(new Opcion
            {
                Id = r.GetInt32(0),
                Orden = r.GetInt32(1),
                Texto = r.GetString(2),
                Habilidad = Texto(r, 3),
                Dificultad = r.GetString(4),
                EscenaExito = r.GetString(5),
                EscenaFallo = Texto(r, 6),
                TextoExito = Texto(r, 7),
                TextoFallo = Texto(r, 8),
                DanoFallo = Texto(r, 9),
                CorduraFallo = Texto(r, 10)
            });
        }
        return lista;
    }
}
