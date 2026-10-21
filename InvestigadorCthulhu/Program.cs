using InvestigadorCthulhu.Datos;
using InvestigadorCthulhu.Formularios;

namespace InvestigadorCthulhu;

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
        try
        {
            BaseDatos.Inicializar();   // crea las tablas de juego la primera vez
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo preparar la base de datos:\n\n" + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Application.Run(new FormPrincipal());
    }
}