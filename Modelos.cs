namespace LojaInstrumentos.Models;

public record Instrumento(int Id, string Nome, string Categoria, decimal Preco)
{
    public string Descricao => $"{Nome} - {Preco:C2}";
}

public class ItemVenda
{
    public int InstrumentoId { get; set; }
    public string Instrumento { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
}

public class Venda
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Data { get; set; } = DateTime.Now;
    public string Cliente { get; set; } = "";
    public string Pagamento { get; set; } = "";
    public List<ItemVenda> Itens { get; set; } = new();
    public decimal Total => Itens.Sum(i => i.Subtotal);
}
