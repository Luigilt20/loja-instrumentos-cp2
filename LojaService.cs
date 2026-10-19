using System.Text.Json;
using LojaInstrumentos.Models;

namespace LojaInstrumentos.Services;
public class LojaService
{
    private readonly string arquivo;
    private readonly List<Venda> vendas;
    public IReadOnlyList<Venda> Vendas => vendas.AsReadOnly();
    public IReadOnlyList<Instrumento> Catalogo { get; } = new List<Instrumento>
    {
        new(1, "Violão acústico", "Cordas", 650m),
        new(2, "Guitarra elétrica", "Cordas", 1490m),
        new(3, "Contrabaixo 4 cordas", "Cordas", 1790m),
        new(4, "Teclado 61 teclas", "Teclas", 1200m),
        new(5, "Bateria acústica", "Percussão", 3500m),
        new(6, "Cajón", "Percussão", 420m),
        new(7, "Flauta doce", "Sopros", 65m),
        new(8, "Saxofone alto", "Sopros", 4200m)
    }.AsReadOnly();
    public static readonly string[] Pagamentos = { "Pix", "Dinheiro", "Cartão de débito", "Cartão de crédito" };

    public LojaService()
    {
        arquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LojaInstrumentos", "vendas.json");
        vendas = File.Exists(arquivo)
            ? JsonSerializer.Deserialize<List<Venda>>(File.ReadAllText(arquivo))
                ?? throw new InvalidDataException("Arquivo de vendas inválido.")
            : new List<Venda>();
        if (vendas.Any(v => v is null || v.Itens is null || v.Itens.Any(i => i is null)))
            throw new InvalidDataException("O arquivo de vendas contém registros inválidos.");
    }

    public Venda Registrar(string cliente, string pagamento, IEnumerable<ItemVenda> itens)
    {
        if (string.IsNullOrWhiteSpace(cliente)) throw new ArgumentException("Informe o nome do cliente.");
        if (!Pagamentos.Contains(pagamento)) throw new ArgumentException("Selecione a forma de pagamento.");
        var copia = new List<ItemVenda>();
        foreach (var item in itens)
        {
            var produto = Catalogo.SingleOrDefault(p => p.Id == item.InstrumentoId)
                ?? throw new ArgumentException("Instrumento inválido.");
            if (item.Quantidade < 1 || item.Quantidade > 999) throw new ArgumentException("Quantidade deve estar entre 1 e 999.");
            copia.Add(new ItemVenda { InstrumentoId = produto.Id, Instrumento = produto.Nome,
                Quantidade = item.Quantidade, PrecoUnitario = produto.Preco });
        }
        if (copia.Count == 0) throw new ArgumentException("Adicione pelo menos um instrumento.");
        var venda = new Venda { Cliente = cliente.Trim(), Pagamento = pagamento, Itens = copia };
        var atualizadas = vendas.Append(venda).ToList();
        Directory.CreateDirectory(Path.GetDirectoryName(arquivo)!);
        var temporario = arquivo + ".tmp";
        try
        {
            File.WriteAllText(temporario, JsonSerializer.Serialize(atualizadas, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temporario, arquivo, true);
        }
        finally
        {
            if (File.Exists(temporario)) File.Delete(temporario);
        }
        vendas.Add(venda);
        return venda;
    }
}
