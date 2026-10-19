using System.Text;
using LojaInstrumentos.Models;
using LojaInstrumentos.Services;
namespace LojaInstrumentos.Forms;
public class RelatorioForm : Form
{
    private readonly LojaService loja;
    private readonly DataGridView vendas = Tema.Grade();
    private readonly DataGridView itens = Tema.Grade();
    private readonly DateTimePicker inicio = new() { Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly DateTimePicker fim = new() { Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly CheckBox periodo = new() { Text = "Filtrar período", AutoSize = true };
    private readonly Label resumo = Tema.Rotulo("");
    private List<Venda> resultado = new();
    public RelatorioForm(LojaService loja)
    {
        this.loja = loja;
        Tema.Aplicar(this, "Loja de Instrumentos | Relatório de vendas", 1100, 720);
        inicio.Value = DateTime.Today.AddDays(1 - DateTime.Today.Day); fim.Value = DateTime.Today;
        inicio.Enabled = fim.Enabled = false;
        periodo.CheckedChanged += (_, _) => inicio.Enabled = fim.Enabled = periodo.Checked;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 6, ColumnCount = 1, Padding = new Padding(18) };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        var filtros = new FlowLayoutPanel { Dock = DockStyle.Fill };
        filtros.Controls.AddRange(new Control[] { periodo, Tema.Rotulo("De:"), inicio, Tema.Rotulo("Até:"), fim,
            Tema.Botao("Consultar", (_, _) => Atualizar()) });
        Tema.Coluna(vendas, "Id", "Pedido"); Tema.Coluna(vendas, "Data", "Data", "dd/MM/yyyy HH:mm");
        Tema.Coluna(vendas, "Cliente", "Cliente"); Tema.Coluna(vendas, "Pagamento", "Pagamento"); Tema.Coluna(vendas, "Total", "Total", "C2");
        Tema.Coluna(itens, "Instrumento", "Instrumento"); Tema.Coluna(itens, "Quantidade", "Quantidade");
        Tema.Coluna(itens, "PrecoUnitario", "Preço unitário", "C2"); Tema.Coluna(itens, "Subtotal", "Subtotal", "C2");
        vendas.SelectionChanged += (_, _) => itens.DataSource = (vendas.CurrentRow?.DataBoundItem as Venda)?.Itens.ToList();
        var botoes = new FlowLayoutPanel { Dock = DockStyle.Fill };
        botoes.Controls.AddRange(new Control[] { Tema.Botao("Exportar CSV", (_, _) => Exportar()), Tema.Botao("Voltar", (_, _) => Close()) });
        layout.Controls.Add(filtros, 0, 0); layout.Controls.Add(vendas, 0, 1);
        layout.Controls.Add(Tema.Rotulo("Itens do pedido selecionado"), 0, 2); layout.Controls.Add(itens, 0, 3);
        layout.Controls.Add(resumo, 0, 4); layout.Controls.Add(botoes, 0, 5); Controls.Add(layout);
        Atualizar();
    }
    private void Atualizar()
    {
        if (periodo.Checked && inicio.Value.Date > fim.Value.Date)
        { MessageBox.Show(this, "A data inicial deve ser anterior ou igual à data final."); return; }
        resultado = loja.Vendas.Where(v => !periodo.Checked ||
            (v.Data.Date >= inicio.Value.Date && v.Data.Date <= fim.Value.Date)).OrderByDescending(v => v.Data).ToList();
        vendas.DataSource = null; vendas.DataSource = resultado;
        itens.DataSource = (vendas.CurrentRow?.DataBoundItem as Venda)?.Itens.ToList();
        resumo.Text = $"Vendas: {resultado.Count}  |  Total vendido: {resultado.Sum(v => v.Total):C2}";
    }
    private static string Campo(string valor)
    {
        if (valor.Length > 0 && "=+-@\t\r\n".Contains(valor[0])) valor = "'" + valor;
        return "\"" + valor.Replace("\"", "\"\"") + "\"";
    }
    private void Exportar()
    {
        if (resultado.Count == 0) { MessageBox.Show(this, "Não há vendas na consulta para exportar."); return; }
        using var dialogo = new SaveFileDialog { Filter = "Arquivo CSV|*.csv", FileName = "relatorio-vendas.csv", DefaultExt = "csv", AddExtension = true };
        if (dialogo.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var linhas = new List<string> { "Pedido;Data;Cliente;Pagamento;Instrumento;Quantidade;PrecoUnitario;Subtotal;TotalPedido" };
            foreach (var v in resultado)
                foreach (var i in v.Itens)
                    linhas.Add(string.Join(";", new[] { Campo(v.Id.ToString()), Campo(v.Data.ToString("dd/MM/yyyy HH:mm:ss")),
                        Campo(v.Cliente), Campo(v.Pagamento), Campo(i.Instrumento), i.Quantidade.ToString(),
                        i.PrecoUnitario.ToString("F2"), i.Subtotal.ToString("F2"), v.Total.ToString("F2") }));
            File.WriteAllLines(dialogo.FileName, linhas, new UTF8Encoding(true));
            MessageBox.Show(this, "Relatório exportado com sucesso.");
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Erro na exportação", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}
