using System.Globalization;
using LojaInstrumentos.Forms;
using LojaInstrumentos.Services;

namespace LojaInstrumentos;
internal static class Program
{
    [STAThread]
    static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
        ApplicationConfiguration.Initialize();
        try
        {
            var loja = new LojaService();
            using var splash = new SplashForm();
            if (splash.ShowDialog() == DialogResult.OK)
                Application.Run(new MenuForm(loja));
        }
        catch (Exception ex)
        {
            MessageBox.Show("Não foi possível abrir a loja. Os dados existentes foram preservados.\n" + ex.Message,
                "Erro de inicialização", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
