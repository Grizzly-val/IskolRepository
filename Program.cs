using IskolRepository.Core;
using IskolRepository.Forms;

namespace IskolRepository;

static class Program
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

        Application.ThreadException += (sender, e) =>
        {
            Console.WriteLine("UI thread exception:");
            Console.WriteLine(e.Exception.ToString());
            MessageBox.Show($"UI thread exception:\n{e.Exception}", "Unhandled UI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                Console.WriteLine("Unhandled domain exception:");
                Console.WriteLine(ex.ToString());
                MessageBox.Show($"Unhandled domain exception:\n{ex}", "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        try
        {
            // Bootstrap services
            var services = ServiceFactory.CreateServices();

            var mainForm = new MainForm(services);
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Application startup failed:");
            Console.WriteLine(ex.ToString());
            MessageBox.Show($"Application startup failed:\n{ex}", "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }    
}
