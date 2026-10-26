using System.Runtime.InteropServices;

namespace InvestigadorCthulhu.Modelo;

public static class GestorSonido
{
    [DllImport("winmm.dll", CharSet = CharSet.Auto)]
    private static extern int mciSendString(string command, string? buffer, int bufferSize, IntPtr hwndCallback);

    public static bool Silenciado { get; private set; } = false;
    private static int contadorEfectos = 0;

    /// <summary>
    /// Busca el archivo en la carpeta Sonidos. Si le pasás "boton.wav" pero tenés "boton.mp3",
    /// encuentra automáticamente el .mp3 (y viceversa).
    /// </summary>
    private static string? BuscarRutaAudio(string nombreArchivo)
    {
        string carpeta = Path.Combine(AppContext.BaseDirectory, "Sonidos");
        if (!Directory.Exists(carpeta)) return null;

        string rutaExacta = Path.Combine(carpeta, nombreArchivo);
        if (File.Exists(rutaExacta)) return rutaExacta;

        string nombreSinExt = Path.GetFileNameWithoutExtension(nombreArchivo);

        string rutaMp3 = Path.Combine(carpeta, nombreSinExt + ".mp3");
        if (File.Exists(rutaMp3)) return rutaMp3;

        string rutaWav = Path.Combine(carpeta, nombreSinExt + ".wav");
        if (File.Exists(rutaWav)) return rutaWav;

        return null;
    }

    /// <summary>
    /// Reproduce música de fondo en bucle continuo (.mp3).
    /// </summary>
    public static void ReproducirMusica(string nombreArchivo = "ambiente.mp3", int volumen = 350)
    {
        if (Silenciado) return;

        string? ruta = BuscarRutaAudio(nombreArchivo);
        if (ruta == null) return;

        mciSendString("close MusicaFondo", null, 0, IntPtr.Zero);
        mciSendString($"open \"{ruta}\" type mpegvideo alias MusicaFondo", null, 0, IntPtr.Zero);
        mciSendString($"setaudio MusicaFondo volume to {volumen}", null, 0, IntPtr.Zero);
        mciSendString("play MusicaFondo repeat", null, 0, IntPtr.Zero);
    }

    public static void DetenerMusica()
    {
        mciSendString("close MusicaFondo", null, 0, IntPtr.Zero);
    }

    /// <summary>
    /// Reproduce cualquier efecto .mp3 sin cortar la música de fondo.
    /// </summary>
    public static void ReproducirEfecto(string nombreArchivo, int volumen = 850)
    {
        if (Silenciado) return;

        string? ruta = BuscarRutaAudio(nombreArchivo);
        if (ruta == null) return;

        contadorEfectos = (contadorEfectos + 1) % 30;
        string alias = $"Sfx_{contadorEfectos}";

        mciSendString($"close {alias}", null, 0, IntPtr.Zero);
        mciSendString($"open \"{ruta}\" type mpegvideo alias {alias}", null, 0, IntPtr.Zero);
        mciSendString($"setaudio {alias} volume to {volumen}", null, 0, IntPtr.Zero);
        mciSendString($"play {alias} from 0", null, 0, IntPtr.Zero);
    }
}