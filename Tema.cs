namespace LojaInstrumentos.Forms;
internal static class Tema
{
    public static void Aplicar(Form form, string titulo, int largura = 1000, int altura = 700)
    {
        form.Text = titulo;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.ClientSize = new Size(largura, altura);
        form.MinimumSize = new Size(largura, altura);
        form.Font = new Font("Segoe UI", 10F);
        form.BackColor = Color.FromArgb(245, 247, 250);
        form.AutoScaleMode = AutoScaleMode.Dpi;
    }
    public static Button Botao(string texto, EventHandler acao)
    {
        var b = new Button { Text = texto, AutoSize = true, Height = 42, MinimumSize = new Size(135, 42),
            BackColor = Color.FromArgb(25, 63, 100), ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
            Padding = new Padding(8), Margin = new Padding(5) };
        b.Click += acao;
        return b;
    }
    public static DataGridView Grade() => new()
    {
        Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
        RowHeadersVisible = false, BackgroundColor = Color.White
    };
    public static void Coluna(DataGridView grid, string propriedade, string titulo, string? formato = null)
    {
        var c = new DataGridViewTextBoxColumn { DataPropertyName = propriedade, HeaderText = titulo };
        if (formato != null) c.DefaultCellStyle.Format = formato;
        grid.Columns.Add(c);
    }
    public static Label Rotulo(string texto) => new() { Text = texto, AutoSize = true, Margin = new Padding(6, 12, 6, 5) };
}
