using LojaInstrumentos.Models;
using LojaInstrumentos.Services;
namespace LojaInstrumentos.Forms;
public class VendaForm : Form
{
    private readonly LojaService loja;
    private readonly List<ItemVenda> itens = new();
    private readonly TextBox cliente = new() { Width = 300, MaxLength = 120 };
    private readonly ComboBox pagamento = new() { Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly ComboBox instrumento = new() { Width = 310, DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly NumericUpDown quantidade = new() { Minimum = 1, Maximum = 999, Value = 1, Width = 75 };
    private readonly DataGridView grade = Tema.Grade();
    private readonly Label total = Tema.Rotulo("Total: R$ 0,00");
    public VendaForm(LojaService loja)
    {
        this.loja = loja;
        Tema.Aplicar(this, "Loja de Instrumentos | Nova venda");
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1, Padding = new Padding(18) };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        var topo = new FlowLayoutPanel { Dock = DockStyle.Fill };
        topo.Controls.AddRange(new Control[] { Tema.Rotulo("Cliente:"), cliente, Tema.Rotulo("Pagamento:"), pagamento });
        pagamento.DataSource = LojaService.Pagamentos.ToArray();
        var produtos = new FlowLayoutPanel { Dock = DockStyle.Fill };
        instrumento.DataSource = loja.Catalogo.ToList(); instrumento.DisplayMember = nameof(Instrumento.Descricao);
        produtos.Controls.AddRange(new Control[] { instrumento, Tema.Rotulo("Qtd.:"), quantidade,
            Tema.Botao("Adicionar", (_, _) => Adicionar()), Tema.Botao("Remover item", (_, _) => Remover()) });
        Tema.Coluna(grade, "Instrumento", "Instrumento"); Tema.Coluna(grade, "Quantidade", "Quantidade");
        Tema.Coluna(grade, "PrecoUnitario", "Preço unitário", "C2"); Tema.Coluna(grade, "Subtotal", "Subtotal", "C2");
        var rodape = new FlowLayoutPanel { Dock = DockStyle.Fill };
        total.Font = new Font(Font, FontStyle.Bold);
        rodape.Controls.AddRange(new Control[] { total, Tema.Botao("Finalizar venda", (_, _) => Finalizar()),
            Tema.Botao("Limpar", (_, _) => { if (ConfirmarDescarte()) Limpar(); }), Tema.Botao("Voltar", (_, _) => Close()) });
        layout.Controls.Add(topo, 0, 0); layout.Controls.Add(produtos, 0, 1);
        layout.Controls.Add(grade, 0, 2); layout.Controls.Add(rodape, 0, 3); Controls.Add(layout);
        FormClosing += (_, e) => { if (!ConfirmarDescarte()) e.Cancel = true; };
    }
    private bool ConfirmarDescarte() => (itens.Count == 0 && string.IsNullOrWhiteSpace(cliente.Text)) ||
        MessageBox.Show(this, "Descartar a venda em andamento?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    private void Atualizar()
    {
        grade.DataSource = null; grade.DataSource = itens.ToList();
        total.Text = $"Total: {itens.Sum(i => i.Subtotal):C2}";
    }
    private void Adicionar()
    {
        if (instrumento.SelectedItem is not Instrumento produto) return;
        var existente = itens.FirstOrDefault(i => i.InstrumentoId == produto.Id);
        int qtd = (int)quantidade.Value;
        if ((existente?.Quantidade ?? 0) + qtd > 999)
        { MessageBox.Show(this, "Limite de 999 unidades por instrumento."); return; }
        if (existente != null) existente.Quantidade += qtd;
        else itens.Add(new ItemVenda { InstrumentoId = produto.Id, Instrumento = produto.Nome, Quantidade = qtd, PrecoUnitario = produto.Preco });
        Atualizar();
    }
    private void Remover()
    {
        if (grade.CurrentRow?.DataBoundItem is ItemVenda item) { itens.Remove(item); Atualizar(); }
    }
    private void Limpar() { itens.Clear(); cliente.Clear(); quantidade.Value = 1; pagamento.SelectedIndex = 0; Atualizar(); }
    private void Finalizar()
    {
        try
        {
            var venda = loja.Registrar(cliente.Text, pagamento.Text, itens);
            Limpar();
            MessageBox.Show(this, $"Venda registrada!\nPedido: {venda.Id}\nTotal: {venda.Total:C2}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Não foi possível registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
