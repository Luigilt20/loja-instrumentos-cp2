namespace LojaInstrumentos.Forms;
public class SplashForm : Form
{
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 30 };
    private readonly ProgressBar progresso = new() { Dock = DockStyle.Bottom, Height = 24 };
    public SplashForm()
    {
        Tema.Aplicar(this, "Loja de Instrumentos Musicais", 520, 260);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Controls.Add(new Label { Text = "LOJA DE INSTRUMENTOS\nMUSICAIS\n\nPreparando o sistema...",
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 19, FontStyle.Bold) });
        Controls.Add(progresso);
        timer.Tick += (_, _) =>
        {
            progresso.Value = Math.Min(100, progresso.Value + 4);
            if (progresso.Value == 100) { timer.Stop(); DialogResult = DialogResult.OK; Close(); }
        };
        Shown += (_, _) => timer.Start();
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing) timer.Dispose();
        base.Dispose(disposing);
    }
}
