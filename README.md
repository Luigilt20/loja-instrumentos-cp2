# Loja de Instrumentos Musicais

Checkpoint 2 - C# Development - 3ESPR.
Aplicativo desktop C# / Windows Forms, .NET 8, sem pacotes externos.

## Executar

No Windows, abrir `LojaInstrumentos.csproj` no Visual Studio com a carga de trabalho Desenvolvimento para desktop com .NET e o SDK .NET 8 instalados. Pressionar F5.

Ou, no terminal da pasta do projeto:

```powershell
dotnet restore
dotnet build -c Release
dotnet run --project LojaInstrumentos.csproj
```

## Funcionalidades

- Splash com progresso e abertura do menu.
- Menu com navegação para venda, relatório e saída.
- Venda com cliente, catálogo, quantidade, pagamento, inclusão e remoção de itens, total e confirmação.
- Validação de cliente obrigatório, carrinho vazio e quantidade entre 1 e 999 por instrumento.
- Relatório com pedidos, itens, filtro de datas, quantidade de vendas e total vendido.
- Exportação CSV da consulta, com uma linha por item. O total do pedido se repete por item; para somar o relatório, utilizar Subtotal.
- Persistência em `%LOCALAPPDATA%\LojaInstrumentos\vendas.json`. O arquivo é criado na primeira venda; não há vendas fictícias iniciais.
- Gravação via arquivo temporário antes da atualização da lista em memória.

O catálogo é fixo. Estoque, autenticação e parcelamento não fazem parte do escopo. Utilizar uma instância do aplicativo por vez: não há controle de concorrência entre processos.

## Organização

- `Program.cs`: inicialização e cultura brasileira.
- `Modelos.cs`: instrumento, venda e itens.
- `LojaService.cs`: catálogo, validação e armazenamento JSON.
- `SplashForm.cs`, `MenuForm.cs`, `VendaForm.cs`, `RelatorioForm.cs` e `Tema.cs`: telas e tema visual.
- `integrantes.txt`: identificação exigida na entrega.

## Validação e entrega

A compilação em Windows é verificada pelo workflow .NET Windows. Os cenários de `VALIDACAO.md` devem ser executados para validar a interface.

Repositório público: https://github.com/Luigilt20/loja-instrumentos-cp2

Integrante informado: Luigi Thiengo Pires - RM 560755.

O repositório está na conta pessoal, por solicitação do responsável. O enunciado exige uma organização e equipe de 3 a 4 pessoas; esses requisitos continuam pendentes. O histórico não representa contribuição ou revisão de integrantes que não foram informados.
