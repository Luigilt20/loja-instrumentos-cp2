using LojaInstrumentos.Services;
namespace LojaInstrumentos.Forms;
public class MenuForm : Form
{
    public MenuForm(LojaService loja)
    {
        Tema.Aplicar(this, "Loja de Instrumentos | Menu", 720, 440);
        var painel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
            WrapContents = false, Padding = new Padding(35) };
        painel.Controls.Add(new Label { Text = "Loja de Instrumentos Musicais", AutoSize = true,
            Font = new Font("Segoe UI", 22, FontStyle.Bold), Margin = new Padding(0, 0, 0, 15) });
        painel.Controls.Add(Tema.Rotulo("Selecione uma opção para começar."));
        painel.Controls.Add(Tema.Botao("Nova venda", (_, _) => { using var tela = new VendaForm(loja); tela.ShowDialog(this); }));
        painel.Controls.Add(Tema.Botao("Relatório de vendas", (_, _) => { using var tela = new RelatorioForm(loja); tela.ShowDialog(this); }));
        painel.Controls.Add(Tema.Botao("Sair", (_, _) => Close()));
        Controls.Add(painel);
    }
}
