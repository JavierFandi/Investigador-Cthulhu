using System.Runtime.InteropServices;

namespace InvestigadorCthulhu.Modelo;

public static class GestorSonido
{
    [DllImport("winmm.dll", CharSet = CharSet.Auto)]
    private static extern int mciSendString(string command, string? buffer, int bufferSize, IntPtr hwndCallback);

    public static bool Silenciado { get; private set; } = false;
    private static int contadorEfectos = 0;

    private static string Ruta(string archivo) =>
        Path.Combine(AppContext.BaseDirectory, "Sonidos", archivo);

    /// <summary>
    /// Reproduce música de fondo en bucle continuo (soporta .mp3 y .wav).
    /// </summary>
    public static void ReproducirMusica(string nombreArchivo = "ambiente.mp3", int volumen = 350)
    {
        if (Silenciado) return;

        string ruta = Ruta(nombreArchivo);
        if (!File.Exists(ruta)) return;

        mciSendString("close MusicaFondo", null, 0, IntPtr.Zero);
        mciSendString($"open \"{ruta}\" type mpegvideo alias MusicaFondo", null, 0, IntPtr.Zero);
        mciSendString($"setaudio MusicaFondo volume to {volumen}", null, 0, IntPtr.Zero); // 0 a 1000
        mciSendString("play MusicaFondo repeat", null, 0, IntPtr.Zero);
    }

    public static void DetenerMusica()
    {
        mciSendString("close MusicaFondo", null, 0, IntPtr.Zero);
    }

    /// <summary>
    /// Reproduce un efecto corto (.wav o .mp3) sin cortar la música de fondo.
    /// </summary>
    public static void ReproducirEfecto(string nombreArchivo, int volumen = 800)
    {
        if (Silenciado) return;

        string ruta = Ruta(nombreArchivo);
        if (!File.Exists(ruta)) return;

        contadorEfectos = (contadorEfectos + 1) % 20;
        string alias = $"Sfx_{contadorEfectos}";

        mciSendString($"close {alias}", null, 0, IntPtr.Zero);
        mciSendString($"open \"{ruta}\" type mpegvideo alias {alias}", null, 0, IntPtr.Zero);
        mciSendString($"setaudio {alias} volume to {volumen}", null, 0, IntPtr.Zero);
        mciSendString($"play {alias}", null, 0, IntPtr.Zero);
    }

    public static bool AlternarSilencio()
    {
        Silenciado = !Silenciado;
        if (Silenciado)
            DetenerMusica();
        else
            ReproducirMusica();

        return Silenciado;
    }
}
