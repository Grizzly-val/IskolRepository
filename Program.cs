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

        // Bootstrap services
        var services = ServiceFactory.CreateServices();

        var mainForm = new MainForm(services);

        Application.Run(mainForm);
    }    
}
